using System;
using UnityEngine;

namespace XiheFramework.Runtime.Entity {
    public static class EntityModuleEvents {
        public const string OnEntityInstantiatedEventName = "XiheEntityModule.OnEntityInstantiated";
        public const string OnEntityDestroyedEventName = "XiheEntityModule.OnEntityDestroyed";

        public struct OnEntityInstantiatedEventArgs {
            public uint entityId;
            public string entityName;
            public string gameObjectName;

            public OnEntityInstantiatedEventArgs(uint entityId, string entityName, string gameObjectName) {
                this.entityId = entityId;
                this.entityName = entityName;
                this.gameObjectName = gameObjectName;
            }
        }

        public struct OnEntityDestroyedEventArgs {
            public uint entityId;
            public Type entityType;
            public string entityAddress;
            public string gameObjectName;
            public Vector3 positionBeforeDestroy;
            public Quaternion rotationBeforeDestroy;

            public OnEntityDestroyedEventArgs(uint entityId, Type entityType, string entityAddress, string gameObjectName, Vector3 positionBeforeDestroy,
                Quaternion rotationBeforeDestroy) {
                this.entityId = entityId;
                this.entityType = entityType;
                this.entityAddress = entityAddress;
                this.gameObjectName = gameObjectName;
                this.positionBeforeDestroy = positionBeforeDestroy;
                this.rotationBeforeDestroy = rotationBeforeDestroy;
            }
        }
    }
}
