using System;

namespace XiheFramework.Runtime.Config {
    [AttributeUsage(AttributeTargets.Field)]
    public class ConfigAttribute : Attribute {
        public string Path { get; }
        public object DefaultValue { get; set; }

        public ConfigAttribute(string path = null) {
            Path = path;
        }
    }
}