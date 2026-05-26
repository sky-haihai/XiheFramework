using System.Collections;
using System.Collections.Generic;

namespace XiheFramework.Runtime.Utility.DataStructure
{
    /// <summary>
    /// A data type keeps key -> value and value -> key mappings and maintains them.
    /// </summary>
    /// <typeparam name="TKey">type of the key</typeparam>
    /// <typeparam name="TValue">type of the value</typeparam>
    public interface IBiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Get access to the key map
        /// </summary>
        Dictionary<TKey, TValue> KeyMap { get; }

        /// <summary>
        /// Get access to the value map
        /// </summary>
        Dictionary<TValue, TKey> ValueMap { get; }

        /// <summary>
        /// Adds a key value pair
        /// </summary>
        /// <param name="key">key to add</param>
        /// <param name="value">value to add</param>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Removes the key and its correspondent value
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>true if the key exists and removed succesfully</returns>
        bool RemoveKey(TKey key);

        /// <summary>
        /// Removes the value and its correspondent key
        /// </summary>
        /// <param name="value">value to remove</param>
        /// <returns>true if the value exists and removed successfully</returns>
        bool RemoveValue(TValue value);

        /// <summary>
        /// Clears out all the elements
        /// </summary>
        void Clear();

        /// <summary>
        /// Checks if the key exists
        /// </summary>
        /// <param name="key">key to check</param>
        /// <returns>true if the key exists</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Checks if the value exists
        /// </summary>
        /// <param name="value">value to check</param>
        /// <returns>true if the value exists</returns>
        bool ContainsValue(TValue value);

        /// <summary>
        /// Gives the count of elements in the Bidictionary
        /// Remark: it counts the key -> value and value -> key elements as 1
        /// </summary>
        /// <returns>Number of elements in the dictionary</returns>
        int Count();

        /// <summary>
        /// Gets the enumerator for keys
        /// </summary>
        /// <returns>enumerator for key to values</returns>
        new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        /// <summary>
        /// Gets the enumerator for values
        /// </summary>
        /// <returns>enumerator for value to keys</returns>
        IEnumerator<KeyValuePair<TValue, TKey>> GetValueEnumerator();
    }

    public class BiDictionary<TKey, TValue> : IBiDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> forward = new Dictionary<TKey, TValue>();
        private readonly Dictionary<TValue, TKey> reverse = new Dictionary<TValue, TKey>();

        public BiDictionary()
        {
            KeyMap = forward;
            ValueMap = reverse;
        }

        public Dictionary<TKey, TValue> KeyMap { get; private set; }
        public Dictionary<TValue, TKey> ValueMap { get; private set; }

        public int Count() => forward.Count;

        public void Add(TKey key, TValue value)
        {
            if (key != null && value != null)
            {
                forward.Add(key, value);
                reverse.Add(value, key);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return forward.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return reverse.ContainsKey(value);
        }

        public bool RemoveKey(TKey key)
        {
            var value = forward[key];
            return value != null && reverse.ContainsKey(value)
                && reverse.Remove(value) && forward.Remove(key);
        }

        public bool RemoveValue(TValue value)
        {
            var key = reverse[value];
            return key != null && forward.ContainsKey(key)
                && forward.Remove(key) && reverse.Remove(value);
        }

        public IEnumerator<KeyValuePair<TValue, TKey>> GetValueEnumerator()
        {
            return reverse.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return forward.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return forward.GetEnumerator();
        }

        public void Clear()
        {
            forward.Clear();
            reverse.Clear();
        }
    }
}