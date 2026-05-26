using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime.Entity;

namespace XiheFramework.Editor.Runtime.Entity {
    [CustomEditor(typeof(XiheEntityModule))]
    public class EntityModuleEditor : UnityEditor.Editor {
        private SerializedObject m_Target;
        private SerializedObject m_EditorTarget;

        [SerializeField]
        private List<EntityDebugInfo> entityDebugInfo = new List<EntityDebugInfo>();

        [Serializable]
        private struct EntityDebugInfo {
            public uint id;
            public uint ownerId;
            public string groupName;
            public string fullName;
            public float timeScale;
            public GameObject instance;

            public EntityDebugInfo(uint id, uint ownerId, string groupName, string fullName, float timeScale, GameObject instance) {
                this.id = id;
                this.ownerId = ownerId;
                this.groupName = groupName;
                this.fullName = fullName;
                this.timeScale = timeScale;
                this.instance = instance;
            }
        }

        private void OnEnable() {
            var p = (XiheEntityModule)target;
            m_Target = new SerializedObject(p);
            m_EditorTarget = new SerializedObject(this);
        }

        public override void OnInspectorGUI() {
            m_Target.Update();
            EditorGUILayout.PropertyField(m_Target.FindProperty("updateInterval"));
            EditorGUILayout.PropertyField(m_Target.FindProperty("fixedUpdateInterval"));
            EditorGUILayout.PropertyField(m_Target.FindProperty("lateUpdateInterval"));
            EditorGUILayout.PropertyField(m_Target.FindProperty("enableDebug"));
            m_Target.ApplyModifiedProperties();

            GUILayout.Space(5f);
            entityDebugInfo.Clear();
            if (Application.isPlaying) {
                var module = m_Target.targetObject as XiheEntityModule;
                if (module != null) {
                    foreach (var entity in module.CurrentEntities) {
                        var debugInfo = new EntityDebugInfo(entity.EntityId, entity.OwnerId, entity.GroupName, entity.EntityAddress, entity.TimeScale, entity.gameObject);
                        entityDebugInfo.Add(debugInfo);
                    }

                    GUI.enabled = false;
                    m_EditorTarget.Update();
                    EditorGUILayout.PropertyField(m_EditorTarget.FindProperty("entityDebugInfo"), new GUIContent("Live Entity Info"), true);
                    GUI.enabled = true;
                }
                else {
                    EditorGUILayout.HelpBox("Entity Module Not Found", MessageType.Error);
                }
            }
            else {
                GUILayout.Label("Live Entity Info", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Editor Not Playing", MessageType.Info);
            }

            Repaint();
        }
    }
}