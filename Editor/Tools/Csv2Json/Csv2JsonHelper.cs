using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XiheFramework.Editor.Tools.Csv2Json {
    public static class Csv2JsonHelper {
        /// <summary>
        /// 加载并转换CSV表格
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="csvPath">表格名称（不带扩展名）</param>
        /// <param name="outputDirectory"></param>
        /// <param name="firstLine"></param>
        public static void ConvertCsv2Json<T>(string csvPath, string outputDirectory, int firstLine) where T : new() {
            if (!Directory.Exists(outputDirectory)) {
                Directory.CreateDirectory(outputDirectory);
            }

            if (!typeof(T).IsSerializable) {
                Debug.LogError($"{typeof(T).Name} is not serializable. Add [System.Serializable] attribute.");
                return;
            }

            var tableName = Path.GetFileNameWithoutExtension(csvPath);
            string jsonPath = Path.Combine(outputDirectory, tableName + "Json.json");

            // 检查CSV文件是否存在
            if (!File.Exists(csvPath)) {
                Debug.LogError($"未找到CSV文件: {csvPath}");
                return;
            }

            try {
                // 读取CSV所有行
                string[] csvLines = File.ReadAllLines(csvPath);
                if (csvLines.Length < 2) {
                    Debug.LogError($"CSV文件为空或没有数据行: {csvPath}");
                    return;
                }

                // 解析表头（第一行）
                string[] headers = csvLines[0].Split(',');

                List<T> dataList = new List<T>(); // 存储解析后的数据列表
                System.Type type = typeof(T); // 获取目标类型信息

                // 处理每一行数据（从第三行开始）
                for (int i = firstLine - 1; i < csvLines.Length; i++) {
                    string[] values = ParseCsvLine(csvLines[i]); // 解析CSV行

                    // 检查列数是否匹配
                    if (values.Length != headers.Length) {
                        var valueString = string.Join(",", values);
                        Debug.LogWarning($"{tableName}表中，第{i + 1}行列数不匹配。预期: {headers.Length}, 实际: {values.Length}。数据: {valueString}");
                        continue;
                    }

                    T dataItem = new T(); // 创建新实例

                    // 遍历每一列
                    for (int j = 0; j < headers.Length; j++) {
                        string fieldName = headers[j].Trim(); // 字段名
                        var fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase); // 字段信息
                        var propertyInfo = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase); // 属性信息

                        if (fieldInfo == null && propertyInfo == null) {
                            Debug.LogWarning($"Cannot find field or property: {fieldName} in {tableName}, skipping this column.");
                        }
                        else {
                            // 解析值（根据目标类型）
                            object value = ParseValue(values[j].Trim(), fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType);

                            // 设置值
                            if (fieldInfo != null) {
                                fieldInfo.SetValue(dataItem, value);
                            }
                            else {
                                propertyInfo.SetValue(dataItem, value, null);
                            }
                        }
                    }

                    dataList.Add(dataItem); // 添加到列表
                }

                // 转换为JSON并保存
                string jsonData = JsonUtility.ToJson(new InfoWrapper<T> { Items = dataList }, true);
                File.WriteAllText(jsonPath, jsonData);
                Debug.Log($"成功转换{tableName}.csv到JSON: {jsonPath}");
            }
            catch (Exception e) {
                Debug.LogError($"转换表格{tableName}时出错: {e.Message}");
            }

            AssetDatabase.Refresh(); // 刷新资源数据库
        }

        /// <summary>
        /// 解析CSV单行（处理带逗号的引用内容）
        /// </summary>
        private static string[] ParseCsvLine(string line) {
            List<string> values = new List<string>();
            bool inQuotes = false; // 是否在引号内
            int startIndex = 0; // 当前值的起始位置

            for (int i = 0; i < line.Length; i++) {
                if (line[i] == '"') {
                    inQuotes = !inQuotes; // 切换引号状态
                }
                else if (line[i] == ',' && !inQuotes) {
                    // 找到分隔符且不在引号内
                    string value = line.Substring(startIndex, i - startIndex);
                    values.Add(value.Trim('"')); // 去除引号
                    startIndex = i + 1; // 更新起始位置
                }
            }

            // 添加最后一个值
            string lastValue = line.Substring(startIndex);
            values.Add(lastValue.Trim('"'));

            return values.ToArray();
        }

        /// <summary>
        /// 解析值（支持数组和基本类型）
        /// </summary>
        private static object ParseValue(string value, System.Type targetType) {
            // 处理空值
            if (string.IsNullOrEmpty(value)) {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            // 处理数组类型
            if (targetType.IsArray) {
                System.Type elementType = targetType.GetElementType();
                string[] elements = value.Split('|'); // 使用"|"分割数组元素
                Array array = Array.CreateInstance(elementType, elements.Length);

                for (int i = 0; i < elements.Length; i++) {
                    array.SetValue(ParseSingleValue(elements[i], elementType), i);
                }

                return array;
            }

            // 处理List<T>类型
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)) {
                System.Type elementType = targetType.GetGenericArguments()[0];
                string[] elements = value.Split('|');
                var list = (System.Collections.IList)Activator.CreateInstance(targetType);

                foreach (string element in elements) {
                    list.Add(ParseSingleValue(element, elementType));
                }

                return list;
            }

            // 处理单值类型
            return ParseSingleValue(value, targetType);
        }

        /// <summary>
        /// 解析单值（基本类型）
        /// </summary>
        private static object ParseSingleValue(string value, System.Type targetType) {
            if (targetType == typeof(string)) {
                return value;
            }
            else if (targetType == typeof(uint)) {
                return uint.TryParse(value, out uint result) ? result : 0;
            }
            else if (targetType == typeof(int)) {
                return int.TryParse(value, out int result) ? result : 0;
            }
            else if (targetType == typeof(float)) {
                return float.TryParse(value, out float result) ? result : 0f;
            }
            else if (targetType == typeof(bool)) {
                return value.ToLower() == "true" || value == "1";
            }
            else if (targetType.IsEnum) {
                try {
                    return Enum.Parse(targetType, value);
                }
                catch {
                    return Enum.GetValues(targetType).GetValue(0);
                }
            }

            return null;
        }

        /// <summary>
        /// JSON序列化包装类（用于处理数组/列表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        private class InfoWrapper<T> {
            public List<T> Items;
        }
    }
}
