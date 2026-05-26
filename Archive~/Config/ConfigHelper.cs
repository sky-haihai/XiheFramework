using System;
using UnityEngine;

namespace XiheFramework.Runtime.Config {
    public class ConfigHelper {
        public static ConfigType GetConfigType(Type type) {
            if (type == typeof(int)) {
                return ConfigType.Integer;
            }

            if (type == typeof(bool)) {
                return ConfigType.Boolean;
            }

            if (type == typeof(float)) {
                return ConfigType.Float;
            }

            if (type == typeof(string)) {
                return ConfigType.String;
            }

            if (type == typeof(Color)) {
                return ConfigType.Color;
            }

            if (type == typeof(LayerMask)) {
                return ConfigType.LayerMask;
            }

            if (type == typeof(Vector2)) {
                return ConfigType.Vector2;
            }

            if (type == typeof(Vector3)) {
                return ConfigType.Vector3;
            }

            if (type == typeof(Vector4)) {
                return ConfigType.Vector4;
            }

            if (type == typeof(AnimationCurve)) {
                return ConfigType.AnimationCurve;
            }

            Debug.LogError($"[CFG] Unsupported config type: {type.FullName}");
            return ConfigType.String;
        }

        public static string GetDefaultPath(Type type, string fieldName) {
            string ns = type.Namespace ?? "Global";
            string className = type.Name;
            return $"{ns}.{className}.{fieldName}";
        }
    }
}