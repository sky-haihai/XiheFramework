using UnityEditor;
using UnityEngine;

namespace XiheFramework.Utility.IK {
    public class IKLeafBone : MonoBehaviour {
//         public Transform target;
//         public Transform pole;
//
//         [Range(2, 10)]
//         public int chainLength = 2;
//
//         [Range(1, 20)]
//         public int iteration = 10;
//
//         public float tolerance = 0.01f;
//
//         private Transform m_Root;
//
//         private Transform[] m_Bones;
//         private Vector3[] m_BonePositions; //cache for iterations 
//         private float[] m_BoneLengths;
//         private float m_TotalLength;
//
//         private Vector3[] m_StartDirectionCache;
//         private Quaternion[] m_StartBoneRotations;
//         private Quaternion m_StartTargetRotation;
//         private Quaternion m_StartRootRotation;
//
//         private void Start() {
//             Init();
//         }
//
//         void Init() {
//             if (target == null) {
//                 return;
//             }
//
//             //position
//             m_Bones = new Transform[chainLength + 1];
//             m_BonePositions = new Vector3[chainLength + 1];
//             m_BoneLengths = new float[chainLength];
//             m_TotalLength = 0f;
//
//             //rotation
//             m_StartDirectionCache = new Vector3[chainLength + 1];
//             m_StartBoneRotations = new Quaternion[chainLength + 1];
//             m_StartTargetRotation = target.rotation;
//
//             //data
//             var current = transform;
//             for (int i = m_Bones.Length - 1; i >= 0; i--) {
//                 m_Bones[i] = current;
//
//                 if (i == m_Bones.Length - 1) {
//                     m_StartDirectionCache[i] = target.position - current.position;
//                 }
//                 else {
//                     m_StartDirectionCache[i] = m_Bones[i + 1].position - current.position;
//
//                     var delta = m_StartDirectionCache[i].magnitude;
//                     m_BoneLengths[i] = delta;
//                     m_TotalLength += delta;
//                 }
//
//                 current = current.parent;
//             }
//         }
//
//         private void LateUpdate() {
//             ResolveIK();
//         }
//
//         private void ResolveIK() {
//             if (target == null) {
//                 return;
//             }
//
//             //in case change chain length at runtime 
//             if (m_BoneLengths.Length != chainLength) {
//                 Init();
//             }
//
//             for (int i = 0; i < m_Bones.Length; i++) {
//                 m_BonePositions[i] = m_Bones[i].position;
//             }
//
//             Quaternion rootRotation;
//             if (m_Bones[0].parent != null) {
//                 rootRotation = m_Bones[0].parent.rotation;
//             }
//             else {
//                 rootRotation = Quaternion.identity;
//             }
//
//             if (Vector3.Distance(target.position, m_BonePositions[0]) > m_TotalLength) {
//                 //cant reach target so distribute in straight line
//                 var dir = target.position - m_BonePositions[0];
//                 dir.Normalize();
//
//                 for (int i = 1; i < m_BonePositions.Length; i++) {
//                     m_BonePositions[i] = m_BonePositions[i - 1] + dir * m_BoneLengths[i - 1];
//                 }
//             }
//             else {
//                 //actual IK iteration happens here
//                 for (int _iteration = 0; _iteration < iteration; _iteration++) {
//                     //calculate from leaf bone
//                     for (int i = m_BonePositions.Length - 1; i > 0; i--) {
//                         if (i == m_BonePositions.Length - 1) {
//                             //first bone
//                             m_BonePositions[i] = target.position;
//                         }
//                         else {
//                             var last = m_BonePositions[i + 1];
//                             m_BonePositions[i] = last + (m_BonePositions[i] - last).normalized * m_BoneLengths[i];
//                         }
//                     }
//
//                     //calculate from root bone
//                     for (int i = 1; i < m_BonePositions.Length; i++) {
//                         var last = m_BonePositions[i - 1];
//                         m_BonePositions[i] = last + (m_BonePositions[i] - last).normalized * m_BoneLengths[i - 1];
//                     }
//
//                     //closer than tolerance
//                     if (Vector3.Distance(m_BonePositions[^1], target.position) < tolerance) {
//                         break;
//                     }
//                 }
//             }
//
//             if (pole != null) {
//                 for (int i = 1; i < m_BonePositions.Length - 1; i++) {
//                     var last = m_BonePositions[i - 1];
//                     var next = m_BonePositions[i + 1];
//                     var plane = new Plane(next - last, last);
//                     var poleProjection = plane.ClosestPointOnPlane(pole.position);
//                     var boneProjection = plane.ClosestPointOnPlane(m_BonePositions[i]);
//
//                     var angle = Vector3.SignedAngle(boneProjection - last, poleProjection - last, plane.normal);
//                     m_BonePositions[i] = Quaternion.AngleAxis(angle, plane.normal) * (m_BonePositions[i] - m_BonePositions[i - 1]) +
//                                          m_BonePositions[i - 1];
//                 }
//             }
//
//             for (int i = 0; i < m_BonePositions.Length; i++) {
//                 // if (i==m_BonePositions.Length-1) {
//                 //     m_Bones[i].rotation = target.rotation;
//                 // }
//
//                 if (i == Positions.Length - 1)
//                     SetRotationRootSpace(m_Bones[i],
//                         Quaternion.Inverse(targetRotation) * StartRotationTarget * Quaternion.Inverse(StartRotationBone[i]));
//                 else
//                     SetRotationRootSpace(Bones[i],
//                         Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * Quaternion.Inverse(StartRotationBone[i]));
//                 SetPositionRootSpace(Bones[i], Positions[i]);
//
//                 m_Bones[i].position = m_BonePositions[i];
//             }
//         }
//
//         private Vector3 GetPositionRootSpace(Transform current) {
//             if (m_Root == null)
//                 return current.position;
//             else
//                 return Quaternion.Inverse(m_Root.rotation) * (current.position - m_Root.position);
//         }
//
//         private void SetPositionRootSpace(Transform current, Vector3 position) {
//             if (m_Root == null)
//                 current.position = position;
//             else
//                 current.position = m_Root.rotation * position + m_Root.position;
//         }
//
//         private Quaternion GetRotationRootSpace(Transform current) {
//             //inverse(after) * before => rot: before -> after
//             if (m_Root == null)
//                 return current.rotation;
//             else
//                 return Quaternion.Inverse(current.rotation) * m_Root.rotation;
//         }
//
//         private void SetRotationRootSpace(Transform current, Quaternion rotation) {
//             if (m_Root == null)
//                 current.rotation = rotation;
//             else
//                 current.rotation = m_Root.rotation * rotation;
//         }
//
//         private void OnDrawGizmos() {
// #if UNITY_EDITOR
//             var current = this.transform;
//             for (int i = 0; i < chainLength && current != null && current.parent != null; i++) {
//                 var scale = Vector3.Distance(current.position, current.parent.position);
//                 Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position),
//                     new Vector3(scale * 0.1f, scale, scale * 0.1f));
//                 Handles.color = Color.green;
//                 Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
//                 current = current.parent;
//             }
// #endif
//         }

        public bool isActive;

        [Range(1, 10)] public int chainLength = 2;

        public Transform target;
        public Transform pole;


        [Header("Solver Parameters")] public int iterations = 10;


        public float tolerance = 0.001f;

        [Range(0, 1)] public float snapBackStrength = 1f;

        public float handleSize;
        private Transform[] m_Bones;

        private float[] m_BonesLength; //Target to Origin
        private float m_CompleteLength;
        private Vector3[] m_Positions;
        private Transform m_Root;
        private Vector3[] m_StartDirectionSucc;
        private Quaternion[] m_StartRotationBone;
        private Quaternion m_StartRotationTarget;


        // Start is called before the first frame update
        private void Awake() {
            Init();
        }

        // Update is called once per frame
        private void LateUpdate() {
            if (isActive) ResolveIK();
        }

        private void OnDrawGizmos() {
#if UNITY_EDITOR
            var current = transform;
            for (var i = 0; i < chainLength && current != null && current.parent != null; i++) {
                var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
                Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position),
                    new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
                Handles.color = Color.green;
                Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
                current = current.parent;
            }

            Handles.matrix = Matrix4x4.identity;
            Handles.color = Color.red;
            if (target != null) Handles.DrawWireCube(target.position, new Vector3(3, 1, 5) * handleSize);

            if (pole != null) Handles.DrawWireCube(pole.position, Vector3.one * handleSize);
#endif
        }

        private void Init() {
            //initial array
            m_Bones = new Transform[chainLength + 1];
            m_Positions = new Vector3[chainLength + 1];
            m_BonesLength = new float[chainLength];
            m_StartDirectionSucc = new Vector3[chainLength + 1];
            m_StartRotationBone = new Quaternion[chainLength + 1];

            //find root
            m_Root = transform;
            for (var i = 0; i <= chainLength; i++) {
                if (m_Root == null)
                    throw new UnityException("The chain value is longer than the ancestor chain!");
                m_Root = m_Root.parent;
            }

            //init target
            if (target == null) {
                target = new GameObject(gameObject.name + " Target").transform;
                SetPositionRootSpace(target, GetPositionRootSpace(transform));
            }

            m_StartRotationTarget = GetRotationRootSpace(target);


            //init data
            var current = transform;
            m_CompleteLength = 0;
            for (var i = m_Bones.Length - 1; i >= 0; i--) {
                m_Bones[i] = current;
                m_StartRotationBone[i] = GetRotationRootSpace(current);

                if (i == m_Bones.Length - 1) {
                    //leaf
                    m_StartDirectionSucc[i] = GetPositionRootSpace(target) - GetPositionRootSpace(current);
                }
                else {
                    //mid bone
                    m_StartDirectionSucc[i] = GetPositionRootSpace(m_Bones[i + 1]) - GetPositionRootSpace(current);
                    m_BonesLength[i] = m_StartDirectionSucc[i].magnitude;
                    m_CompleteLength += m_BonesLength[i];
                }

                current = current.parent;
            }
        }

        private void ResolveIK() {
            if (target == null)
                return;

            if (m_BonesLength.Length != chainLength)
                Init();

            //Fabric

            //  root
            //  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
            //   x--------------------x--------------------x---...

            //get position
            for (var i = 0; i < m_Bones.Length; i++)
                m_Positions[i] = GetPositionRootSpace(m_Bones[i]);

            var targetPosition = GetPositionRootSpace(target);
            var targetRotation = GetRotationRootSpace(target);

            //1st is possible to reach?
            if ((targetPosition - GetPositionRootSpace(m_Bones[0])).sqrMagnitude >= m_CompleteLength * m_CompleteLength) {
                //just strech it
                var direction = (targetPosition - m_Positions[0]).normalized;
                //set everything after root
                for (var i = 1; i < m_Positions.Length; i++)
                    m_Positions[i] = m_Positions[i - 1] + direction * m_BonesLength[i - 1];
            }
            else {
                for (var i = 0; i < m_Positions.Length - 1; i++)
                    m_Positions[i + 1] = Vector3.Lerp(m_Positions[i + 1], m_Positions[i] + m_StartDirectionSucc[i], snapBackStrength);

                for (var iteration = 0; iteration < iterations; iteration++) {
                    //back
                    for (var i = m_Positions.Length - 1; i > 0; i--)
                        if (i == m_Positions.Length - 1)
                            m_Positions[i] = targetPosition; //set it to target
                        else
                            m_Positions[i] =
                                m_Positions[i + 1] + (m_Positions[i] - m_Positions[i + 1]).normalized * m_BonesLength[i]; //set in line on distance

                    //forward
                    for (var i = 1; i < m_Positions.Length; i++)
                        m_Positions[i] = m_Positions[i - 1] + (m_Positions[i] - m_Positions[i - 1]).normalized * m_BonesLength[i - 1];

                    //close enough?
                    if ((m_Positions[m_Positions.Length - 1] - targetPosition).sqrMagnitude < tolerance * tolerance)
                        break;
                }
            }

            //move towards pole
            if (pole != null) {
                var polePosition = GetPositionRootSpace(pole);
                for (var i = 1; i < m_Positions.Length - 1; i++) {
                    var plane = new Plane(m_Positions[i + 1] - m_Positions[i - 1], m_Positions[i - 1]);
                    var projectedPole = plane.ClosestPointOnPlane(polePosition);
                    var projectedBone = plane.ClosestPointOnPlane(m_Positions[i]);
                    var angle = Vector3.SignedAngle(projectedBone - m_Positions[i - 1], projectedPole - m_Positions[i - 1], plane.normal);
                    m_Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (m_Positions[i] - m_Positions[i - 1]) + m_Positions[i - 1];
                }
            }

            //set position & rotation
            for (var i = 0; i < m_Positions.Length; i++) {
                if (i == m_Positions.Length - 1)
                    SetRotationRootSpace(m_Bones[i],
                        Quaternion.Inverse(targetRotation) * m_StartRotationTarget * Quaternion.Inverse(m_StartRotationBone[i]));
                else
                    SetRotationRootSpace(m_Bones[i],
                        Quaternion.FromToRotation(m_StartDirectionSucc[i], m_Positions[i + 1] - m_Positions[i]) *
                        Quaternion.Inverse(m_StartRotationBone[i]));
                SetPositionRootSpace(m_Bones[i], m_Positions[i]);
            }
        }

        private Vector3 GetPositionRootSpace(Transform current) {
            if (m_Root == null)
                return current.position;
            return Quaternion.Inverse(m_Root.rotation) * (current.position - m_Root.position);
        }

        private void SetPositionRootSpace(Transform current, Vector3 position) {
            if (m_Root == null)
                current.position = position;
            else
                current.position = m_Root.rotation * position + m_Root.position;
        }

        private Quaternion GetRotationRootSpace(Transform current) {
            //inverse(after) * before => rot: before -> after
            if (m_Root == null)
                return current.rotation;
            return Quaternion.Inverse(current.rotation) * m_Root.rotation;
        }

        private void SetRotationRootSpace(Transform current, Quaternion rotation) {
            if (m_Root == null)
                current.rotation = rotation;
            else
                current.rotation = m_Root.rotation * rotation;
        }
    }
}