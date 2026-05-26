using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime;
using XiheFramework.Runtime.FSM;
using XiheFramework.Runtime.Utility.DataStructure;

namespace XiheFramework.Editor.Runtime.Fsm {
    public class FsmDebugWindow : EditorWindow {
        private int m_IterationIndex;

        //string[] m_Options = {"Group", "Type"};
        //private int m_SortingMode;
        private Vector2 m_ScrollPos;

        private TreeNode<bool> m_States;

        [MenuItem("XiheFramework/FSM Debug Window")]
        private static void Init() {
            // Get existing open window or if none, make a new one:
            var window = (FsmDebugWindow)GetWindow(typeof(FsmDebugWindow));
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

            if (!Game.TryGetModule<IXiheStateMachineModule>(out var fsmModule)) {
                GUILayout.Label("DATA STATUS : MISSING XIHE FRAMEWORK", EditorStyles.boldLabel);
                return;
            }

            var data = fsmModule.GetData();
            if (data == null) {
                GUILayout.Label("DATA STATUS : DATA NULL", EditorStyles.boldLabel);
                return;
            }

            GUILayout.Label("DATA STATUS : LIVE", EditorStyles.boldLabel);

            //m_SortingMode = EditorGUILayout.Popup("Sorting Mode:", m_SortingMode, m_Options);

            GUILayout.Space(10f);

            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("State Machine", EditorStyles.boldLabel);
            GUILayout.Label("Current State", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            foreach (var key in data.Keys) {
                GUILayout.BeginHorizontal();
                GUILayout.Label(key, EditorStyles.boldLabel);
                GUILayout.Label(data[key].GetCurrentState());
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10f);

            EditorGUILayout.EndScrollView();
        }
    }
}
