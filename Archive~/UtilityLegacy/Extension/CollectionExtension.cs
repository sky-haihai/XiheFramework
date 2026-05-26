using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiheFramework.Utility.Extension
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 获取最大Index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int MaxIndex<T>(this IList<T> list)
        {
            return list.Count - 1;
        }

        /// <summary>
        /// 获取List最后一位的对象或数据，方法构成简单，可以大量调用。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T LastOne<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        public static void RemoveLastOne<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// 向列表内添加多个new的T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> AddManyNew<T>(this IList<T> list, int times) where T : new()
        {
            for (int i = 0; i < times; i++)
            {
                list.Add(new T());
            }
            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> AddMany_Default<T>(this IList<T> list, int times)
        {
            for (int i = 0; i < times; i++)
            {
                list.Add(default);
            }
            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<string> AddTo(this IList<string> list, int times)
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(string.Empty);
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> AddTo_Default<T>(this IList<T> list, int times)
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(default);
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> AddTo_Class<T>(this IList<T> list, int times) where T : class, new()
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(new T());
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> ChangeTo_New<T>(this IList<T> list, int times) where T : new()
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(new T());
                }
            }
            else if (len < 0 && list.Count > 0)
            {
                for (int i = list.Count - 1; i > list.Count + len; --i)
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<string> ChangeTo_String(this IList<string> list, int times)
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(string.Empty);
                }
            }
            else if (len < 0 && list.Count > 0)
            {
                for (int i = list.Count - 1; i > list.Count + len; --i)
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> ChangeTo_Default<T>(this IList<T> list, int times)
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(default);
                }
            }
            else if (len < 0 && list.Count > 0)
            {
                for (int i = list.Count - 1; i > list.Count + len; --i)
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<int> ChangeTo_Int(this IList<int> list, int times, int value = 0)
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(value);
                }
            }
            else if (len < 0 && list.Count > 0)
            {
                for (int i = list.Count - 1; i > list.Count + len; --i)
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<float> ChangeTo_Float(this IList<float> list, int times)
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(0f);
                }
            }
            else if (len < 0 && list.Count > 0)
            {
                for (int i = list.Count - 1; i >= times; --i)
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<float> SetAllTo(this IList<float> list, float value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = value;
            }

            return list;
        }

        /// <summary>
        /// 向列表内添加多个null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> AddManyNull<T>(this IList<T> list, int times) where T : class
        {
            for (int i = 0; i < times; i++)
            {
                list.Add(null);
            }
            return list;
        }

        /// <summary>
        /// 向列表内添加多个default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="times">要添加多少个new的T</param>
        /// <returns>返回列表本身</returns>
        public static IList<T> AddTo_Null<T>(this IList<T> list, int times) where T : class
        {
            int len = times - list.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    list.Add(null);
                }
            }

            return list;
        }

        public static void ClearNull<T>(this List<T> list) where T : new()
        {
            for (int i = list.Count; i >= 0; --i)
            {
                if (list == null)
                    list.RemoveAt(i);
            }
        }

        public static void ClearNull<T>(this HashSet<T> list) where T : new()
        {
            list.RemoveWhere((x) => x == null);
        }

        public static T RandomPickTable<T>(this List<T> list, UnityAction<int, PassFloatToMe> perItemCallback) where T : class
        {
            PassFloatToMe passFloatToMe = new PassFloatToMe();
            float totalTable = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                perItemCallback(i, passFloatToMe);
                totalTable += passFloatToMe.Value;
            }
            float targetValue = UnityEngine.Random.Range(0, totalTable);
            totalTable = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                perItemCallback(i, passFloatToMe);
                totalTable += passFloatToMe.Value;
                if (totalTable >= targetValue)
                {
                    return list[i];
                }
            }
            return null;
        }

        public static List<T> RandomPick<T>(this List<T> list, uint PickCount)
        {
            PickCount = (uint)Mathf.Min(PickCount, list.Count);
            List<T> Picked = new List<T>();
            for (uint i = 0; i < PickCount; i++)
            {
                Picked.Add(list[UnityEngine.Random.Range(0, list.Count)]);
            }
            return Picked;
        }

        public static List<T> RandomPickNotRepeat<T>(this List<T> list, uint PickCount)
        {
            PickCount = (uint)Mathf.Min(PickCount, list.Count);
            List<T> Picked = new List<T>();
            T[] copy = new T[list.Count];
            list.CopyTo(copy);
            int end = list.Count;
            int num;

            for (uint i = 0; i < PickCount; i++)
            {
                num = UnityEngine.Random.Range(0, end);
                Picked.Add(copy[num]);
                copy[num] = copy[end - 1];
                end--;
            }

            return Picked;
        }

        public static void RandomizeList<T>(this List<T> sources)
        {
            System.Random rd = new System.Random();
            int index;
            T temp;
            for (int i = 0; i < sources.Count; i++)
            {
                index = rd.Next(0, sources.Count - 1);
                if (index != i)
                {
                    temp = sources[i];
                    sources[i] = sources[index];
                    sources[index] = temp;
                }
            }
        }

        public static void AddEX<TKey, TValue>(this Dictionary<TKey, List<TValue>> dic, TKey key, TValue value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, new List<TValue>() { value });
            else
                dic[key].Add(value);
        }

        public static void AddEXNotRepeat<TKey, TValue>(this Dictionary<TKey, List<TValue>> dic, TKey key, TValue value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, new List<TValue>() { value });
            else if(!dic[key].Contains(value))
            {
                dic[key].Add(value);
            }
        }

        public static void AddEXNotRepeat<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dic, TKey key, TValue value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, new HashSet<TValue>() { value });
            else if (!dic[key].Contains(value))
            {
                dic[key].Add(value);
            }
        }

        public static void ReplaceEX<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dic, TKey key, HashSet<TValue> value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, value);
            else
            {
                dic[key] = value;
            }
        }
    }

    public class PassFloatToMe
    {
        public float Value { get; private set; }

        public void SetValue(float value)
        {
            Value = value;
        }
    }
}