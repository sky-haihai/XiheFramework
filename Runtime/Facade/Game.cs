using System;
using UnityEngine;
using XiheFramework.Runtime.Base;
using XiheFramework.Runtime.Blackboard;
using XiheFramework.Runtime.Entity;
using XiheFramework.Runtime.Event;
using XiheFramework.Runtime.FSM;
using XiheFramework.Runtime.LogicTime;
using XiheFramework.Runtime.Resource;
using XiheFramework.Runtime.Scene;
using XiheFramework.Runtime.Serialization;
using XiheFramework.Runtime.UI;

namespace XiheFramework.Runtime {
    public static class Game {
        private static IXiheBlackboardModule s_Blackboard;
        private static IXiheEntityModule s_Entity;
        private static IXiheEventModule s_Event;
        private static IXiheStateMachineModule s_Fsm;
        private static IXiheLogicTimeModule s_LogicTime;
        private static IXiheResourceModule s_Resource;
        private static IXiheSceneModule s_Scene;
        private static IXiheSerializationModule s_Serialization;
        private static IXiheUIModule s_UI;

        public static bool IsCached { get; private set; }
        public static GameManager Manager => GameManager.Current;
        public static string GameName => GameManager.GameName;

        public static IXiheBlackboardModule Blackboard => GetCachedModule(ref s_Blackboard);
        public static IXiheEntityModule Entity => GetCachedModule(ref s_Entity);
        public static IXiheEventModule Event => GetCachedModule(ref s_Event);
        public static IXiheStateMachineModule Fsm => GetCachedModule(ref s_Fsm);
        public static IXiheStateMachineModule StateMachine => Fsm;
        public static IXiheLogicTimeModule LogicTime => GetCachedModule(ref s_LogicTime);
        public static IXiheResourceModule Resource => GetCachedModule(ref s_Resource);
        public static IXiheSceneModule Scene => GetCachedModule(ref s_Scene);
        public static IXiheSerializationModule Serialization => GetCachedModule(ref s_Serialization);
        public static IXiheUIModule UI => GetCachedModule(ref s_UI);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState() {
            ClearCache();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CacheModulesAfterSceneLoad() {
            CacheModules();
        }

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
            ClearCache();
        }

        public static string GetGameName() {
            return GameManager.GameName;
        }

        public static void LogMessage(string message) {
            Debug.Log(message);
        }

        public static void LogWarning(string message) {
            Debug.LogWarning(message);
        }

        public static void LogError(string message) {
            Debug.LogError(message);
        }

        public static void CacheModules() {
            GameManager.TryGetModule(out s_Blackboard);
            GameManager.TryGetModule(out s_Entity);
            GameManager.TryGetModule(out s_Event);
            GameManager.TryGetModule(out s_Fsm);
            GameManager.TryGetModule(out s_LogicTime);
            GameManager.TryGetModule(out s_Resource);
            GameManager.TryGetModule(out s_Scene);
            GameManager.TryGetModule(out s_Serialization);
            GameManager.TryGetModule(out s_UI);
            IsCached = Manager != null;
        }

        public static void ClearCache() {
            s_Blackboard = null;
            s_Entity = null;
            s_Event = null;
            s_Fsm = null;
            s_LogicTime = null;
            s_Resource = null;
            s_Scene = null;
            s_Serialization = null;
            s_UI = null;
            IsCached = false;
        }

        private static T GetCachedModule<T>(ref T module) where T : class {
            if (module is UnityEngine.Object unityObject && unityObject == null) {
                module = null;
            }

            if (module == null) {
                module = GameManager.GetModule<T>();
            }

            return module;
        }
    }
}
