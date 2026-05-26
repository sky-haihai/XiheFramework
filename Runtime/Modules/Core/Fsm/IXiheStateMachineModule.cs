using System;
using System.Collections.Generic;

namespace XiheFramework.Runtime.FSM {
    public interface IXiheStateMachineModule {
        Dictionary<string, StateMachine> GetData();
        string GetCurrentState(string fsmName);
        void SetInitialState(string fsmName, string stateName);
        void ChangeState(string fsmName, string stateName);
        void AddState(string fsmName, BaseState state);
        void AddActionState(string fsmName, string stateName, Action onEnter, Action onUpdate, Action onExit);
        void RemoveState(string fsmName, string stateName);
        bool IsFsmExisted(string fsmName);
        StateMachine CreateStateMachine(string fsmName);
        void RemoveStateMachine(string fsmName);
        void StartStateMachine(string fsmName);
        void StopStateMachine(string fsmName);
        void ClearStates(string fsmName);
        void StopAllStateMachines();
        void StartAllStateMachines();
    }
}
