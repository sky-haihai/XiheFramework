using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework {
    public class FlowScriptEventModule : GameModule {
        public List<FlowScriptEvent> allEvents;

        private Dictionary<string, FlowScriptEvent> m_AllEvents = new Dictionary<string, FlowScriptEvent>();

        private bool m_IsAnyEventRunning;

        private List<FlowScriptEvent> m_ActiveEvents = new List<FlowScriptEvent>();

        //"FlowEvent.xxx"
        
        public void StartEvent(string eventName) {
            if (m_IsAnyEventRunning) {
                Debug.LogWarning("[FLOW EVENT] Other event is running, you should not allow this to happen");
                return;
            }

            if (m_AllEvents.ContainsKey(eventName)) {
                var e = Instantiate(m_AllEvents[eventName]);
                e.Play();
                m_ActiveEvents.Add(e);
            }
        }

        public void DestroyEvent(string eventName, float delay) {
            if (m_AllEvents.ContainsKey(eventName)) {
                foreach (var activeEvent in m_ActiveEvents) {
                    if (activeEvent.eventName.Equals(eventName)) {
                        Destroy(activeEvent.gameObject, delay);
                    }
                }
            }
        }

        private void Start() {
            m_AllEvents.Clear();
            foreach (var flowEvent in allEvents) {
                m_AllEvents.Add(flowEvent.eventName, flowEvent);
            }

            Game.Event.Subscribe("OnFlowEventInvoked", OnFlowEventInvoked);
            Game.Event.Subscribe("OnFlowEventEnded", OnFlowEventEnded);
        }

        private void OnFlowEventEnded(object sender, object e) {
            m_IsAnyEventRunning = false;
        }

        private void OnFlowEventInvoked(object sender, object e) {
            var ne = (string) e;
            StartEvent(ne);
        }

        public FlowScriptEvent GetEvent(string eventName) {
            if (m_AllEvents.ContainsKey(eventName)) {
                return m_AllEvents[eventName];
            }

            return null;
        }


        public override void Update() {
        }

        public override void ShutDown(ShutDownType shutDownType) {
            foreach (var e in m_ActiveEvents) {
                Destroy(e.gameObject, Time.deltaTime);
            }

            m_ActiveEvents.Clear();
        }
    }
}