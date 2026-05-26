using System;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework.Runtime;

namespace XiheFramework.Utility.Csv2Json {
    public static class JsonLoader {
        public static List<T> LoadDataList<T>(string jsonAddress) {
            // 从Resources加载（无需.json扩展名）
            var jsonText = Game.Resource.LoadAsset<TextAsset>(jsonAddress);

            if (jsonText == null || string.IsNullOrEmpty(jsonText.text)) {
                Debug.LogError($"未找到JSON文件: {jsonAddress}");
                return new List<T>();
            }

            try {
                // 使用包装类解析数组形式的JSON
                InfoWrapper<T> wrapper = JsonUtility.FromJson<InfoWrapper<T>>(jsonText.text);
                return wrapper.Items;
            }
            catch (System.Exception e) {
                Debug.LogError($"解析JSON失败: {e.Message}");
                return new List<T>();
            }
        }

        /// <summary>
        /// JSON序列化包装类（用于处理数组/列表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        private class InfoWrapper<T> {
            public List<T> Items;
        }
        
        /// <summary>
        /// 深复制一个可序列化的数据类
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            string json = JsonUtility.ToJson(obj);
            return JsonUtility.FromJson<T>(json);
        }
    }
}