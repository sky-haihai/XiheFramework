﻿using System;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework.Core.Base;
using XiheFramework.Core.Utility.DataStructure;
using static System.String;

namespace XiheFramework.Core.Event {
    public class EventModule : GameModule {
        private readonly MultiDictionary<string, string> m_CurrentEvents = new();
        private readonly Dictionary<string, EventHandler<object>> m_ActiveEventHandlers = new(); //keep track of active event handlers for unsubscribing

        private readonly Queue<EventPair> m_WaitingList = new();

        private readonly object m_LockRoot = new();

        public override void OnUpdate() {
            lock (m_LockRoot) {
                while (m_WaitingList.Count > 0) {
                    var element = m_WaitingList.Dequeue();
                    element.EventHandler.Invoke(element.Sender, element.Argument);
                }
            }
        }

        /// <summary>
        /// Subscribe to event name with a handler
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handler"></param>
        public string Subscribe(string eventName, EventHandler<object> handler) {
            var id = Guid.NewGuid().ToString();

            m_ActiveEventHandlers.Add(id, handler);
            m_CurrentEvents.Add(eventName, id);

            if (enableDebug) {
                Debug.Log($"[Event] Subscribe: {eventName} with handler: {handler.Method.Name} [{id}]");
            }

            return id;
        }

        public void Unsubscribe(string eventName, string handlerId) {
            if (IsNullOrEmpty(handlerId)) {
                //Debug.LogWarning($"HandlerId:{handlerId} is null or empty");
                return;
            }

            if (!m_ActiveEventHandlers.ContainsKey(handlerId)) {
                //Debug.LogError($"Handler: {handlerId} does not exist");
                return;
            }

            if (!m_CurrentEvents.ContainsKey(eventName) && m_CurrentEvents[eventName].Contains(handlerId)) {
                return;
            }


            if (enableDebug) {
                Debug.Log($"[Event] Unsubscribe: {eventName} with handler: {handlerId}");
            }

            m_CurrentEvents.Remove(eventName, handlerId);
            m_ActiveEventHandlers.Remove(handlerId);
        }

        public void InvokeNow(string eventName, object sender = null, object eventArg = null) {
            if (m_CurrentEvents.ContainsKey(eventName)) {
                var handlers = m_CurrentEvents[eventName].ToArray(); //TODO: change to linkedlist to avoid "handler list being modified during iteration" error
                foreach (var handlerId in handlers) {
                    if (IsNullOrEmpty(handlerId)) {
                        Debug.LogError($"Handler: {handlerId} is null or empty");
                        return;
                    }

                    if (m_ActiveEventHandlers.TryGetValue(handlerId, out var handler)) {
                        handler.Invoke(sender, eventArg);
                    }
                    else {
                        Debug.LogError($"Handler: {handlerId} callback is null");
                    }
                }
            }
            else
                Debug.Log("Event :" + eventName + " does not have any c# subscriber");
        }

        public void Invoke(string eventName, object sender = null, object eventArg = null) {
            if (m_CurrentEvents.TryGetValue(eventName, out var value))
                foreach (var handlerId in value) {
                    if (IsNullOrEmpty(handlerId)) {
                        Debug.LogError($"Handler: {handlerId} is null or empty");
                        return;
                    }

                    if (m_ActiveEventHandlers.TryGetValue(handlerId, out var handler)) {
                        var eventPair = new EventPair(sender, eventArg, handler);
                        lock (m_LockRoot) {
                            m_WaitingList.Enqueue(eventPair);
                            if (enableDebug) {
                                Debug.Log($"[Event] Invoke: {eventName} with handler: {handler.Method.Name} [{handlerId}]");
                            }
                        }
                    }
                }
            else {
                if (enableDebug) {
                    Debug.Log("Event :" + eventName + " does not have any c# subscriber");
                }
            }
        }

        public int Count() {
            return m_CurrentEvents.Count;
        }

        public MultiDictionary<string, string> GetEvents() {
            return m_CurrentEvents;
        }

        public Dictionary<string, EventHandler<object>> GetHandlers() {
            return m_ActiveEventHandlers;
        }

        private class EventPair {
            public readonly object Argument;
            public readonly EventHandler<object> EventHandler;
            public readonly object Sender;

            public EventPair(object sender, object argument, EventHandler<object> eventHandler) {
                Sender = sender;
                Argument = argument;
                EventHandler = eventHandler;
            }
        }

        public override void OnReset() {
            m_CurrentEvents.Clear();
            m_ActiveEventHandlers.Clear();
            lock (m_LockRoot) {
                m_WaitingList.Clear();
            }
        }
    }
}