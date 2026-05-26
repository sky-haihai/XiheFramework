using System.Linq;
using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime.Config;

namespace XiheFramework.Editor.Runtime.Config {
    [CustomEditor(typeof(ConfigModule))]
    public class ConfigModuleEditor : UnityEditor.Editor {
        private SerializedObject m_Target;

        private void OnEnable() {
            var configModule = (ConfigModule)target;
            m_Target = new SerializedObject(configModule);
        }

        public override void OnInspectorGUI() {
            m_Target.Update();
            EditorGUILayout.PropertyField(m_Target.FindProperty("updateInterval"));
            EditorGUILayout.PropertyField(m_Target.FindProperty("fixedUpdateInterval"));
            EditorGUILayout.PropertyField(m_Target.FindProperty("lateUpdateInterval"));
            EditorGUILayout.PropertyField(m_Target.FindProperty("enableDebug"));
            m_Target.ApplyModifiedProperties();

            if (GUILayout.Button("Open Config Editor", GUILayout.Height(40))) {
                ConfigModuleEditorWindow.ShowWindow(m_Target);
            }

            if (GUILayout.Button("Retrieve Config Attributes", GUILayout.Height(40))) {
                AddNewFoundConfigs();
            }
        }

        private void AddNewFoundConfigs() {
            var configModule = (ConfigModule)target;
            var allConfigInfo = ConfigEditorHelper.FindAllConfigInfo();
            var configDic = configModule.configSettings.ToDictionary(x => x.path, x => x);
            foreach (var configInfo in allConfigInfo) {
                if (configDic.ContainsKey(configInfo.path)) {
                    continue;
                }

                var presetConfigEntry = new PresetConfigEntry();
                presetConfigEntry.path = configInfo.path;
                presetConfigEntry.type = configInfo.type;
                if (configInfo.defaultValue != null) {
                    presetConfigEntry.SetValue(configInfo.defaultValue);
                }

                configModule.configSettings.Add(presetConfigEntry);
            }
        }
    }
}