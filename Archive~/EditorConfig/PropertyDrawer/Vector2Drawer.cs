using UnityEditor;
using UnityEngine;

namespace XiheFramework.Editor.Runtime.Config.PropertyDrawer {
    [CustomPropertyDrawer(typeof(Vector2))]
    public class Vector2Drawer : UnityEditor.PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            property.vector2Value = EditorGUI.Vector2Field(position, label.text, property.vector2Value);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}