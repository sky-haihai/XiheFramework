using UnityEditor;
using UnityEngine;

namespace XiheFramework.Editor.Runtime.Config.PropertyDrawer {
    [CustomPropertyDrawer(typeof(AnimationCurve))]
    public class AnimationCurveDrawer : UnityEditor.PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}