using System;
using UnityEngine;

namespace XiheFramework.Runtime.Base {
    public abstract class GameModuleBase : MonoBehaviour {
        /// <summary>
        /// Priority of the module to determine the order of execution.
        /// </summary>
        public abstract int Priority { get; }

        public int updateInterval;
        public int fixedUpdateInterval;
        public int lateUpdateInterval;

        public bool enableDebug;

        internal void SetDebugEnabled(bool value) {
            enableDebug = value;
        }

        #region Game Module Callbacks

        internal void OnInstantiatedInternal(Action onInstantiated) {
            OnInstantiated();
            onInstantiated?.Invoke();
        }

        internal void OnUpdateInternal() => OnUpdate();

        internal void OnFixedUpdateInternal() => OnFixedUpdate();

        internal void OnLateUpdateInternal() => OnLateUpdate();

        internal void OnDestroyedInternal() => OnDestroyed();

        /// <summary>
        /// Called after the module has been registered.
        /// </summary>
        protected virtual void OnInstantiated() { }

        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnDestroyed() { }

        #endregion
    }
}
