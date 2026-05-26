using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime.UI;

namespace XiheFramework.Editor.Runtime.UI {
    [CustomEditor(typeof(XiheUIModule))]
    public class UIModuleEditor : UnityEditor.Editor {
        private SerializedObject m_SerializedObject;
        private SerializedObject m_EditorSerializedObject;
        private XiheUIModule m_UIModule;

        [SerializeField]
        private List<XiheUIModule.PageReturnHistoryInfo> pageHistoryDebug = new();

        // private SerializedProperty m_PageCanvasProp;
        // private SerializedProperty m_PopupCanvasProp;
        // private SerializedProperty m_OverlayCanvasProp;
        private SerializedProperty m_PageHistoryDebugProp;

        [Serializable]
        private struct UIDebugInfo {
            public uint id;
            public string fullName;
            public bool destroyOnClose;
            public GameObject instance;

            public UIDebugInfo(uint id, string fullName, bool destroyOnClose, GameObject instance) {
                this.id = id;
                this.fullName = fullName;
                this.destroyOnClose = destroyOnClose;
                this.instance = instance;
            }
        }

        private void OnEnable() {
            m_UIModule = (XiheUIModule)target;
            m_SerializedObject = new SerializedObject(m_UIModule);
            m_EditorSerializedObject = new SerializedObject(this);

            // m_PageCanvasProp = m_SerializedObject.FindProperty("pageCanvas");
            // m_PopupCanvasProp = m_SerializedObject.FindProperty("popupCanvas");
            // m_OverlayCanvasProp = m_SerializedObject.FindProperty("overlayCanvas");

            m_PageHistoryDebugProp = m_EditorSerializedObject.FindProperty("pageHistoryDebug");
        }

        public override void OnInspectorGUI() {
            m_SerializedObject.Update();
            DrawDefaultInspector();

            // if (m_PageCanvasProp.objectReferenceValue == null) {
            //     var child = m_UIModule.transform.Find("PageCanvas");
            //     if (child) {
            //         m_PageCanvasProp.objectReferenceValue = child.GetComponent<Canvas>();
            //     }
            //
            //     if (m_PageCanvasProp.objectReferenceValue == null) {
            //         EditorGUILayout.HelpBox("Page Canvas not found, Set it manually.", MessageType.Error);
            //     }
            // }
            //
            // if (m_PopupCanvasProp.objectReferenceValue == null) {
            //     var child = m_UIModule.transform.Find("PopupCanvas");
            //     if (child) {
            //         m_PopupCanvasProp.objectReferenceValue = child.GetComponent<Canvas>();
            //     }
            //
            //     if (m_PopupCanvasProp.objectReferenceValue == null) {
            //         EditorGUILayout.HelpBox("Popup Canvas not found, Set it manually.", MessageType.Error);
            //     }
            // }
            //
            // if (m_OverlayCanvasProp.objectReferenceValue == null) {
            //     var child = m_UIModule.transform.Find("OverlayCanvas");
            //     if (child) {
            //         m_OverlayCanvasProp.objectReferenceValue = child.GetComponent<Canvas>();
            //     }
            //
            //     if (m_OverlayCanvasProp.objectReferenceValue == null) {
            //         EditorGUILayout.HelpBox("Overlay Canvas not found, Set it manually.", MessageType.Error);
            //     }
            // }

            m_SerializedObject.ApplyModifiedProperties();

            GUILayout.Space(5f);
            GUILayout.Label("Page Layer", EditorStyles.boldLabel);

            pageHistoryDebug.Clear();
            pageHistoryDebug = m_UIModule.CurrentPageHistory.ToList();
            GUI.enabled = false;
            m_EditorSerializedObject.Update();
            EditorGUILayout.PropertyField(m_PageHistoryDebugProp, new GUIContent("Page Return History"), true);
            GUI.enabled = true;
            Repaint();
        }
    }
}