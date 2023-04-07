﻿using UnityEngine;

namespace XiheFramework.Modules.Base {
    public abstract class GameModule : MonoBehaviour {
        protected virtual void Awake() {
            GameManager.RegisterComponent(this);
        }

        public abstract void Update();

        public virtual void FixedUpdate() { }

        public virtual void LateUpdate() { }

        /// <summary>
        ///     Called after all game modules are registered (End of Awake)
        ///     Useful for setting up data before other modules trying to access it
        /// </summary>
        public virtual void Setup() { }

        public abstract void ShutDown(ShutDownType shutDownType);
    }
}