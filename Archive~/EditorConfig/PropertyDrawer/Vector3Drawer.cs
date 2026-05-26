using UnityEditor;
using UnityEngine;

namespace XiheFramework.Editor.Runtime.Config.PropertyDrawer {
    [CustomPropertyDrawer(typeof(Vector3))]
    public class Vector3Drawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.vector3Value = EditorGUI.Vector3Field(position, label.text, property.vector3Value);
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}