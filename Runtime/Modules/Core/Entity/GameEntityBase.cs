using System;
using UnityEngine;
using XiheFramework.Runtime.Event;
using XiheFramework.Runtime.LogicTime;
using XiheFramework.Runtime.Utility.DataStructure;

namespace XiheFramework.Runtime.Entity {
    public abstract class GameEntityBase : MonoBehaviour {
        [SerializeField, HideInInspector]
        public uint presetEntityId;

        [SerializeField, HideInInspector]
        public uint presetOwnerId;

        [SerializeField, HideInInspector]
        public bool useLogicTime = true;

        public abstract string GroupName { get; }
        public string EntityAddress { get; internal set; }
        public uint EntityId { get; internal set; }
        public uint OwnerId { get; internal set; }

        public float TimeScale { get; protected set; }
        protected float ScaledDeltaTime => Time.unscaledDeltaTime * TimeScale;

        private readonly MultiDictionary<string, string> m_EventHandlerIds = new();

        protected virtual void OnInitCallback() { }

        /// <summary>
        /// one frame after init
        /// </summary>
        protected virtual void OnAwakeCallback() { }

        protected virtual void OnUpdateCallback() { }
        protected virtual void OnFixedUpdateCallback() { }
        protected virtual void OnLateUpdateCallback() { }
        protected virtual void OnDestroyCallback() { }

        // public virtual SerializableEntityData OnSaveCallBack(OnSaveEventArgs args) {
        //     var data = new SerializableEntityData();
        //     data.entityAddress = EntityAddress;
        //     data.position = transform.position;
        //     data.rotation = transform.rotation.eulerAngles;
        //     return data;
        // }
        //
        // public virtual void OnLoadCallBack(SerializableEntityData data) {
        //     transform.position = data.position;
        //     transform.rotation = Quaternion.Euler(data.rotation);
        // }

        internal void OnInitCallbackInternal() {
            TimeScale = 1;
            var logicTimeModule = Game.GetModule<IXiheLogicTimeModule>();
            SubscribeEvent(logicTimeModule.OnSetGlobalTimeScaleEventName, OnSetGlobalTimeScale);
            SubscribeEvent(EntityModuleEvents.OnEntityInstantiatedEventName, OnEntityInstantiated);
            OnInitCallback();
        }

        private void OnEntityInstantiated(object sender, object e) {
            if (e is not EntityModuleEvents.OnEntityInstantiatedEventArgs args) {
                return;
            }

            if (args.entityId != EntityId) {
                return;
            }

            OnAwakeCallback();
        }

        internal void OnUpdateCallbackInternal() => OnUpdateCallback();

        internal void OnFixedUpdateCallbackInternal() => OnFixedUpdateCallback();

        internal void OnLateUpdateCallbackInternal() => OnLateUpdateCallback();

        // internal void OnSaveCallBackInternal(OnSaveEventArgs args) => OnSaveCallBack(args);
        //
        // internal void OnLoadCallBackInternal(SerializableEntityData entityData) => OnLoadCallBack(entityData);

        internal void OnDestroyCallbackInternal() {
            foreach (var eventName in m_EventHandlerIds.Keys) {
                UnsubscribeEvent(eventName);
            }

            m_EventHandlerIds.Clear();

            OnDestroyCallback();
        }

        public void DestroyEntity() {
            Game.GetModule<IXiheEntityModule>().DestroyEntity(EntityId);
        }

        /// <summary>
        /// Automatically Unsubscribe Event when Entity is being Destroyed
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        protected void SubscribeEvent(string eventName, EventHandler<object> eventHandler) {
            var handlerId = Game.GetModule<IXiheEventModule>().Subscribe(eventName, eventHandler);
            m_EventHandlerIds.Add(eventName, handlerId);
        }

        private void UnsubscribeEvent(string eventName) {
            if (m_EventHandlerIds.TryGetValue(eventName, out var handlerIds)) {
                foreach (var handlerId in handlerIds) {
                    Game.GetModule<IXiheEventModule>().Unsubscribe(eventName, handlerId);
                }
            }
        }

        protected virtual void OnSetGlobalTimeScale(object sender, object e) {
            if (useLogicTime) {
                var args = (OnSetGlobalTimeScaleEventArgs)e;
                TimeScale = args.newTimeScale;
            }
        }

        private void Start() {
            if (EntityId == 0) {
                Game.GetModule<IXiheEntityModule>().RegisterEntity(this, presetEntityId, false, presetOwnerId);
            }
        }

        private void OnDestroy() {
            Game.GetModule<IXiheEntityModule>().UnregisterEntity(EntityId);
        }
    }
}
