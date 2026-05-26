///Copyright Jiawei Peng 2021

using UnityEngine;

namespace XiheFramework.Utility.Extension {
    public static class VectorExtension {
        /// <summary>
        /// 将原本的Vector3中的Y归零，仅保留X和Z
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 ZeroY(this Vector3 v) {
            return new Vector3(v.x, 0, v.z);
        }

        /// <summary>
        /// 将原本的Vector3中的X和Z归零，仅保留Y
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 ZeroXZ(this Vector3 v) {
            return new Vector3(0, v.y, 0);
        }

        public static Vector3 ZeroZ(this Vector3 v) {
            return new Vector3(v.x, v.y, 0);
        }

        /// <summary>
        /// 只修改Vector3中的X
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 SetX(this Vector3 v, float x) {
            return new Vector3(x, v.y, v.z);
        }

        /// <summary>
        /// 只修改Vector3中的Y
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 SetY(this Vector3 v, float y) {
            return new Vector3(v.x, y, v.z);
        }

        /// <summary>
        /// 只修改Vector3中的Z
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 SetZ(this Vector3 v, float z) {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// 只修改Vector3中的Z
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 SetXZ(this Vector3 v, float xz) {
            return new Vector3(xz, v.y, xz);
        }

        /// <summary>
        /// 计算只包括X和Z的向量大小
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float MagnitudeXZ(this Vector3 v) {
            return ZeroY(v).magnitude;
        }

        /// <summary>
        /// 计算目标与世界Forward方向的夹角，返回0~360之间的数值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float AngleXZ(this Vector3 v) {
            return Vector3.Angle(Vector3.forward, ZeroY(v));
        }

        /// <summary>
        /// 计算目标与世界Up方向的夹角，返回0~360之间的数值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float AngleXY(this Vector3 v) {
            return Vector3.Angle(Vector3.up, ZeroZ(v));
        }

        /// <summary>
        /// 计算目标与世界Forward方向的夹角，返回-180~180之间的数值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float SignedAngleXZ(this Vector3 v) {
            return Vector3.SignedAngle(Vector3.forward, ZeroY(v), Vector3.up);
        }

        /// <summary>
        /// 计算目标与世界Up方向的夹角，返回-180~180之间的数值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float SignedAngleXY(this Vector3 v) {
            return Vector3.SignedAngle(Vector3.up, ZeroZ(v), Vector3.forward);
        }

        /// <summary>
        /// 计算目标与世界Forward方向的夹角，返回-180~180之间的数值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float SignedAngleXY(this Vector2 v) {
            return Vector3.SignedAngle(Vector3.up, v, Vector3.forward);
        }

        /// <summary>
        /// 计算其与世界Up向量之间的夹角，返回0~360之间的数值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Angle(this Vector2 v) {
            return Vector2.Angle(Vector2.up, v);
        }

        public static Vector3 RotationToForward(this Quaternion angle) {
            return angle * Vector3.forward;
        }

        /// <summary>
        /// 计算其与世界Up向量之间的夹角，返回-180~180之间的数值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float SignedAngle(this Vector2 v) {
            return Vector2.SignedAngle(Vector2.up, v);
        }

        /// <summary>
        /// 将Vector3转换为Vector2, 三维的Y轴对应二维的Y轴
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 ToVector2_XY2XY(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 XY_TO_XZ(this Vector3 v) {
            return new Vector3(v.x, 0, v.y);
        }

        public static Vector3 ToVector3_XY2XY(this Vector2 v) {
            return new Vector3(v.x, v.y, 0);
        }

        public static Vector3 ToVector3_XY2XZ(this Vector2 v) {
            return new Vector3(v.x, 0, v.y);
        }

        /// <summary>
        /// 只修改Vector3中的X
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector2 SetX(this Vector2 v, float x) {
            return new Vector2(x, v.y);
        }

        /// <summary>
        /// 只修改Vector3中的Y
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector2 SetY(this Vector2 v, float y) {
            return new Vector2(v.x, y);
        }

        /// <summary>
        /// 将Vector3转换为Vector2, 三维的Z轴对应二维的Y轴
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 ToVector2_XZ2XY(this Vector3 v) {
            return new Vector2(v.x, v.z);
        }

        /// <summary>
        /// 将数组转换为二维向量
        /// </summary>
        /// <param name="array">必须长度为2，否则返回Vector2</param>
        /// <returns></returns>
        public static Vector2 ToVector2(this float[] array) {
            if (array.Length == 2) {
                return new Vector2(array[0], array[1]);
            }

            Debug.LogWarning("数组转换为Vector2失败，传入的数组，长度不为2");
            return Vector2.zero;
        }

        public static Vector3 ToVector3(this float[] array) {
            if (array.Length == 2) {
                return new Vector3(array[0], array[1]);
            }
            else if (array.Length == 3) {
                return new Vector3(array[0], array[1], array[2]);
            }

            Debug.LogWarning("数组转换为Vector3失败，传入的数组，长度不为2或3");
            return Vector3.zero;
        }

        public static Vector2 PerpendicularClockwise(this Vector2 vector2) {
            return new Vector2(vector2.y, -vector2.x);
        }

        public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2) {
            return new Vector2(-vector2.y, vector2.x);
        }
    }
}