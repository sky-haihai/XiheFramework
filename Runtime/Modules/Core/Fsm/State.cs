using System;

namespace XiheFramework.Runtime.FSM {
    [Serializable]
    public abstract class State<T> : BaseState {
        protected T owner;

        protected State(StateMachine parentStateMachine, string stateName, T owner) : base(parentStateMachine, stateName) {
            this.owner = owner;
        }
    }
}