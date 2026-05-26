using UnityEditor;
using UnityEngine;
using XiheFramework.Utility.GOArray;

namespace XiheFramework.Editor.Utility.GOArray {
    [CustomEditor(typeof(GoArray))]
    public class GoArrayEditor : UnityEditor.Editor {
        private GoArray m_Target;

        private void OnEnable() {
            m_Target = (GoArray)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update")) m_Target.UpdateModifier();

            if (GUILayout.Button("Clear")) m_Target.ClearModifier();
        }
    }
}