using System;
using System.Collections.Generic;
using System.Linq;

namespace XiheFramework.Runtime.Utility {
    public static class SystemUtil {
        public static int[] AllIndexesOf(this string str, string substr, bool ignoreCase = false) {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(substr)) throw new ArgumentException("String or substring is not specified.");

            var indexes = new List<int>();
            var index = 0;

            while ((index = str.IndexOf(substr, index, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) != -1) indexes.Add(index++);

            return indexes.ToArray();
        }

        public static IEnumerable<Type> GetAllTypesImplemented<T>() {
            var resultTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            return resultTypes;
        }
    }
}