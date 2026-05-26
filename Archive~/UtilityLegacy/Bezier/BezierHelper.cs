using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework.Utility.Bezier {
    public static class BezierHelper {
        public static Vector3 GetLinearPoint(Vector3 start, Vector3 end, float t) {
            t = Mathf.Clamp01(t);

            return Vector3.Lerp(start, end, t);
        }

        public static Vector3 GetQuadraticPoint(Vector3 start, Vector3 end, Vector3 handle, float t) {
            t = Mathf.Clamp01(t);

            var l0 = Vector3.Lerp(start, handle, t); //lerp between p0 and ph
            var l1 = Vector3.Lerp(handle, end, t); //lerp between ph and p1
            var lh = Vector3.Lerp(l0, l1, t); //lerp between l0 and l1
            return lh;
        }

        public static float GetQuadraticLength(Vector3 start, Vector3 end, Vector3 handle, int resolution) {
            //TODO: change to better method
            var sum = 0f;
            var temp = GetQuadraticPoint(start, end, handle, 0f);
            for (var i = 0; i < resolution; i++) {
                var t = (float)(i + 1) / resolution;

                //correct 0.9999.. or 1.000000..1 to 1
                if ((i + 1) / resolution == 1) t = 1f;

                var pos = GetQuadraticPoint(start, end, handle, t);
                sum += (pos - temp).magnitude;
                temp = pos;
            }

            return sum;
        }

        public static TransformInfo[] GetQuadraticPoints(Transform start, Transform end, Transform handle, int resolution) {
            var result = new List<TransformInfo>();

            var temp = GetQuadraticPoint(start.position, end.position, handle.position, 0f);
            result.Add(new TransformInfo(temp, start.rotation, start.localScale));
            for (var i = 0; i < resolution; i++) {
                var t = (float)(i + 1) / resolution;

                //correct 0.9999.. or 1.000000..1 to 1
                if ((i + 1) / resolution == 1) t = 1f;

                var pos = GetQuadraticPoint(start.position, end.position, handle.position, t);
                //lerp rotation and scale
                var rotation = Quaternion.Lerp(start.rotation, end.rotation, t);
                var scale = Vector3.Lerp(start.localScale, end.localScale, t);

                result.Add(new TransformInfo(pos, rotation, scale));
                temp = pos;
            }

            return result.ToArray();
        }
    }
}