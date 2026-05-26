using UnityEngine;

namespace XiheFramework.Runtime.Serialization {
    public static class SerializationUtil {
        //int
        public static string ParseIntToString(int value) {
            return value.ToString();
        }

        public static int ParseStringToInt(string value) {
            return int.Parse(value);
        }


        //float
        public static string ParseFloatToString(float value) {
            return $"{value}";
        }

        public static float ParseStringToFloat(string value) {
            return float.Parse(value);
        }

        //bool
        public static string ParseBoolToString(bool value) {
            return value ? "True" : "False";
        }

        public static bool ParseStringToBool(string value) {
            return bool.Parse(value);
        }

        //vector3
        public static string ParseVector3ToString(Vector3 value) {
            return $"{value.x},{value.y},{value.z}";
        }

        public static Vector3 ParseStringToVector3(string value) {
            var target = new Vector3();
            var strs = value.Split(',');
            if (strs.Length == 3) {
                target.x = float.Parse(strs[0]);
                target.y = float.Parse(strs[1]);
                target.z = float.Parse(strs[2]);
            }

            return target;
        }
    }
}