using System;
using UnityEditor;
using UnityEngine;

namespace XiheFramework.Utility.KCC {
    public class XiheCharacterController2D : MonoBehaviour {
        public enum Axis {
            X,
            Y,
            // Z
        }

        public LayerMask layerMask;
        public Axis axis = Axis.X;
        public float radius = 1f;
        public float height = 2f;
        public float skinThickness = 0.1f;
        public int maxBounces = 5;
        public Vector3 center;

        private Vector3 m_DesiredMovementBuffer;

        public void Move(Vector3 movement) {
            m_DesiredMovementBuffer += movement;
        }

        private void LateUpdate() {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            MoveAndSlide(m_DesiredMovementBuffer);
            m_DesiredMovementBuffer = Vector3.zero;
        }

        private void MoveAndSlide(Vector3 desiredMovement) {
            GetCapsulePoints(out var point0, out var point1);

            var bounceCount = 0;
            for (int i = 0; i < maxBounces; i++) {
                var hasHit = Physics.CapsuleCast(point0, point1, radius, desiredMovement.normalized, out var hitInfo, desiredMovement.magnitude + skinThickness, layerMask,
                    QueryTriggerInteraction.Ignore);
                if (!hasHit) {
                    break;
                }

                if (desiredMovement.magnitude <= (hitInfo.distance - skinThickness)) {
                    break;
                }

                var tangent = Vector3.Cross(Vector3.forward, hitInfo.normal);
                desiredMovement = Vector3.Project(desiredMovement, tangent);
                bounceCount++;
            }

            if (bounceCount == maxBounces) {
                return;
            }

            transform.position += desiredMovement;

            AvoidPenetration(point0, point1);
        }

        private void GetCapsulePoints(out Vector3 point0, out Vector3 point1) {
            point0 = axis switch {
                Axis.X => transform.TransformPoint(center + Vector3.left * height / 2),
                Axis.Y => transform.TransformPoint(center + Vector3.up * height / 2),
                _ => throw new ArgumentOutOfRangeException()
            };

            point1 = axis switch {
                Axis.X => transform.TransformPoint(center + Vector3.right * height / 2),
                Axis.Y => transform.TransformPoint(center + Vector3.down * height / 2),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void AvoidPenetration(Vector3 point0, Vector3 point1) {
            var hasCollision = Physics.CheckCapsule(point0, point1, radius, layerMask, QueryTriggerInteraction.Ignore);
            if (!hasCollision) return;

            var validPos = FindValidClosestPosition(5, skinThickness, point0, point1); //todo: 
            transform.position = validPos;
        }

        private Vector3 FindValidClosestPosition(int maxStep, float stepDistance, Vector3 point0, Vector3 point1) {
            var validPosition = transform.position;

            var avgNormal = Vector3.zero;
            for (int i = 0; i < maxStep; i++) {
                var newColliders = Physics.OverlapCapsule(point0 + avgNormal * (stepDistance * i), point1 + avgNormal * (stepDistance * i), radius, layerMask,
                    QueryTriggerInteraction.Ignore);

                avgNormal = Vector3.zero;
                foreach (var col in newColliders) {
                    var closestPoint = col.ClosestPoint(transform.position);
                    var normal = (closestPoint - transform.position).normalized;
                    avgNormal += normal;
                }

                avgNormal = -avgNormal.normalized;
                validPosition += avgNormal * stepDistance;

                Debug.DrawLine(point0 + avgNormal * (stepDistance * i), point1 + avgNormal * (stepDistance * i), Color.red * i / maxStep);
                if (newColliders.Length == 0) {
                    break;
                }
            }

            return validPosition;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            var rotation = axis switch {
                Axis.X => Quaternion.Euler(0, 0, 90),
                Axis.Y => Quaternion.Euler(0, 0, 0),
                _ => throw new ArgumentOutOfRangeException()
            };

            rotation *= transform.rotation;

            DrawWireCapsule(transform.TransformPoint(center), rotation, radius, height + 2 * radius, Color.red);
            DrawWireCapsule(transform.TransformPoint(center), rotation, radius - skinThickness, height + 2 * (radius - skinThickness), Color.green);
        }

        private static void DrawWireCapsule(Vector3 pos, Quaternion rot, float radius, float height, Color color = default(Color)) {
            if (color != default(Color))
                Handles.color = color;

            Matrix4x4 angleMatrix = Matrix4x4.TRS(pos, rot, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix)) {
                var pointOffset = (height - (radius * 2)) / 2;

                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
                Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
                Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
            }
        }
#endif
    }
}