using System;
using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime {
    public static class Game {
        public static GameManager Manager { get; internal set; }

        public static void InstantiateGameModule<T>(Action onInstantiated = null) where T : GameModuleBase {
            GameManager.InstantiatePresetGameModule<T>(onInstantiated);
        }

        public static void InstantiateGameModuleAsync<T>(Action onInstantiated = null) where T : GameModuleBase {
            GameManager.InstantiatePresetGameModuleAsync<T>(onInstantiated);
        }

        public static T GetModule<T>() where T : class {
            return GameManager.GetModule<T>();
        }

        public static bool TryGetModule<T>(out T module) where T : class {
            return GameManager.TryGetModule(out module);
        }

        public static void DestroyFramework() {
            GameManager.DestroyFramework();
        }

        public static string GetGameName() {
            return GameManager.GameName;
        }

        public static void LogMessage(string message) {
            UnityEngine.Debug.Log(message);
        }

        public static void LogWarning(string message) {
            UnityEngine.Debug.LogWarning(message);
        }

        public static void LogError(string message) {
            UnityEngine.Debug.LogError(message);
        }
    }
}
