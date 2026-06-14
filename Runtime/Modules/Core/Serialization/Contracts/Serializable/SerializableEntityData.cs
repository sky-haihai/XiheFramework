using System;
using UnityEngine;

namespace XiheFramework.Runtime.Serialization.Serializable {
    [Serializable]
    public class SerializableEntityData {
        public string entityAddress;
        public Vector3 position;
        public Vector3 rotation;
        public string customData;
        public SerializableEntityData() { }

        public SerializableEntityData(string entityAddress, Vector3 position, Vector3 rotation, string customData) {
            this.entityAddress = entityAddress;
            this.position = position;
            this.rotation = rotation;
            this.customData = customData;
        }
    }
}