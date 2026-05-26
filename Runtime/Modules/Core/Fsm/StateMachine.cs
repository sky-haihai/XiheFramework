using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.String;

namespace XiheFramework.Runtime.FSM {
    public class StateMachine {
        public string FsmName => m_FsmName;

        private string m_FsmName;

        private string CurrentState { get; set; }

        private string m_InitialState;
        private string m_NextState;

        private string m_PreviousState;

        private Dictionary<string, BaseState> m_States = new();

        public static StateMachine Create(string fsmName, string initialState) {
            return new StateMachine { m_FsmName = fsmName, m_InitialState = initialState };
        }

        public static StateMachine Create(string fsmName) {
            return new StateMachine() { m_FsmName = fsmName };
        }

        public int GetStateCount() {
            return m_States.Count;
        }

        public void SetInitialState(string stateName) {
            m_InitialState = stateName;
        }

        public string GetInitialState() {
            return m_InitialState;
        }

        public string GetCurrentState() {
            return CurrentState;
        }

        public string GetPreviousState() {
            return m_PreviousState;
        }

        public void AddState(BaseState state) {
            m_States[state.StateName] = state;
        }

        public void RemoveState(string stateName) {
            if (m_States.ContainsKey(stateName)) m_States.Remove(stateName);
        }

        public void ClearStates() {
            m_States.Clear();
        }

        public void ChangeState(string targetState) {
            //when targetState has not been added to this state machine
            if (!m_States.ContainsKey(targetState)) {
                Debug.LogErrorFormat("[FSM] Current FSM does not contain the target state: {0}", targetState);
                return;
            }

            m_UpdateToExit = true;
            m_PreviousState = CurrentState;
            m_NextState = targetState;
            if (Game.GetModule<XiheStateMachineModule>().enableDebug) Debug.Log("Change to " + targetState);
        }

        #region Life Cycle

        public void OnStart() {
            if (string.IsNullOrEmpty(m_InitialState)) {
                m_InitialState = m_States.Keys.First();
            }

            if (!m_States.ContainsKey(m_InitialState)) {
                Game.LogError("[FSM] Initial state " + m_InitialState + " does not exist");
            }

            if (m_InitialState == null) {
                Game.LogWarning("[FSM] Initial state is null");
            }

            CurrentState = m_InitialState;

            m_ExitToEnter = true;
            // m_States[m_CurrentState].OnEnter();
        }

        public void OnUpdate() {
            if (IsNullOrEmpty(CurrentState)) {
                return;
            }

            if (m_ExitToEnter) {
                m_States[CurrentState].OnEnterInternal();
                m_ExitToEnter = false;
                m_EnterToUpdate = true;
                m_NextFrameLock = false;
            }

            if (m_UpdateToExit) {
                m_States[CurrentState].OnExitInternal();
                if (m_NextState == "") {
                    Game.LogWarning("[FSM] Next state is null or empty");
                }

                CurrentState = m_NextState;
                m_UpdateToExit = false;
                m_ExitToEnter = true;
                m_EnterToUpdate = false;
            }

            if (m_EnterToUpdate && m_NextFrameLock) {
                if (m_States.ContainsKey(CurrentState))
                    m_States[CurrentState].OnUpdateInternal();
                else
                    Debug.Log("[FSM] state " + CurrentState + " does not exist");
            }

            m_NextFrameLock = true;
        }

        public void OnExit() {
            m_States[CurrentState].OnExitInternal();
            CurrentState = Empty;
        }

        #endregion

        #region Locks

        private bool m_ExitToEnter;
        private bool m_EnterToUpdate;
        private bool m_UpdateToExit;
        private bool m_NextFrameLock;

        #endregion
    }
}
