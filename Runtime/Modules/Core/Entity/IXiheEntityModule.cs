using System;
using UnityEngine;

namespace XiheFramework.Runtime.Entity {
    public interface IXiheEntityModule {
        public T InstantiateEntity<T>(string entityAddress, uint presetId = 0, Action<T> onInstantiatedCallback = null) where T : GameEntityBase;
        public GameEntityBase InstantiateEntity(string entityAddress, uint presetId = 0, Action<GameEntityBase> onInstantiatedCallback = null);
        public void InstantiateEntityAsync<T>(string entityAddress, uint presetId = 0, Action<T> onInstantiatedCallback = null) where T : GameEntityBase;
        public GameEntityBase InstantiateEntityAsync(string entityAddress, uint presetId = 0, Action<GameEntityBase> onInstantiatedCallback = null);
        public void DestroyEntity(uint entityId);
        public void DestroyEntity(GameEntityBase entityBase);

        /// <summary>
        /// Register an instantiated entity
        /// </summary>
        /// <param name="entityBase"></param>
        /// <param name="ownerEntityId"></param>
        /// <param name="setParent"></param>
        /// <param name="presetId"></param>
        public void RegisterEntity(GameEntityBase entityBase, uint ownerEntityId = 0, bool setParent = true, uint presetId = 0);

        /// <summary>
        /// Unregister an instantiated entity
        /// </summary>
        /// <param name="entityId"></param>
        public void UnregisterEntity(uint entityId);

        public void ChangeEntityOwner(uint entityId, uint ownerId, bool setParent = true, Transform rootTransform = null);
        public T GetEntity<T>(uint entityId) where T : GameEntityBase;
        public GameEntityBase GetEntity(uint entityId);
        public bool IsEntityAvailable(uint entityId);
    }
}