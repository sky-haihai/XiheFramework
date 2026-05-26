using System;
using UnityEngine;

namespace XiheFramework.Runtime.Config {
    [Serializable]
    public class PresetConfigEntry {
        public string path;
        public ConfigType type;

        public int intValue;
        public bool boolValue;
        public float floatValue;
        public string stringValue;
        public Color colorValue;
        public LayerMask layerMaskValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Vector4 vector4Value;
        public AnimationCurve animationCurveValue;

        public object GetValue() {
            switch (type) {
                case ConfigType.Integer:
                    return intValue;
                case ConfigType.Boolean:
                    return boolValue;
                case ConfigType.Float:
                    return floatValue;
                case ConfigType.String:
                    return stringValue;
                case ConfigType.Color:
                    return colorValue;
                case ConfigType.LayerMask:
                    return layerMaskValue;
                case ConfigType.Vector2:
                    return vector2Value;
                case ConfigType.Vector3:
                    return vector3Value;
                case ConfigType.Vector4:
                    return vector4Value;
                case ConfigType.AnimationCurve:
                    return animationCurveValue;
                default:
                    return null;
            }
        }

        public void SetValue(object value) {
            try {
                switch (type) {
                    case ConfigType.Integer:
                        intValue = (int)Convert.ChangeType(value, typeof(int));
                        break;
                    case ConfigType.Boolean:
                        boolValue = (bool)Convert.ChangeType(value, typeof(bool));
                        break;
                    case ConfigType.Float:
                        floatValue = (float)Convert.ChangeType(value, typeof(float));
                        break;
                    case ConfigType.String:
                        stringValue = (string)Convert.ChangeType(value, typeof(string));
                        break;
                    case ConfigType.Color:
                        colorValue = (Color)Convert.ChangeType(value, typeof(Color));
                        break;
                    case ConfigType.LayerMask:
                        layerMaskValue = (LayerMask)Convert.ChangeType(value, typeof(LayerMask));
                        break;
                    case ConfigType.Vector2:
                        vector2Value = (Vector2)Convert.ChangeType(value, typeof(Vector2));
                        break;
                    case ConfigType.Vector3:
                        vector3Value = (Vector3)Convert.ChangeType(value, typeof(Vector3));
                        break;
                    case ConfigType.Vector4:
                        vector4Value = (Vector4)Convert.ChangeType(value, typeof(Vector4));
                        break;
                    case ConfigType.AnimationCurve:
                        animationCurveValue = (AnimationCurve)Convert.ChangeType(value, typeof(AnimationCurve));
                        break;
                    default:
                        Debug.LogWarning($"Type {type} is not supported, please configure {path} manually.");
                        break;
                }
            }
            catch (Exception e) {
                Debug.LogWarning(
                    $"[CONFIG] Path: {path}, Value: {value}, Type: {value.GetType().Name} cannot be converted to Config Type: {type}, please configure it manually. Error: {e}");
            }
        }
    }
}