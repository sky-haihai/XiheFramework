using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XiheFramework.Runtime.Config;

namespace XiheFramework.Editor.Runtime.Config {
    public class ConfigModuleEditorWindow : EditorWindow {
        private SerializedObject m_Target;
        private SerializedProperty m_ConfigSettings;

        private ReorderableList m_ReorderableList;

        public static void ShowWindow(SerializedObject configModule) {
            var window = GetWindow<ConfigModuleEditorWindow>("Config Editor");
            window.m_Target = configModule;
            window.InitList();
        }

        private void InitList() {
            m_ConfigSettings = m_Target.FindProperty("configSettings");
            m_ReorderableList = new ReorderableList(m_Target, m_ConfigSettings, true, true, true, true);
            m_ReorderableList.drawHeaderCallback = OnDrawHeader;
            m_ReorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            m_ReorderableList.drawElementCallback = OnDrawElement;
            m_ReorderableList.elementHeightCallback = OnElementHeight;
            m_ReorderableList.onAddCallback = OnAddElement;
        }

        private void OnAddElement(ReorderableList list) {
            ReorderableList.defaultBehaviours.DoAddButton(list);

            var element = m_ConfigSettings.GetArrayElementAtIndex(list.count - 1);
            element.FindPropertyRelative("path").stringValue = $"NewConfig_{list.count}";
            element.FindPropertyRelative("type").enumValueIndex = (int)ConfigType.String;
        }

        private float OnElementHeight(int index) {
            var element = m_ConfigSettings.GetArrayElementAtIndex(index);
            var type = element.FindPropertyRelative("type");
            switch ((ConfigType)type.enumValueIndex) {
                case ConfigType.Integer: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("intValue"));
                case ConfigType.Boolean: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("boolValue"));
                case ConfigType.Float: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("floatValue"));
                case ConfigType.String: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("stringValue"));
                case ConfigType.Color: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("colorValue"));
                case ConfigType.LayerMask: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("layerMaskValue"));
                case ConfigType.Vector2: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("vector2Value"));
                case ConfigType.Vector3: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("vector3Value"));
                case ConfigType.Vector4: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("vector4Value"));
                case ConfigType.AnimationCurve: return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(element.FindPropertyRelative("animationCurveValue"));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDrawHeader(Rect rect) {
            EditorGUI.LabelField(rect, "Preset Configs");
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty element = m_ConfigSettings.GetArrayElementAtIndex(index);
            var path = element.FindPropertyRelative("path");
            var type = element.FindPropertyRelative("type");

            EditorGUI.LabelField(new Rect(rect.x, rect.y, 70, EditorGUIUtility.singleLineHeight), "Path");
            EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), path, GUIContent.none);

            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 70, EditorGUIUtility.singleLineHeight), "Type");
            EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight, rect.width - 70, EditorGUIUtility.singleLineHeight), type, GUIContent.none);

            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 70, EditorGUIUtility.singleLineHeight), "Value");
            switch ((ConfigType)type.enumValueIndex) {
                case ConfigType.Integer:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("intValue"), GUIContent.none);
                    break;
                case ConfigType.Boolean:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("boolValue"), GUIContent.none);
                    break;
                case ConfigType.Float:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("floatValue"), GUIContent.none);
                    break;
                case ConfigType.String:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("stringValue"), GUIContent.none);
                    break;
                case ConfigType.Color:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("colorValue"), GUIContent.none);
                    break;
                case ConfigType.LayerMask:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("layerMaskValue"),
                        GUIContent.none);
                    break;
                case ConfigType.Vector2:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("vector2Value"), GUIContent.none);
                    break;
                case ConfigType.Vector3:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("vector3Value"), GUIContent.none);
                    break;
                case ConfigType.Vector4:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("vector4Value"), GUIContent.none);
                    break;
                case ConfigType.AnimationCurve:
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("animationCurveValue"), GUIContent.none);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnGUI() {
            GUILayout.Label("Config Editor", EditorStyles.boldLabel);

            if (Application.isPlaying) {
                EditorGUILayout.HelpBox("Changes made during Play Mode will not be saved", MessageType.Warning);
            }

            if (m_Target == null) {
                EditorGUILayout.HelpBox("Open Config Editor from ConfigModule", MessageType.Warning);
                return;
            }

            if (m_ReorderableList == null) {
                InitList();
            }

            if (m_ReorderableList == null) {
                EditorGUILayout.HelpBox("Open Config Editor from ConfigModule", MessageType.Error);
                return;
            }

            if (m_ConfigSettings.serializedObject != null) {
                m_ConfigSettings.serializedObject.Update();
                m_ReorderableList.DoLayoutList();
                m_ConfigSettings.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}