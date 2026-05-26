using System;
using UnityEngine;

namespace XiheFramework.Utility.ShapeKeyDriver {
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class ShapeKeyDriver : MonoBehaviour {
        public enum Axis {
            X = 0,
            Y,
            Z
        }

        public int blendShapeIndex;
        public Transform targetBone;
        public Axis axis;
        public float minRotation;
        public float maxRotation;
        private Quaternion m_OriginalRotation;

        private SkinnedMeshRenderer m_TargetRenderer;

        private void Start() {
            m_TargetRenderer = GetComponent<SkinnedMeshRenderer>();
            m_OriginalRotation = targetBone.localRotation;
        }

        private void Update() {
            var currentAngle = targetBone.localRotation.eulerAngles;
            var driverValue = axis switch {
                Axis.X => currentAngle.x - m_OriginalRotation.eulerAngles.x,
                Axis.Y => currentAngle.y - m_OriginalRotation.eulerAngles.y,
                Axis.Z => currentAngle.z - m_OriginalRotation.eulerAngles.z,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (driverValue < -180f)
                driverValue += 360f;
            else if (driverValue > 180f) driverValue -= 360f;

            var value = Mathf.InverseLerp(minRotation, maxRotation, driverValue);
            value = Mathf.Clamp01(value);

            m_TargetRenderer.SetBlendShapeWeight(blendShapeIndex, value * 100f);
        }
    }
}