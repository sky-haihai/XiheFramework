using System;

namespace XiheFramework.Runtime.Config {
    [Serializable]
    public class ConfigEntry {
        public string path;
        public ConfigType type;
        public object value;
    }
}