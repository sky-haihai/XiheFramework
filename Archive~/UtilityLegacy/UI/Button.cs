using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace XiheFramework.Utility.UI {
    public class Button : UnityEngine.UI.Button {
        public Sprite normalImage;
        public Sprite pressedImage;
        
        public override void OnPointerDown(PointerEventData eventData) {
            base.OnPointerDown(eventData);
            if (pressedImage != null) {
                image.sprite = pressedImage;
            }
        }

        public override void OnPointerUp(PointerEventData eventData) {
            base.OnPointerUp(eventData);
            if (normalImage != null) {
                image.sprite = normalImage;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Button), true)]
    public class ButtonEditor : UnityEditor.UI.ButtonEditor {
        private SerializedProperty m_NormalImageProperty;
        private SerializedProperty m_PressedImageProperty;

        protected override void OnEnable() {
            base.OnEnable();

            m_NormalImageProperty = serializedObject.FindProperty("normalImage");
            m_PressedImageProperty = serializedObject.FindProperty("pressedImage");
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_NormalImageProperty);
            EditorGUILayout.PropertyField(m_PressedImageProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}