using System;
using UnityEngine;

namespace XiheFramework.Utility.Bezier {
    [Serializable]
    public struct TransformInfo {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformInfo(Vector3 position, Quaternion rotation, Vector3 scale) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public static TransformInfo CreateInstance() {
            return new TransformInfo();
        }
    }
}