// -----------------------------------
// XiheFramework
// Author: sky_haihai, yifeng
// -----------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace XiheFramework.Runtime.Base {
    /// <summary>
    /// Game manager for XiheFramework.
    /// </summary>
    [DefaultExecutionOrder(300)]
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private string m_GameName = "My Game";

        [Header("Core Modules")]
        [SerializeField]
        private bool m_AutoCreateCoreModules = true;

        [SerializeField]
        private List<XiheCoreModuleSelection> m_CoreModules = new();

        [Header("Custom Modules")]
        [SerializeField]
        private List<GameModuleBase> m_CustomGameModulePrefabs = new();

        [Header("Debug")]
        [SerializeField]
        private bool m_EnableFrameworkDebug;

        private readonly Dictionary<Type, GameModuleBase> m_PresetGameModules = new();
        private readonly Dictionary<Type, GameModuleBase> m_AliveGameModules = new();
        private readonly Queue<GameModuleRegistrationInfo> m_RegisterGameModulesQueue = new();
        private readonly Dictionary<Type, int> m_AliveGameModuleUpdateTimers = new();
        private readonly Dictionary<Type, int> m_AliveGameModuleFixedUpdateTimers = new();
        private readonly Dictionary<Type, int> m_AliveGameModuleLateUpdateTimers = new();
        private readonly Dictionary<Type, int> m_AliveGameModuleRegistrationOrders = new();
        private readonly List<Type> m_AliveGameModuleExecutionOrder = new();
        private Type[] m_AliveGameModuleExecutionOrderCache = Array.Empty<Type>();
        private int m_NextGameModuleRegistrationOrder;

        private Transform m_GameModuleRoot;

        private static GameManager m_Instance;

        private static GameManager Instance {
            get {
                if (m_Instance == null) {
#if UNITY_2023_1_OR_NEWER
                    m_Instance = FindFirstObjectByType<GameManager>();
#else
                    m_Instance = FindObjectOfType<GameManager>();
#endif
                }

                return m_Instance;
            }
        }

        public static string GameName => Instance != null ? Instance.m_GameName : string.Empty;

        private void Awake() {
            m_Instance = this;
            Game.Manager = this;

            m_GameModuleRoot = new GameObject("GameModules(Instantiated)").transform;
            m_GameModuleRoot.SetParent(transform, false);

            EnsureCoreModuleSelections();
            CachePresetModules(m_CustomGameModulePrefabs);

            var startupModules = new List<GameModuleBase>();
            if (m_AutoCreateCoreModules) {
                startupModules.AddRange(CreateSelectedCoreModules());
            }

            startupModules.AddRange(InstantiatePresetModules());
            InstantiateStartupModules(startupModules);

            Debug.Log("XiheFramework Initialized");
        }

        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            foreach (var gameModuleType in m_AliveGameModuleExecutionOrderCache) {
                if (!m_AliveGameModules.TryGetValue(gameModuleType, out var aliveGameModule)) {
                    continue;
                }

                if (m_AliveGameModuleUpdateTimers[gameModuleType] > 0) {
                    m_AliveGameModuleUpdateTimers[gameModuleType] -= 1;
                    continue;
                }

                aliveGameModule.OnUpdateInternal();
                m_AliveGameModuleUpdateTimers[gameModuleType] = aliveGameModule.updateInterval;
            }
        }

        private void FixedUpdate() {
            foreach (var gameModuleType in m_AliveGameModuleExecutionOrderCache) {
                if (!m_AliveGameModules.TryGetValue(gameModuleType, out var aliveGameModule)) {
                    continue;
                }

                if (m_AliveGameModuleFixedUpdateTimers[gameModuleType] > 0) {
                    m_AliveGameModuleFixedUpdateTimers[gameModuleType] -= 1;
                    continue;
                }

                aliveGameModule.OnFixedUpdateInternal();
                m_AliveGameModuleFixedUpdateTimers[gameModuleType] = aliveGameModule.fixedUpdateInterval;
            }
        }

        private void LateUpdate() {
            foreach (var gameModuleType in m_AliveGameModuleExecutionOrderCache) {
                if (!m_AliveGameModules.TryGetValue(gameModuleType, out var aliveGameModule)) {
                    continue;
                }

                if (m_AliveGameModuleLateUpdateTimers[gameModuleType] > 0) {
                    m_AliveGameModuleLateUpdateTimers[gameModuleType] -= 1;
                    continue;
                }

                aliveGameModule.OnLateUpdateInternal();
                m_AliveGameModuleLateUpdateTimers[gameModuleType] = aliveGameModule.lateUpdateInterval;
            }

            ProcessGameModuleRegistrationQueue();
        }

        public static void InstantiatePresetGameModule(Type gameModuleType, Action onInstantiated = null) {
            if (Instance == null) {
                Debug.LogError("[GAME MANAGER] No GameManager instance exists.");
                return;
            }

            if (!typeof(GameModuleBase).IsAssignableFrom(gameModuleType)) {
                Debug.LogError($"[GAME MANAGER] GameModule: {gameModuleType.Name} is not a GameModuleBase");
                return;
            }

            if (!Instance.m_PresetGameModules.TryGetValue(gameModuleType, out var prefab)) {
                Debug.LogError($"[GAME MANAGER] GameModule: {gameModuleType.Name} does not exist in custom module prefabs");
                return;
            }

            if (Instance.m_AliveGameModules.ContainsKey(gameModuleType)) {
                Debug.LogError($"[GAME MANAGER] Component: {gameModuleType} has already been registered");
                return;
            }

            var gameModule = Instantiate(prefab, Instance.m_GameModuleRoot, true);
            if (gameModule == null) {
                Debug.LogError($"[GAME MANAGER] GameModule: {gameModuleType.Name} failed to instantiate");
                return;
            }

            Instance.ApplyDebugSettings(gameModule);
            Instance.RegisterAliveGameModule(gameModuleType, gameModule);
            gameModule.OnInstantiatedInternal(onInstantiated);
        }

        public static void InstantiatePresetGameModule<T>(Action onInstantiated = null) where T : GameModuleBase {
            InstantiatePresetGameModule(typeof(T), onInstantiated);
        }

        public static void InstantiatePresetGameModuleAsync(Type gameModuleType, Action onInstantiated) {
            if (Instance == null) {
                Debug.LogError("[GAME MANAGER] No GameManager instance exists.");
                return;
            }

            if (!Instance.m_PresetGameModules.TryGetValue(gameModuleType, out var prefab)) {
                Debug.LogError($"[GAME MANAGER] GameModule: {gameModuleType.Name} does not exist in custom module prefabs");
                return;
            }

            if (Instance.m_AliveGameModules.ContainsKey(gameModuleType)) {
                Debug.LogError($"[GAME MANAGER] Component: {gameModuleType} has already been registered");
                return;
            }

            var gameModule = Instantiate(prefab, Instance.m_GameModuleRoot, true);
            if (gameModule == null) {
                Debug.LogError($"[GAME MANAGER] GameModule: {gameModuleType.Name} failed to instantiate");
                return;
            }

            Instance.ApplyDebugSettings(gameModule);
            Instance.m_RegisterGameModulesQueue.Enqueue(new GameModuleRegistrationInfo(gameModuleType, gameModule, onInstantiated));
        }

        public static void InstantiatePresetGameModuleAsync<T>(Action onInstantiated) where T : GameModuleBase {
            InstantiatePresetGameModuleAsync(typeof(T), onInstantiated);
        }

        public static T GetModule<T>() where T : class {
            if (TryGetModule(out T module)) {
                return module;
            }

            Debug.LogErrorFormat("[GAME MANAGER] Component assignable to {0} does not exist", typeof(T).Name);
            return null;
        }

        public static bool TryGetModule<T>(out T module) where T : class {
            module = null;
            if (Instance == null) {
                return false;
            }

            var targetType = typeof(T);
            if (Instance.m_AliveGameModules.TryGetValue(targetType, out var exactModule)) {
                module = exactModule as T;
                return module != null;
            }

            foreach (var aliveModule in Instance.m_AliveGameModules.Values) {
                if (aliveModule is T typedModule) {
                    module = typedModule;
                    return true;
                }
            }

            return false;
        }

        public static void DestroyFramework() {
            if (Instance == null) {
                return;
            }

            foreach (var gameModuleType in Instance.m_AliveGameModuleExecutionOrderCache) {
                if (Instance.m_AliveGameModules.TryGetValue(gameModuleType, out var component)) {
                    component.OnDestroyedInternal();
                }
            }

            Instance.m_AliveGameModules.Clear();
            Instance.m_RegisterGameModulesQueue.Clear();
            Instance.m_AliveGameModuleUpdateTimers.Clear();
            Instance.m_AliveGameModuleFixedUpdateTimers.Clear();
            Instance.m_AliveGameModuleLateUpdateTimers.Clear();
            Instance.m_AliveGameModuleRegistrationOrders.Clear();
            Instance.m_AliveGameModuleExecutionOrder.Clear();
            Instance.m_AliveGameModuleExecutionOrderCache = Array.Empty<Type>();
            Instance.m_NextGameModuleRegistrationOrder = 0;
        }

        private void CachePresetModules(IEnumerable<GameModuleBase> modulePrefabs) {
            if (modulePrefabs == null) {
                return;
            }

            foreach (var modulePrefab in modulePrefabs) {
                if (modulePrefab == null) {
                    continue;
                }

                m_PresetGameModules[modulePrefab.GetType()] = modulePrefab;
            }
        }

        private void EnsureCoreModuleSelections() {
            var discoveredModules = DiscoverCoreModuleDefinitions();
            foreach (var contractGroup in discoveredModules.GroupBy(module => module.ContractTypeName)) {
                var selection = m_CoreModules.FirstOrDefault(module => module.contractTypeName == contractGroup.Key);
                if (selection == null) {
                    selection = new XiheCoreModuleSelection {
                        enabled = true,
                        contractTypeName = contractGroup.Key
                    };
                    m_CoreModules.Add(selection);
                }

                if (string.IsNullOrEmpty(selection.implementationTypeName) || ResolveType(selection.implementationTypeName) == null) {
                    selection.implementationTypeName = contractGroup.OrderBy(module => module.ImplementationTypeName).First().ImplementationTypeName;
                }
            }
        }

        private IEnumerable<GameModuleBase> CreateSelectedCoreModules() {
            foreach (var moduleSelection in m_CoreModules.Where(module => module.enabled)) {
                var moduleType = ResolveType(moduleSelection.implementationTypeName);
                if (moduleType == null) {
                    Debug.LogError($"[GAME MANAGER] Core module implementation type not found: {moduleSelection.implementationTypeName}");
                    continue;
                }

                if (!typeof(GameModuleBase).IsAssignableFrom(moduleType)) {
                    Debug.LogError($"[GAME MANAGER] Core module implementation is not a GameModuleBase: {moduleType.Name}");
                    continue;
                }

                if (m_AliveGameModules.ContainsKey(moduleType)) {
                    continue;
                }

                var moduleRoot = new GameObject(moduleType.Name);
                moduleRoot.transform.SetParent(m_GameModuleRoot, false);
                var module = moduleRoot.AddComponent(moduleType) as GameModuleBase;
                if (module == null) {
                    Debug.LogError($"[GAME MANAGER] Failed to create core module: {moduleType.Name}");
                    continue;
                }

                yield return module;
            }
        }

        private IEnumerable<GameModuleBase> InstantiatePresetModules() {
            foreach (var modulePrefab in m_PresetGameModules.Values) {
                if (modulePrefab == null) {
                    continue;
                }

                var moduleType = modulePrefab.GetType();
                if (m_AliveGameModules.ContainsKey(moduleType)) {
                    continue;
                }

                var module = Instantiate(modulePrefab, m_GameModuleRoot, true);
                if (module == null) {
                    Debug.LogError($"[GAME MANAGER] GameModule: {moduleType.Name} failed to instantiate");
                    continue;
                }

                yield return module;
            }
        }

        private void InstantiateStartupModules(IEnumerable<GameModuleBase> modules) {
            foreach (var gameModule in modules.Where(module => module != null).OrderBy(module => module.Priority)) {
                ApplyDebugSettings(gameModule);
                RegisterAliveGameModule(gameModule.GetType(), gameModule);
                gameModule.OnInstantiatedInternal(null);
            }
        }

        private IEnumerable<CoreModuleDefinition> DiscoverCoreModuleDefinitions() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetLoadableTypes)
                .Where(type => !type.IsAbstract)
                .Where(type => typeof(GameModuleBase).IsAssignableFrom(type))
                .Select(type => new { Type = type, Attribute = type.GetCustomAttribute<XiheCoreModuleAttribute>() })
                .Where(entry => entry.Attribute?.ContractType != null)
                .Select(entry => new CoreModuleDefinition(entry.Attribute.ContractType.AssemblyQualifiedName, entry.Type.AssemblyQualifiedName));
        }

        private static Type ResolveType(string typeName) {
            if (string.IsNullOrEmpty(typeName)) {
                return null;
            }

            var type = Type.GetType(typeName);
            if (type != null) {
                return type;
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetLoadableTypes)
                .FirstOrDefault(candidate => candidate.FullName == typeName || candidate.AssemblyQualifiedName == typeName);
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly) {
            try {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e) {
                return e.Types.Where(type => type != null);
            }
        }

        private void ProcessGameModuleRegistrationQueue() {
            while (m_RegisterGameModulesQueue.Count > 0) {
                var registrationInfo = m_RegisterGameModulesQueue.Dequeue();
                RegisterAliveGameModule(registrationInfo.GameModuleType, registrationInfo.GameModule);
                registrationInfo.GameModule.OnInstantiatedInternal(registrationInfo.OnInstantiated);
            }
        }

        private void RegisterAliveGameModule(Type gameModuleType, GameModuleBase gameModule) {
            m_AliveGameModules[gameModuleType] = gameModule;
            m_AliveGameModuleUpdateTimers[gameModuleType] = gameModule.updateInterval;
            m_AliveGameModuleFixedUpdateTimers[gameModuleType] = gameModule.fixedUpdateInterval;
            m_AliveGameModuleLateUpdateTimers[gameModuleType] = gameModule.lateUpdateInterval;

            if (!m_AliveGameModuleRegistrationOrders.ContainsKey(gameModuleType)) {
                m_AliveGameModuleRegistrationOrders[gameModuleType] = m_NextGameModuleRegistrationOrder;
                m_NextGameModuleRegistrationOrder += 1;
            }

            if (!m_AliveGameModuleExecutionOrder.Contains(gameModuleType)) {
                m_AliveGameModuleExecutionOrder.Add(gameModuleType);
            }

            SortAliveGameModulesByPriority();
        }

        private void SortAliveGameModulesByPriority() {
            m_AliveGameModuleExecutionOrder.Sort((left, right) => {
                var priorityComparison = m_AliveGameModules[left].Priority.CompareTo(m_AliveGameModules[right].Priority);
                if (priorityComparison != 0) {
                    return priorityComparison;
                }

                return m_AliveGameModuleRegistrationOrders[left].CompareTo(m_AliveGameModuleRegistrationOrders[right]);
            });
            m_AliveGameModuleExecutionOrderCache = m_AliveGameModuleExecutionOrder.ToArray();
        }

        private void ApplyDebugSettings(GameModuleBase gameModule) {
            gameModule.SetDebugEnabled(m_EnableFrameworkDebug);
        }

        private readonly struct CoreModuleDefinition {
            public readonly string ContractTypeName;
            public readonly string ImplementationTypeName;

            public CoreModuleDefinition(string contractTypeName, string implementationTypeName) {
                ContractTypeName = contractTypeName;
                ImplementationTypeName = implementationTypeName;
            }
        }

        private readonly struct GameModuleRegistrationInfo {
            public readonly Type GameModuleType;
            public readonly GameModuleBase GameModule;
            public readonly Action OnInstantiated;

            public GameModuleRegistrationInfo(Type gameModuleType, GameModuleBase gameModule, Action onInstantiated) {
                GameModuleType = gameModuleType;
                GameModule = gameModule;
                OnInstantiated = onInstantiated;
            }
        }
    }
}
