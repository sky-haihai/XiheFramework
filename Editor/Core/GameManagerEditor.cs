using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime.Base;

namespace XiheFramework.Editor.Core {
    [CustomEditor(typeof(GameManager))]
    public sealed class GameManagerEditor : UnityEditor.Editor {
        private SerializedProperty m_GameNameProperty;
        private SerializedProperty m_AutoCreateCoreModulesProperty;
        private SerializedProperty m_CoreModulesProperty;
        private SerializedProperty m_CustomGameModulePrefabsProperty;
        private SerializedProperty m_EnableFrameworkDebugProperty;

        private void OnEnable() {
            m_GameNameProperty = serializedObject.FindProperty("m_GameName");
            m_AutoCreateCoreModulesProperty = serializedObject.FindProperty("m_AutoCreateCoreModules");
            m_CoreModulesProperty = serializedObject.FindProperty("m_CoreModules");
            m_CustomGameModulePrefabsProperty = serializedObject.FindProperty("m_CustomGameModulePrefabs");
            m_EnableFrameworkDebugProperty = serializedObject.FindProperty("m_EnableFrameworkDebug");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_GameNameProperty);
            EditorGUILayout.PropertyField(m_EnableFrameworkDebugProperty);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_AutoCreateCoreModulesProperty);

            if (m_AutoCreateCoreModulesProperty.boolValue) {
                DrawCoreModuleSelections();
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_CustomGameModulePrefabsProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCoreModuleSelections() {
            var moduleDefinitions = DiscoverCoreModuleDefinitions()
                .GroupBy(definition => definition.ContractType.AssemblyQualifiedName)
                .OrderBy(group => group.First().ContractType.Name)
                .ToArray();

            SynchronizeCoreModuleSelections(moduleDefinitions);

            EditorGUILayout.LabelField("Core Modules", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope()) {
                foreach (var moduleGroup in moduleDefinitions) {
                    var selectionProperty = FindSelectionProperty(moduleGroup.Key);
                    if (selectionProperty == null) {
                        continue;
                    }

                    var enabledProperty = selectionProperty.FindPropertyRelative("enabled");
                    var implementationTypeNameProperty = selectionProperty.FindPropertyRelative("implementationTypeName");
                    var implementations = moduleGroup.OrderBy(definition => definition.ImplementationType.Name).ToArray();
                    var implementationNames = implementations.Select(definition => definition.ImplementationType.Name).ToArray();
                    var selectedIndex = Array.FindIndex(implementations, definition => definition.ImplementationType.AssemblyQualifiedName == implementationTypeNameProperty.stringValue);
                    if (selectedIndex < 0) {
                        selectedIndex = 0;
                        implementationTypeNameProperty.stringValue = implementations[selectedIndex].ImplementationType.AssemblyQualifiedName;
                    }

                    EditorGUILayout.BeginHorizontal();
                    enabledProperty.boolValue = EditorGUILayout.Toggle(enabledProperty.boolValue, GUILayout.Width(18));
                    EditorGUILayout.LabelField(GetDisplayName(moduleGroup.First().ContractType), GUILayout.Width(180));
                    var newSelectedIndex = EditorGUILayout.Popup(selectedIndex, implementationNames);
                    if (newSelectedIndex != selectedIndex) {
                        implementationTypeNameProperty.stringValue = implementations[newSelectedIndex].ImplementationType.AssemblyQualifiedName;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void SynchronizeCoreModuleSelections(IEnumerable<IGrouping<string, CoreModuleDefinition>> moduleDefinitions) {
            foreach (var moduleGroup in moduleDefinitions) {
                var selectionProperty = FindSelectionProperty(moduleGroup.Key);
                if (selectionProperty != null) {
                    continue;
                }

                var index = m_CoreModulesProperty.arraySize;
                m_CoreModulesProperty.InsertArrayElementAtIndex(index);
                var elementProperty = m_CoreModulesProperty.GetArrayElementAtIndex(index);
                elementProperty.FindPropertyRelative("enabled").boolValue = true;
                elementProperty.FindPropertyRelative("contractTypeName").stringValue = moduleGroup.Key;
                elementProperty.FindPropertyRelative("implementationTypeName").stringValue = moduleGroup.OrderBy(definition => definition.ImplementationType.Name).First().ImplementationType.AssemblyQualifiedName;
            }
        }

        private SerializedProperty FindSelectionProperty(string contractTypeName) {
            for (var i = 0; i < m_CoreModulesProperty.arraySize; i++) {
                var elementProperty = m_CoreModulesProperty.GetArrayElementAtIndex(i);
                if (elementProperty.FindPropertyRelative("contractTypeName").stringValue == contractTypeName) {
                    return elementProperty;
                }
            }

            return null;
        }

        private static IEnumerable<CoreModuleDefinition> DiscoverCoreModuleDefinitions() {
            return TypeCache.GetTypesDerivedFrom<GameModuleBase>()
                .Where(type => !type.IsAbstract)
                .Select(type => new { Type = type, Attribute = type.GetCustomAttribute<XiheCoreModuleAttribute>() })
                .Where(entry => entry.Attribute?.ContractType != null)
                .Select(entry => new CoreModuleDefinition(entry.Attribute.ContractType, entry.Type));
        }

        private static string GetDisplayName(Type contractType) {
            var typeName = contractType.Name;
            const string xiheInterfacePrefix = "IXihe";
            if (typeName.StartsWith(xiheInterfacePrefix, StringComparison.Ordinal)) {
                typeName = typeName.Substring(xiheInterfacePrefix.Length);
            }

            if (typeName.Length > 1 && typeName[0] == 'I' && char.IsUpper(typeName[1])) {
                typeName = typeName.Substring(1);
            }

            const string moduleSuffix = "Module";
            if (typeName.EndsWith(moduleSuffix, StringComparison.Ordinal)) {
                typeName = typeName.Substring(0, typeName.Length - moduleSuffix.Length);
            }

            return typeName;
        }

        private readonly struct CoreModuleDefinition {
            public readonly Type ContractType;
            public readonly Type ImplementationType;

            public CoreModuleDefinition(Type contractType, Type implementationType) {
                ContractType = contractType;
                ImplementationType = implementationType;
            }
        }
    }
}
