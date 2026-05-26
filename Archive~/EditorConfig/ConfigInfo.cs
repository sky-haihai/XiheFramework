using XiheFramework.Runtime.Config;

namespace XiheFramework.Editor.Runtime.Config {
    public class ConfigInfo {
        public string path;
        public ConfigType type;
        public object defaultValue;


        public ConfigInfo(string path, ConfigType type, object defaultValue) {
            this.path = path;
            this.type = type;
            this.defaultValue = defaultValue;
        }
    }
}