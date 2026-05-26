using System;
using XiheFramework.Runtime.Event;
using XiheFramework.Runtime.Utility.DataStructure;

namespace XiheFramework.Runtime.FSM {
    public abstract class BaseState {
        public string StateName => m_StateName;

        protected readonly StateMachine parentStateMachine;

        private readonly MultiDictionary<string, string> m_EventHandlerIds = new();
        private readonly string m_StateName;

        protected BaseState(StateMachine parentStateMachine, string stateName) {
            this.parentStateMachine = parentStateMachine;
            m_StateName = stateName;
        }

        internal void OnEnterInternal() {
            OnEnterCallback();
            Game.GetModule<IXiheEventModule>().Invoke(FsmEvents.OnStateEnterEventName, new FsmEvents.OnStateEnteredEventArgs(parentStateMachine.FsmName, m_StateName));
        }

        internal void OnUpdateInternal() {
            OnUpdateCallback();
        }

        internal void OnExitInternal() {
            foreach (var eventName in m_EventHandlerIds.Keys) {
                UnsubscribeEvent(eventName);
            }

            m_EventHandlerIds.Clear();
            OnExitCallback();
            Game.GetModule<IXiheEventModule>().Invoke(FsmEvents.OnStateExitEventName, new FsmEvents.OnStateExitedEventArgs(parentStateMachine.FsmName, m_StateName));
        }

        protected abstract void OnEnterCallback();
        protected abstract void OnUpdateCallback();
        protected abstract void OnExitCallback();

        public void ChangeState(string targetState) {
            parentStateMachine.ChangeState(targetState);
        }

        protected void SubscribeEvent(string eventName, EventHandler<object> eventHandler) {
            var handlerId = Game.GetModule<IXiheEventModule>().Subscribe(eventName, eventHandler);
            m_EventHandlerIds.Add(eventName, handlerId);
        }

        private void UnsubscribeEvent(string eventName) {
            if (!m_EventHandlerIds.TryGetValue(eventName, out var handlerIds)) {
                return;
            }

            var eventModule = Game.GetModule<IXiheEventModule>();
            foreach (var handlerId in handlerIds) {
                eventModule.Unsubscribe(eventName, handlerId);
            }
        }
    }
}
