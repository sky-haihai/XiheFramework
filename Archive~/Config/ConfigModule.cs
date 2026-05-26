using System;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework.Runtime.Base;
using Object = System.Object;

namespace XiheFramework.Runtime.Config {
    public class ConfigModule : GameModuleBase {
        public override int Priority => CoreModulePriority.Config;

        public List<PresetConfigEntry> configSettings = new List<PresetConfigEntry>();

        private Dictionary<string, ConfigEntry> m_ConfigEntries = new Dictionary<string, ConfigEntry>();

        public void AddConfig(ConfigEntry configEntry) {
            if (!m_ConfigEntries.ContainsKey(configEntry.path)) {
                m_ConfigEntries.Add(configEntry.path, configEntry);
                if (enableDebug) {
                    Debug.Log($"[CFG]Add Config: {configEntry.path} to {configEntry.value}");
                }
            }
            else {
                var oldType = m_ConfigEntries[configEntry.path].type;
                var oldValue = m_ConfigEntries[configEntry.path].value;
                m_ConfigEntries[configEntry.path] = configEntry;
                Debug.LogWarning(
                    $"[CFG]Duplicated Config. Replacing Config:\"{configEntry.path}\" from [{oldType}] {oldValue} to [{configEntry.type}] {configEntry.value}");
            }
        }

        public void RemoveConfig(string configPath) {
            if (m_ConfigEntries.ContainsKey(configPath)) {
                m_ConfigEntries.Remove(configPath);
                Debug.Log($"Remove Config: {configPath}");
            }
        }

        public T FetchConfig<T>(string configPath) {
            if (!m_ConfigEntries.TryGetValue(configPath, out var entry)) {
                Debug.LogError($"[CFG]Config not found: {configPath}");
                return default;
            }

            if (entry.value is not T value) {
                Debug.LogError($"[CFG]Config type mismatch: {configPath} is not {typeof(T)}");
                return default;
            }

            return value;
        }

        public T FetchConfigDefaultPath<T>(Type ownerType, string fieldName) {
            var path = GetDefaultPath(ownerType, fieldName);
            return FetchConfig<T>(path);
        }

        public T FetchConfigDefaultPath<T>(Object ownerInstance, string fieldName) {
            return FetchConfigDefaultPath<T>(ownerInstance.GetType(), fieldName);
        }

        public string GetDefaultPath(Type type, string fieldName) {
            return ConfigHelper.GetDefaultPath(type, fieldName);
        }

        protected override void OnInstantiated() {
            foreach (var setting in configSettings) {
                var newEntry = new ConfigEntry {
                    path = setting.path,
                    type = setting.type,
                    value = setting.GetValue()
                };
                AddConfig(newEntry);
            }

            if (enableDebug) {
                Debug.Log($"[CONFIG]Loaded {m_ConfigEntries.Count} config entries");
            }

            Game.Config = this;
        }
    }
}