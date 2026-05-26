using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.FSM {
    [XiheCoreModule(typeof(IXiheStateMachineModule))]
    public class XiheStateMachineModule : GameModuleBase, IXiheStateMachineModule {
        public override int Priority => (int)CoreModulePriority.Fsm;

        private readonly Dictionary<string, StateMachine> m_StateMachines = new();

        private HashSet<string> m_PendingRemoveStateMachines = new();
        private bool m_IsActive = true;

        protected override void OnUpdate() {
            if (!m_IsActive) return;

            var pendingRemoveStateMachines = m_PendingRemoveStateMachines.ToArray();
            foreach (var fsmName in pendingRemoveStateMachines) {
                m_StateMachines.Remove(fsmName);
                m_PendingRemoveStateMachines.Remove(fsmName);
            }

            var keys = m_StateMachines.Keys.ToArray();
            foreach (var key in keys) {
                if (!m_StateMachines.ContainsKey(key)) {
                    Debug.LogWarning($"[FSM] Fsm: {key} does not exist");
                }
                else if (m_StateMachines[key] == null) {
                    Debug.LogWarning($"[FSM] Fsm: {key} is null");
                }
                else {
                    var stateMachine = m_StateMachines[key];
                    stateMachine.OnUpdate();
                }
            }
        }

        public Dictionary<string, StateMachine> GetData() {
            return m_StateMachines;
        }

        public string GetCurrentState(string fsmName) {
            if (m_StateMachines.ContainsKey(fsmName)) {
                return m_StateMachines[fsmName].GetCurrentState();
            }

            // Debug.LogError("[FSM] Fsm: " + fsmName + " does not exist");
            return null;
        }

        public void SetInitialState(string fsmName, string stateName) {
            if (!IsFsmExisted(fsmName)) return;

            m_StateMachines[fsmName].SetInitialState(stateName);
        }

        public void ChangeState(string fsmName, string stateName) {
            if (!IsFsmExisted(fsmName)) return;

            m_StateMachines[fsmName].ChangeState(stateName);
        }

        public void AddState(string fsmName, BaseState state) {
            if (!IsFsmExisted(fsmName)) return;

            m_StateMachines[fsmName].AddState(state);
        }

        public void AddActionState(string fsmName, string stateName, Action onEnter, Action onUpdate, Action onExit) {
            if (!IsFsmExisted(fsmName)) return;

            var state = new ActionState(m_StateMachines[fsmName], stateName, onEnter, onUpdate, onExit);

            m_StateMachines[fsmName].AddState(state);
        }

        public void RemoveState(string fsmName, string stateName) {
            if (!IsFsmExisted(fsmName)) return;

            m_StateMachines[fsmName].RemoveState(stateName);
        }

        public bool IsFsmExisted(string fsmName) {
            return m_StateMachines.ContainsKey(fsmName) && !m_PendingRemoveStateMachines.Contains(fsmName);
        }

        public StateMachine CreateStateMachine(string fsmName) {
            var fsm = StateMachine.Create(fsmName);
            if (m_StateMachines.ContainsKey(fsmName)) {
                Debug.LogWarningFormat("[FSM] Fsm with name: {0} has already existed, replacing it..", fsmName);
                m_StateMachines[fsmName] = fsm;
            }
            else {
                m_StateMachines.Add(fsmName, fsm);
            }

            m_PendingRemoveStateMachines.Remove(fsmName);
            return fsm;
        }

        public void RemoveStateMachine(string fsmName) {
            if (!IsFsmExisted(fsmName)) {
                Debug.LogWarningFormat("[FSM] Can not remove fsm with name: {0} because it does not exist", fsmName);
                return;
            }

            if (m_StateMachines.TryGetValue(fsmName, out var stateMachine)) {
                stateMachine.OnExit();
                m_PendingRemoveStateMachines.Add(fsmName);
            }
        }

        public void StartStateMachine(string fsmName) {
            if (m_StateMachines.TryGetValue(fsmName, out var stateMachine)) {
                stateMachine.OnStart();
            }
            else {
                Debug.LogWarningFormat("[FSM] Can not start fsm with name: {0} because it does not exist", fsmName);
            }
        }

        public void StopStateMachine(string fsmName) {
            if (m_StateMachines.TryGetValue(fsmName, out var stateMachine)) {
                stateMachine.OnExit();
            }
            else {
                Debug.LogWarningFormat("[FSM] Can not stop fsm with name: {0} because it does not exist", fsmName);
            }
        }

        /// <summary>
        /// stop fsm and remove all states for that fsm
        /// </summary>
        public void ClearStates(string fsmName) {
            if (!IsFsmExisted(fsmName)) {
                return;
            }

            StopStateMachine(fsmName);
            m_StateMachines[fsmName].ClearStates();
        }

        public void StopAllStateMachines() {
            m_IsActive = false;
        }

        public void StartAllStateMachines() {
            m_IsActive = true;
        }

        protected override void OnInstantiated() {
            base.OnInstantiated();
        }

        protected override void OnDestroyed() {
            base.OnDestroyed();
            m_StateMachines.Clear();
            m_PendingRemoveStateMachines.Clear();
        }
    }
}
