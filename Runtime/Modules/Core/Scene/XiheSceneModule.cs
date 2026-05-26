using System;
using System.Collections;
#if USE_ADDRESSABLE
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.Scene {
    [XiheCoreModule(typeof(IXiheSceneModule))]
    public class XiheSceneModule : GameModuleBase, IXiheSceneModule {
        public override int Priority => (int)CoreModulePriority.Scene;
        /// <summary>
        /// Load scene async using Addressable
        /// </summary>
        /// <param name="sceneAddress"></param>
        /// <param name="loadSceneMode"></param>
        /// <param name="onSceneLoadComplete"></param>
#if USE_ADDRESSABLE
        public void LoadSceneAsync(string sceneAddress, LoadSceneMode loadSceneMode, Action<AsyncOperationHandle<SceneInstance>> onSceneLoadComplete = null) {
            var handle = LoadSceneAddressable(sceneAddress, loadSceneMode);
            handle.Completed += operationHandle => {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded) {
                    var activateHandle = operationHandle.Result.ActivateAsync();
                    activateHandle.completed += op => onSceneLoadComplete?.Invoke(operationHandle);
                }
            };
        }
#endif

        public void LoadScene(string sceneAddress, LoadSceneMode loadSceneMode, Action onSceneLoadComplete = null) {
#if USE_ADDRESSABLE
            StartCoroutine(LoadSceneCo(sceneAddress, loadSceneMode, onSceneLoadComplete));
#else
            Debug.LogError("Please import Addressable Package and define USE_ADDRESSABLE in your project settings: Player->Other Settings->Scripting Define Symbols");
#endif
        }

#if USE_ADDRESSABLE
        private IEnumerator LoadSceneCo(string address, LoadSceneMode loadSceneMode, Action onSceneLoadComplete) {
            var handle = LoadSceneAddressable(address, loadSceneMode);
            var sceneInstance = handle.WaitForCompletion();
            yield return sceneInstance.ActivateAsync();

            onSceneLoadComplete?.Invoke();
        }

        private static AsyncOperationHandle<SceneInstance> LoadSceneAddressable(string address, LoadSceneMode loadSceneMode) {
#if UNITY_6000_0_OR_NEWER
            return Addressables.LoadSceneAsync(address, loadSceneMode, false);
#else
            return Addressables.LoadSceneAsync(address, loadSceneMode);
#endif
        }
#endif

        protected override void OnInstantiated() {
        }
    }
}
