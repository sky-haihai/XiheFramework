using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework.Runtime.Utility.DataStructure {
    public class MultiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, List<TValue>>> {
        private readonly Dictionary<TKey, List<TValue>> m_Dictionary = new();

        public List<TValue> this[TKey key] => m_Dictionary[key];

        public Dictionary<TKey, List<TValue>>.KeyCollection Keys => m_Dictionary.Keys;
        public Dictionary<TKey, List<TValue>>.ValueCollection Values => m_Dictionary.Values;

        public int Count => m_Dictionary.Count;


        public MultiDictionary() { }

        public MultiDictionary(Dictionary<TKey, List<TValue>> dictionary) {
            m_Dictionary = dictionary;
        }

        public MultiDictionary(IEnumerable<KeyValuePair<TKey, List<TValue>>> source) {
            m_Dictionary = new Dictionary<TKey, List<TValue>>(source);
        }

        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator() {
            return m_Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(TKey key, TValue value) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                return;
            }

            if (value == null) {
                Debug.LogError("MultiDictionary value is null");
                return;
            }

            if (m_Dictionary.TryGetValue(key, out var targetList)) {
                targetList.Add(value);
            }
            else {
                var list = new List<TValue> { value };
                m_Dictionary.Add(key, list);
            }
        }

        public void AddRange(TKey key, IEnumerable<TValue> values) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                return;
            }

            if (values == null) {
                Debug.LogError("MultiDictionary values is null");
                return;
            }

            if (m_Dictionary.TryGetValue(key, out var targetList)) {
                targetList.AddRange(values);
            }
            else {
                var list = new List<TValue>(values);
                m_Dictionary.Add(key, list);
            }
        }
        
        public void Remove(TKey key, TValue value) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                return;
            }

            if (value == null) {
                Debug.LogError("MultiDictionary value is null");
                return;
            }

            if (!m_Dictionary.ContainsKey(key)) {
                Debug.LogErrorFormat("MultiDictionary Key {0} is not existed", key);
                return;
            }

            if (!m_Dictionary[key].Contains(value)) {
                Debug.LogErrorFormat("MultiDictionary Value {0} is not existed", value);
                return;
            }

            m_Dictionary[key].Remove(value);
            if (m_Dictionary[key].Count == 0) m_Dictionary.Remove(key);
        }

        public void RemoveList(TKey key) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                return;
            }

            if (!m_Dictionary.ContainsKey(key)) {
                Debug.LogErrorFormat("MultiDictionary Key {0} is not existed", key);
                return;
            }

            m_Dictionary.Remove(key);
        }

        public List<TValue> GetList(TKey key) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                return null;
            }

            return m_Dictionary[key];
        }

        public bool ContainsKey(TKey key) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                return false;
            }

            return m_Dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out List<TValue> value) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                value = null;
                return false;
            }

            return m_Dictionary.TryGetValue(key, out value);
        }

        public bool ContainsValue(TKey key, TValue value) {
            if (key == null) {
                Debug.LogError("MultiDictionary key is null");
                return false;
            }

            if (value == null) {
                Debug.LogError("MultiDictionary value is null");
                return false;
            }

            if (!m_Dictionary.ContainsKey(key)) return false;

            return m_Dictionary[key].Contains(value);
        }

        public void Clear() {
            m_Dictionary.Clear();
        }

        public static implicit operator MultiDictionary<TKey, TValue>(Dictionary<TKey, List<TValue>> dictionary) {
            return new MultiDictionary<TKey, TValue>(dictionary);
        }

        public static implicit operator Dictionary<TKey, List<TValue>>(MultiDictionary<TKey, TValue> dictionary) {
            return dictionary.m_Dictionary;
        }
    }
}