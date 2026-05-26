using UnityEditor;
using UnityEngine;

namespace XiheFramework.Editor.Runtime.Config.PropertyDrawer {
    [CustomPropertyDrawer(typeof(Vector4))]
    public class Vector4Drawer : UnityEditor.PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            property.vector4Value = EditorGUI.Vector4Field(position, label.text, property.vector4Value);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}