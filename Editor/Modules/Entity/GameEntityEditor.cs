using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime.Entity;

namespace XiheFramework.Editor.Runtime.Entity {
    [CustomEditor(typeof(GameEntityBase), true)]
    public class GameEntityEditor : UnityEditor.Editor {
        private GameEntityBase m_Target;
        private SerializedObject m_SerializedObject;

        private SerializedProperty m_PresetEntityId;
        private SerializedProperty m_PresetOwnerId;
        private SerializedProperty m_UseLogicTime;

        private void OnEnable() {
            m_Target = (GameEntityBase)target;
            GetEntityProperties(m_Target, out m_SerializedObject, out m_PresetEntityId, out m_PresetOwnerId, out m_UseLogicTime);
        }
        
        public static void GetEntityProperties(GameEntityBase target, out SerializedObject serializedObject, out SerializedProperty presetEntityId, out SerializedProperty presetOwnerId, out SerializedProperty useLogicTime) {
            serializedObject = new SerializedObject(target);
            presetEntityId = serializedObject.FindProperty("presetEntityId");
            presetOwnerId = serializedObject.FindProperty("presetOwnerId");
            useLogicTime = serializedObject.FindProperty("useLogicTime");
        }

        public static void DrawEntityProperties(GameEntityBase target, SerializedObject serializedObject, SerializedProperty presetEntityId, SerializedProperty presetOwnerId, SerializedProperty useLogicTime) {
            GUILayout.Label("Entity Status", EditorStyles.boldLabel);

            if (!Application.isPlaying) {
                GUI.enabled = false;
            }

            EditorGUILayout.LabelField("Entity ID", Application.isPlaying ? target.EntityId.ToString() : "Not Playing");
            EditorGUILayout.LabelField("Owner ID", Application.isPlaying ? target.OwnerId.ToString() : "Not Playing");
            EditorGUILayout.LabelField("Group Name", target.GroupName);
            EditorGUILayout.LabelField("Full Name", Application.isPlaying ? target.EntityAddress : "Not Playing");
            EditorGUILayout.LabelField("Time Scale", Application.isPlaying ? target.TimeScale.ToString("0.000") : "Not Playing");

            GUI.enabled = true;
            GUILayout.Space(5f);

            GUILayout.Label("Entity Settings", EditorStyles.boldLabel);

            serializedObject.Update();
            //isExpanded = static 
            GUILayout.BeginHorizontal();
            GUI.enabled = presetEntityId.isExpanded;
            EditorGUILayout.PropertyField(presetEntityId);
            GUI.enabled = true;
            if (Application.isPlaying) {
                GUI.enabled = false;
            }

            GUILayout.Label(new GUIContent("Dynamic", "Dynamic mode will let XiheFramework generate a dynamic Entity Id for this entity at runtime"), GUILayout.MaxWidth(55));
            presetEntityId.isExpanded = !EditorGUILayout.Toggle(!presetEntityId.isExpanded, GUILayout.MaxWidth(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            if (!presetEntityId.isExpanded) {
                target.presetEntityId = 0;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUI.enabled = presetOwnerId.isExpanded;
            EditorGUILayout.PropertyField(presetOwnerId);
            GUI.enabled = true;
            if (Application.isPlaying) {
                GUI.enabled = false;
            }

            GUILayout.Label(new GUIContent("Dynamic", "Dynamic mode will let XiheFramework assign a dynamic owner Id if this entity is instantiated with a owner ID at runtime"),
                GUILayout.MaxWidth(55));
            presetOwnerId.isExpanded = !EditorGUILayout.Toggle(!presetOwnerId.isExpanded, GUILayout.MaxWidth(EditorGUIUtility.singleLineHeight));
            GUI.enabled = true;

            if (!presetOwnerId.isExpanded) {
                target.presetOwnerId = 0;
            }

            GUILayout.EndHorizontal();


            EditorGUILayout.PropertyField(useLogicTime);

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(5f);
        }

        public override void OnInspectorGUI() {
            DrawEntityProperties(m_Target, m_SerializedObject, m_PresetEntityId, m_PresetOwnerId, m_UseLogicTime);
            DrawDefaultInspector();
        }
    }
}