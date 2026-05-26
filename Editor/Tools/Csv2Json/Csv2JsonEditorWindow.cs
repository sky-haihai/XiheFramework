using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XiheFramework.Runtime.Utility.Csv2Json;

namespace XiheFramework.Editor.Tools.Csv2Json {
    public class Csv2JsonEditorWindow : EditorWindow {
        private static CsvListHolder m_Target;
        private SerializedObject m_SerializedObject;
        private SerializedProperty m_CsvTypePairsListProp;

        // 常量定义
        private string m_OutputPath = "Assets/AddressableResources/CsvToJsonData/"; // JSON输出文件夹路径
        private static ReorderableList m_CsvTypePairsReorderableList;

        //open window
        [MenuItem("XiheFramework/Csv2Json")]
        private static void ShowWindow() {
            // Get existing open window or if none, make a new one:
            var window = (Csv2JsonEditorWindow)GetWindow(typeof(Csv2JsonEditorWindow));
            window.Show();
        }

        private void OnEnable() {
            if (m_Target == null) {
                m_Target = ScriptableObject.CreateInstance<CsvListHolder>();
            }

            m_SerializedObject = new SerializedObject(m_Target);
            m_CsvTypePairsListProp = m_SerializedObject.FindProperty("csvTypePairs");
            InitReorderableList();
        }

        private void InitReorderableList() {
            m_CsvTypePairsReorderableList = new ReorderableList(m_SerializedObject, m_CsvTypePairsListProp, true, true, true, true);
            m_CsvTypePairsReorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            m_CsvTypePairsReorderableList.drawElementCallback = OnDrawElement;
            m_CsvTypePairsReorderableList.onAddCallback = OnAddElement;
            m_CsvTypePairsReorderableList.elementHeightCallback = (int index) => EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            var element = m_CsvTypePairsListProp.GetArrayElementAtIndex(index);
            var csvAsset = element.FindPropertyRelative("csvAsset");
            var dataType = element.FindPropertyRelative("infoType");
            var firstLine = element.FindPropertyRelative("firstLine");
            var pathRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(pathRect, csvAsset, new GUIContent("CSV Asset"));
            var typeRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(typeRect, dataType, new GUIContent("Table Type"));
            var firstLineRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(firstLineRect, firstLine, new GUIContent("First Line"));
        }

        private void OnAddElement(ReorderableList list) {
            ReorderableList.defaultBehaviours.DoAddButton(list);

            var element = m_CsvTypePairsListProp.GetArrayElementAtIndex(list.count - 1);
            var infoTypeProp = element.FindPropertyRelative("infoType");
            infoTypeProp.FindPropertyRelative("infoTypeIndex").intValue = 0;
            infoTypeProp.FindPropertyRelative("fullName").stringValue = "Empty";
        }

        private void OnGUI() {
            m_OutputPath = EditorGUILayout.TextField("Output Path", m_OutputPath);

            if (m_CsvTypePairsReorderableList == null) {
                InitReorderableList();
            }

            if (m_CsvTypePairsListProp != null && m_CsvTypePairsListProp.serializedObject != null) {
                m_CsvTypePairsListProp.serializedObject.Update();
                m_CsvTypePairsReorderableList?.DoLayoutList();
                m_CsvTypePairsListProp.serializedObject.ApplyModifiedProperties();
            }


            if (GUILayout.Button("Convert")) {
                ConvertAll();
            }
        }

        private void ConvertAll() {
            if (m_Target == null || m_Target.csvTypePairs == null || m_Target.csvTypePairs.Count == 0) {
                return;
            }

            foreach (var csvTypePair in m_Target.csvTypePairs) {
                var method = typeof(Csv2JsonHelper).GetMethod(nameof(Csv2JsonHelper.ConvertCsv2Json), BindingFlags.Public | BindingFlags.Static);
                if (method == null) continue;
                var infoType = ResolveType(csvTypePair.infoType.fullName);
                if (infoType == null) {
                    Debug.LogError($"Cannot resolve CSV info type: {csvTypePair.infoType.fullName}");
                    continue;
                }

                var generic = method.MakeGenericMethod(infoType);
                var filePath = AssetDatabase.GetAssetPath(csvTypePair.csvAsset);
                if (string.IsNullOrEmpty(filePath)) {
                    Debug.LogError("CSV asset is missing.");
                    continue;
                }

                generic.Invoke(null, new object[] { filePath, m_OutputPath, csvTypePair.firstLine });
            }
        }

        public static Type ResolveType(string fullName) {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => t.FullName == fullName);
        }

        private class CsvListHolder : ScriptableObject {
            public List<CsvTypePair> csvTypePairs = new List<CsvTypePair>();
        }

        [Serializable]
        private class CsvTypePair {
            public TextAsset csvAsset;
            public InfoType infoType;
            public int firstLine = 3;
        }

        [Serializable]
        private class InfoType {
            public int infoTypeIndex;
            public string fullName;
        }

        [CustomPropertyDrawer(typeof(InfoType))]
        private class InfoTypesPopupDrawer : PropertyDrawer {
            private List<Type> m_CsvInfoTypes;
            private string[] m_CsvInfoTypeNames;

            private void LoadCsvInfoTypes() {
                m_CsvInfoTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.GetCustomAttribute<CsvInfoAttribute>() != null && !t.IsAbstract)
                    .ToList();

                m_CsvInfoTypeNames = m_CsvInfoTypes.Select(t => t.Name).ToArray();
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                if (m_CsvInfoTypes == null) {
                    LoadCsvInfoTypes();
                }

                var infoTypeIndexProp = property.FindPropertyRelative("infoTypeIndex");
                var typeFullNameProp = property.FindPropertyRelative("fullName");

                if (m_CsvInfoTypes == null || m_CsvInfoTypes.Count == 0) {
                    infoTypeIndexProp.intValue = EditorGUI.Popup(position, label.text, 0, new[] { "Empty" });
                }
                else {
                    var clampedIndex = Mathf.Clamp(infoTypeIndexProp.intValue, 0, m_CsvInfoTypes.Count - 1);
                    infoTypeIndexProp.intValue = EditorGUI.Popup(position, label.text, clampedIndex, m_CsvInfoTypeNames);
                    typeFullNameProp.stringValue = m_CsvInfoTypes[infoTypeIndexProp.intValue].FullName;
                }
            }
        }
    }
}
