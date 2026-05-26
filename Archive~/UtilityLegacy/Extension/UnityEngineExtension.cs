using System;
using System.Collections;
using UnityEngine;

namespace XiheFramework.Utility.Extension {
    public static class UnityEngineExtension {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            var component = gameObject.GetComponent<T>();
            if (component != null) return component;

            return gameObject.AddComponent<T>();
        }

        public static Vector2 ToVector2(this Vector3 vector3, V3ToV2Type convertType) {
            var result = convertType switch {
                V3ToV2Type.XY => new Vector2(vector3.x, vector3.y),
                V3ToV2Type.XZ => new Vector2(vector3.x, vector3.z),
                V3ToV2Type.YZ => new Vector2(vector3.y, vector3.z),
                _ => throw new ArgumentOutOfRangeException(nameof(convertType), convertType, null)
            };

            return result;
        }

        public static Vector3 ToVector3(this Vector2 vector2, V2ToV3Type convertType) {
            var result = convertType switch {
                V2ToV3Type.XY => new Vector3(vector2.x, vector2.y, 0f),
                V2ToV3Type.XZ => new Vector3(vector2.x, 0f, vector2.y),
                V2ToV3Type.YZ => new Vector3(0f, vector2.x, vector2.y),
                _ => throw new ArgumentOutOfRangeException(nameof(convertType), convertType, null)
            };

            return result;
        }

        //hard clamp (change direction)
        public static Vector2 Clamp(this Vector2 vector2, float min, float max) {
            var x = Mathf.Clamp(vector2.x, min, max);
            var y = Mathf.Clamp(vector2.y, min, max);

            return new Vector2(x, y);
        }

        //hard clamp (change direction)
        public static Vector2 Clamp(this Vector2 vector2, Vector2 min, Vector2 max) {
            var x = Mathf.Clamp(vector2.x, min.x, max.x);
            var y = Mathf.Clamp(vector2.y, min.y, max.y);

            return new Vector2(x, y);
        }

        public static Vector2 Clamp(this Vector2 vector2, Vector4 bound) {
            return vector2.Clamp(new Vector2(bound.x, bound.y), new Vector2(bound.z, bound.w));
        }

        //proportional clamp (change length)
        public static Vector3 ClampProportional(this Vector3 vector3, float min, float max) {
            if (vector3.magnitude < min)
                return vector3.normalized * min;
            if (vector3.magnitude > max) return vector3.normalized * max;

            return vector3;
        }

        //x - x min
        //y - x max 
        //z - y min
        //w - y max
        public static bool Contain(this Vector4 area, Vector2 target) {
            if (target.x < area.x) return false;

            if (target.x > area.y) return false;

            if (target.y < area.z) return false;

            if (target.y > area.w) return false;

            return true;
        }

        public static Vector2 GetNearestPointFromOutside(this Vector2 origin, Vector4 area) {
            //0
            if (origin.x < area.x && origin.y > area.w) return new Vector2(area.x, area.w);

            //1
            if (origin.x > area.x && origin.x < area.y && origin.y > area.w) return new Vector2(origin.x, area.w);

            //2
            if (origin.x > area.y && origin.y > area.w) return new Vector2(area.y, area.w);

            //3
            if (origin.x < area.x && origin.y > area.z && origin.y < area.w) return new Vector2(area.x, origin.y);

            //4
            //middle situation implement later
            //currently return origin at the end

            //5
            if (origin.x > area.y && origin.y > area.z && origin.y < area.w) return new Vector2(area.y, origin.y);

            //6
            if (origin.x < area.x && origin.y < area.z) return new Vector2(area.x, area.z);

            //7
            if (origin.x > area.x && origin.x < area.y && origin.y < area.z) return new Vector2(origin.x, area.z);

            //8
            if (origin.x > area.y && origin.y < area.z) return new Vector2(area.y, area.z);

            return origin;
        }

        public static float GetAnimationClipLength(this Animator animator, string name) {
            var runtime = animator.runtimeAnimatorController;
            foreach (var clip in runtime.animationClips)
                if (clip.name.Equals(name))
                    return clip.length;

            return 0f;
        }

        public static void SetLightIntensitySmooth(this Light light, MonoBehaviour owner, float targetIntensity, float duration) {
            owner.StopAllCoroutines();
            owner.StartCoroutine(SetLightIntensity(light, targetIntensity, duration));
        }

        private static IEnumerator SetLightIntensity(Light targetLight, float targetIntensity, float duration) {
            while (Math.Abs(targetLight.intensity - targetIntensity) > 0.001f) {
                var intensity = targetLight.intensity;

                var delta = (targetIntensity - intensity) * Time.deltaTime / duration;
                intensity += delta;

                targetLight.intensity = intensity;

                if (Math.Abs(intensity - targetIntensity) < delta) targetLight.intensity = targetIntensity;

                yield return null;
            }

            targetLight.intensity = targetIntensity;

            yield return null;
        }

        public static bool Includes(this LayerMask mask, int layer) {
            return mask == (mask | (1 << layer));
        }

        public static bool IncludesBitMask(this uint mask1, uint mask2) {
            return (mask1 & mask2) != 0;
        }
    }


    public enum V3ToV2Type {
        XY,
        XZ,
        YZ
    }

    public enum V2ToV3Type {
        XY,
        XZ,
        YZ
    }
}