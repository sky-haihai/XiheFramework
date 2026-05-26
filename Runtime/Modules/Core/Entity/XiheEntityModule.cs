using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XiheFramework.Runtime.Base;
using XiheFramework.Runtime.Event;
using XiheFramework.Runtime.Resource;

namespace XiheFramework.Runtime.Entity {
    [XiheCoreModule(typeof(IXiheEntityModule))]
    public class XiheEntityModule : XiheEntityModuleBase {
        private readonly Dictionary<uint, GameEntityBase> m_Entities = new();
        private readonly Dictionary<uint, uint> m_RecycledEntityIds = new(); //entity ids that's being destroyed at current frame

        private readonly object m_LockRoot = new();
        public GameEntityBase[] CurrentEntities => m_Entities.Values.ToArray();

        #region Public Methods

        public override T InstantiateEntity<T>(string entityAddress, uint presetId = 0, Action<T> onInstantiatedCallback = null) {
            if (string.IsNullOrEmpty(entityAddress)) {
                Debug.LogError("[ENTITY] Entity address cannot be null or empty");
                return null;
            }

            var go = Game.GetModule<IXiheResourceModule>().InstantiateAsset<GameObject>(entityAddress);
            if (go == null) {
                // Debug.LogError("[ENTITY] Entity Instantiation Failed : " + entityAddress + " does not exist or is not a GameObject");
                return null;
            }

            var entity = go.GetComponent<T>();
            if (entity == null) {
                Debug.LogError("[ENTITY] Entity Instantiation Failed : " + entityAddress + " has no component of type " + typeof(T));
                return null;
            }

            entity.gameObject.name = entityAddress + "(Prefab)";
            lock (m_LockRoot) SetUpGameEntity(entity, entityAddress, presetId, onInstantiatedCallback);

            if (enableDebug) {
                Debug.Log($"[ENTITY] Entity Instantiated : {entity.EntityId} ({entity.EntityAddress})");
            }

            return entity;
        }

        public override GameEntityBase InstantiateEntity(string entityAddress, uint presetId = 0, Action<GameEntityBase> onInstantiatedCallback = null) {
            return InstantiateEntity<GameEntityBase>(entityAddress, presetId, onInstantiatedCallback);
        }

        public override void InstantiateEntityAsync<T>(string entityAddress, uint presetId = 0, Action<T> onInstantiatedCallback = null) {
            if (string.IsNullOrEmpty(entityAddress)) {
                Debug.LogError("[ENTITY] Entity address cannot be null or empty");
                return;
            }

            Game.GetModule<IXiheResourceModule>().InstantiateAssetAsync<GameObject>(entityAddress, go => {
                if (go == null) {
                    Debug.LogError("[ENTITY] Entity Instantiation Failed : " + entityAddress + " is not a GameObject");
                    return;
                }

                var entity = go.GetComponent<T>();
                entity.gameObject.name = entityAddress + "(Prefab)";
                lock (m_LockRoot) SetUpGameEntity(entity, entityAddress, presetId, onInstantiatedCallback);
                if (enableDebug) {
                    Debug.Log($"[ENTITY] Entity Instantiated : {entity.EntityId} ({entity.EntityAddress})");
                }
            });
        }

        public override GameEntityBase InstantiateEntityAsync(string entityAddress, uint presetId = 0, Action<GameEntityBase> onInstantiatedCallback = null) {
            return InstantiateEntity<GameEntityBase>(entityAddress, presetId, onInstantiatedCallback);
        }

        public override void DestroyEntity(uint entityId) {
            lock (m_LockRoot) {
                if (!m_Entities.ContainsKey(entityId) || m_RecycledEntityIds.ContainsKey(entityId)) return;
                var entity = m_Entities[entityId];
                if (entity == null) {
                    Debug.LogError($"[ENTITY] Trying to destroy Entity {entityId} but it's NULL");
                    return;
                }

                m_RecycledEntityIds.Add(entityId, entityId);
                Destroy(entity.gameObject);
                if (enableDebug) {
                    Debug.Log($"[ENTITY] Entity being Destroyed : {entityId} ({entity.EntityAddress})");
                }
            }
        }

        public override void DestroyEntity(GameEntityBase entityBase) {
            DestroyEntity(entityBase.EntityId);
        }

        public override void RegisterEntity(GameEntityBase entityBase, uint ownerEntityId = 0, bool setParent = true, uint presetId = 0) {
            if (entityBase.EntityId != 0) {
                Debug.LogWarning("[ENTITY] Entity : " + entityBase.EntityId + " has already been registered, no need to register again");
                return;
            }

            if (!entityBase.gameObject.name.EndsWith("(Scene)", StringComparison.Ordinal)) {
                entityBase.gameObject.name += "(Scene)";
            }

            lock (m_LockRoot) SetUpGameEntity(entityBase, entityBase.EntityAddress, presetId);
        }

        public override void UnregisterEntity(uint entityId) {
            lock (m_LockRoot) {
                if (!m_Entities.TryGetValue(entityId, out var entity)) return;
                if (entity == null) {
                    Debug.LogError($"[ENTITY] Entity {entityId} can not be unregistered: NULL.");
                    m_Entities.Remove(entityId);
                    return;
                }

                entity.OnDestroyCallbackInternal();
                if (enableDebug) {
                    Debug.Log($"[ENTITY] Entity Unregistered : {entityId} ({entity.EntityAddress})");
                }

                var args = new EntityModuleEvents.OnEntityDestroyedEventArgs(entityId, entity.GetType(), entity.EntityAddress, entity.gameObject.name, entity.transform.position,
                    entity.transform.rotation);
                Game.GetModule<IXiheEventModule>().InvokeNow(EntityModuleEvents.OnEntityDestroyedEventName, args);
                m_Entities.Remove(entityId);
                m_RecycledEntityIds.Remove(entityId); // release recycled id if exist
                entity.EntityId = 0;
                entity.OwnerId = 0;
            }
        }

        public override void ChangeEntityOwner(uint entityId, uint ownerId, bool setParent = true, Transform rootTransform = null) {
            if (!m_Entities.ContainsKey(entityId)) {
                return;
            }

            if (!m_Entities.ContainsKey(ownerId)) {
                return;
            }

            m_Entities[entityId].OwnerId = ownerId;
            if (setParent) {
                if (rootTransform) {
                    m_Entities[entityId].transform.SetParent(rootTransform, false);
                }
                else {
                    m_Entities[entityId].transform.SetParent(m_Entities[ownerId].transform, false);
                }
            }
        }

        public override T GetEntity<T>(uint entityId) {
            if (!m_Entities.ContainsKey(entityId)) {
                Debug.LogWarning($"[ENTITY] Entity : {entityId} is not existed or is not Type of {typeof(T)}");
                return null;
            }

            var result = m_Entities[entityId];
            return result as T;
        }

        public override GameEntityBase GetEntity(uint entityId) {
            return GetEntity<GameEntityBase>(entityId);
        }

        public override bool IsEntityAvailable(uint entityId) {
            lock (m_LockRoot) {
                m_Entities.TryGetValue(entityId, out var entity);
                if (entity == null) {
                    return false;
                }

                if (m_RecycledEntityIds.ContainsKey(entityId)) {
                    return false;
                }

                return true;
            }
        }

        #endregion

        protected override void OnInstantiated() {
            base.OnInstantiated();
        }

        protected override void OnUpdate() {
            var cache = new List<uint>(m_Entities.Keys);
            foreach (var entityId in cache) {
                if (m_Entities.TryGetValue(entityId, out var entity)) {
                    entity.OnUpdateCallbackInternal();
                }
            }
        }

        protected override void OnFixedUpdate() {
            var cache = new List<uint>(m_Entities.Keys);
            foreach (var entity in cache) {
                if (m_Entities.ContainsKey(entity)) {
                    m_Entities[entity].OnFixedUpdateCallbackInternal();
                }
            }
        }

        protected override void OnLateUpdate() {
            var cache = new List<uint>(m_Entities.Keys);
            foreach (var entityId in cache) {
                if (m_Entities.TryGetValue(entityId, out var entity)) {
                    entity.OnLateUpdateCallbackInternal();
                }
            }
        }

        public void OnDestroy() {
            var destroyQueue = new Queue<GameEntityBase>();
            foreach (var entity in m_Entities.Values) {
                destroyQueue.Enqueue(entity);
            }

            while (destroyQueue.Count > 0) {
                var entity = destroyQueue.Dequeue();
                if (entity != null) {
                    DestroyEntity(entity);
                }
            }

            m_Entities.Clear();
        }

        #region Private Methods

        private uint CalculateFinalId(uint presetId) {
            uint finalId = 1;
            if (presetId != 0) {
                finalId = presetId;
            }
            else {
                while (m_Entities.ContainsKey(finalId) || m_RecycledEntityIds.ContainsKey(finalId)) {
                    finalId++;
                }
            }

            return finalId;
        }

        private void SetUpGameEntity<T>(T entity, string address, uint presetId, Action<T> onInstantiated = null) where T : GameEntityBase {
            if (presetId != 0) DestroyEntity(presetId);
            var finalId = CalculateFinalId(presetId);

            entity.EntityId = finalId;
            entity.EntityAddress = address;
            m_Entities.Add(finalId, entity);

            onInstantiated?.Invoke(entity);

            var args = new EntityModuleEvents.OnEntityInstantiatedEventArgs(finalId, entity.EntityAddress, entity.gameObject.name);

            entity.OnInitCallbackInternal();
            Game.GetModule<IXiheEventModule>().Invoke(EntityModuleEvents.OnEntityInstantiatedEventName, args);
        }

        #endregion
    }
}
