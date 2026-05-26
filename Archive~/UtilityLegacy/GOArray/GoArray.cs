using UnityEditor;
using UnityEngine;

namespace XiheFramework.Utility.GOArray {
    [ExecuteInEditMode]
    public class GoArray : MonoBehaviour {
#if UNITY_EDITOR
        public GameObject source;

        [Range(1, 20)]
        public int countX = 1;

        [Range(1, 20)]
        public int countY = 1;

        [Range(1, 20)]
        public int countZ = 1;

        //public bool useRelativeOffset;
        // public Vector3 relativeOffset;

        //public bool useConstantOffset;
        // public Vector3 constantOffset;

        public bool keepLocalPosition;
        public bool keepLocalRotation;
        public bool alignRendererBound;
        public Vector3 offset;

        // private Transform[,,] cachedInstances;
        //public bool merge;

        private Transform m_CachedTransform;

        private Renderer m_Renderer;

        public void UpdateModifier() {
            if (source == null) return;

            m_CachedTransform = transform;

            if (alignRendererBound) m_Renderer = source.GetComponent<Renderer>();

            foreach (Transform t in m_CachedTransform)
                //DestroyImmediate(t.gameObject);
                EditorApplication.delayCall += () => {
                    Undo.DestroyObjectImmediate(t.gameObject);
                    // DestroyImmediate(t.gameObject);
                };


            for (var i = 0; i < countX; i++)
            for (var j = 0; j < countY; j++)
            for (var k = 0; k < countZ; k++) {
                var posOffset = keepLocalPosition ? source.transform.position : Vector3.zero;
                var pos = new Vector3(i * offset.x, j * offset.y, k * offset.z) + posOffset;

                if (alignRendererBound) {
                    var _bounds = m_Renderer.bounds;
                    pos = new Vector3(i * _bounds.size.x, j * _bounds.size.y, k * _bounds.size.z) + m_Renderer.transform.position -
                          _bounds.min;
                }

                var globalPos = m_CachedTransform.TransformPoint(pos);
                var go = PrefabUtility.InstantiatePrefab(source) as GameObject;
                //todo:temp solution for scene objs
                if (go == null) {
                    go = Instantiate(source) as GameObject;
                }

                // globalPos, m_CachedTransform.rotation, m_CachedTransform
                go.transform.SetParent(m_CachedTransform);
                go.transform.position = globalPos;
                if (!keepLocalRotation) {
                    go.transform.rotation = m_CachedTransform.rotation;
                }

                go.name = source.name + "_" + i + "_" + j + "_" + k;
            }
        }

        public void ClearModifier() {
            foreach (Transform t in m_CachedTransform)
                //DestroyImmediate(t.gameObject);
                EditorApplication.delayCall += () => {
                    Undo.DestroyObjectImmediate(t.gameObject);
                    // DestroyImmediate(t.gameObject);
                };
        }
#endif
    }
}