using System;
using UnityEngine;
using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.Entity {
    public abstract class XiheEntityModuleBase : GameModuleBase, IXiheEntityModule {
        public override int Priority => (int)CoreModulePriority.Entity;

        public abstract T InstantiateEntity<T>(string entityAddress, uint presetId = 0, Action<T> onInstantiatedCallback = null) where T : GameEntityBase;

        public abstract GameEntityBase InstantiateEntity(string entityAddress, uint presetId = 0, Action<GameEntityBase> onInstantiatedCallback = null);

        public abstract void InstantiateEntityAsync<T>(string entityAddress, uint presetId = 0, Action<T> onInstantiatedCallback = null) where T : GameEntityBase;

        public abstract GameEntityBase InstantiateEntityAsync(string entityAddress, uint presetId = 0, Action<GameEntityBase> onInstantiatedCallback = null);

        /// <summary>
        /// Destroy Entity at the end of the frame
        /// </summary>
        /// <param name="entityId"></param>
        public abstract void DestroyEntity(uint entityId);

        /// <summary>
        /// Destroy Entity at the end of the frame
        /// </summary>
        /// <param name="entityBase"></param>
        public abstract void DestroyEntity(GameEntityBase entityBase);

        public abstract void RegisterEntity(GameEntityBase entityBase, uint ownerEntityId = 0, bool setParent = true, uint presetId = 0);

        public abstract void UnregisterEntity(uint entityId);

        public abstract void ChangeEntityOwner(uint entityId, uint ownerId, bool setParent = true, Transform rootTransform = null);

        public abstract T GetEntity<T>(uint entityId) where T : GameEntityBase;

        public abstract GameEntityBase GetEntity(uint entityId);

        public abstract bool IsEntityAvailable(uint entityId);
    }
}