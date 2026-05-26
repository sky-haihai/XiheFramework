using UnityEngine;

namespace XiheFramework.Utility.Extension
{
    public static class ColorExtension
    {
        /// <summary>
        /// 修改32位Color的Alpha值
        /// </summary>
        /// <param name="color32"></param>
        /// <param name="a">数值范围：0-255</param>
        /// <returns></returns>
        public static Color32 WithAlpha(this Color32 color32, int a)
        {
            color32.a = (byte)a;
            return color32;
        }

        /// <summary>
        /// 修改Color的Alpha值
        /// </summary>
        /// <param name="color"></param>
        /// <param name="a">数值范围：0-1</param>
        /// <returns></returns>
        public static Color WithAlpha(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        /// <summary>
        /// 取出Color的RGB值。Alpha为1。
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color RGB(this Color color)
        {
            return new Color(color.r, color.g, color.b, 1);
        }
    }
}