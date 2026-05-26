using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime;
using XiheFramework.Runtime.Event;
using XiheFramework.Runtime.Utility.DataStructure;

namespace XiheFramework.Editor.Runtime.Event {
    public class EventDebugWindow : EditorWindow {
        private int m_IterationIndex;

        //string[] m_Options = {"Group", "Type"};
        //private int m_SortingMode;
        private Vector2 m_EventsScrollPos;
        private Vector2 m_HandlersScrollPos;

        private TreeNode<bool> m_States;

        [MenuItem("XiheFramework/Events Debug Window")]
        private static void Init() {
            // Get existing open window or if none, make a new one:
            var window = (EventDebugWindow)GetWindow(typeof(EventDebugWindow));
            window.Show();
        }

        private void Update() {
            Repaint();
        }
        
        private void OnGUI() {
            if (!Application.isPlaying) {
                GUILayout.Label("DATA STATUS : NOT IN PLAY MODE", EditorStyles.boldLabel);
                //m_SortingMode = EditorGUILayout.Popup("Sorting Mode:", m_SortingMode, m_Options);
                return;
            }

            if (!Game.TryGetModule<XiheEventModule>(out var eventModule)) {
                GUILayout.Label("DATA STATUS : MISSING XIHE FRAMEWORK", EditorStyles.boldLabel);
                return;
            }

            var events = eventModule.GetEvents();
            if (events == null) {
                GUILayout.Label("DATA STATUS : EVENTS NULL", EditorStyles.boldLabel);
                return;
            }

            var handlers = eventModule.GetHandlers();
            if (handlers == null) {
                GUILayout.Label("DATA STATUS : HANDLERS NULL", EditorStyles.boldLabel);
                return;
            }

            GUILayout.Label("DATA STATUS : LIVE", EditorStyles.boldLabel);

            //m_SortingMode = EditorGUILayout.Popup("Sorting Mode:", m_SortingMode, m_Options);

            GUILayout.Space(10f);

            m_EventsScrollPos = EditorGUILayout.BeginScrollView(m_EventsScrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Event Name", EditorStyles.boldLabel);
            GUILayout.Label("Handler Count", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            foreach (var key in events.Keys) {
                GUILayout.BeginHorizontal();
                GUILayout.Label(key, EditorStyles.boldLabel);
                GUILayout.Label(events[key].Count.ToString(), EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10f);

            EditorGUILayout.EndScrollView();

            m_HandlersScrollPos = EditorGUILayout.BeginScrollView(m_HandlersScrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Handler Count:", EditorStyles.boldLabel);
            GUILayout.Label(handlers.Count.ToString(), EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.Space(10f);

            EditorGUILayout.EndScrollView();
        }
    }
}
