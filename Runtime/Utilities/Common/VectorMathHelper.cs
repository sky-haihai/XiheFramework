using UnityEngine;

namespace XiheFramework.Runtime.Utility {
    public static class VectorMathHelper {
        public static Vector2 GetRandomPointWithinCircle(float radius, Vector2 origin = default) {
            var ran = Random.Range(0, radius);
            var angle = Random.Range(0, 3.1415f * 2f);
            var x = ran * Mathf.Cos(angle);
            var y = ran * Mathf.Sin(angle);
            var pos = new Vector2(x, y) + origin;
            return pos;
        }

        public static Vector2 GetRandomPointWithinCircleUniformly(float radius, Vector2 origin = default) {
            var ran = Random.Range(0f, Mathf.Pow(radius, 2f));
            var angle = Random.Range(0f, Mathf.PI * 2f);
            var x = Mathf.Sqrt(ran) * Mathf.Cos(angle);
            var y = Mathf.Sqrt(ran) * Mathf.Sin(angle);
            var pos = new Vector2(x, y) + origin;
            return pos;
        }
    }
}