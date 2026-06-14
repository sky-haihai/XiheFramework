using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XiheFramework.Runtime.Resource {
    public interface IXiheResourceModule {
        T InstantiateAsset<T>(string address) where T : Object;
        T InstantiateAsset<T>(string address, Vector3 localPosition, Quaternion localRotation) where T : Object;
        T LoadAsset<T>(string address) where T : Object;
        void InstantiateAssetAsync<T>(string address, Vector3 localPosition, Quaternion localRotation, Action<T> onInstantiated) where T : Object;
        void InstantiateAssetAsync<T>(string address, Vector3 localPosition, Action<T> onInstantiated) where T : Object;
        void InstantiateAssetAsync<T>(string address, Action<T> onInstantiated) where T : Object;
        void LoadAssetAsync<T>(string address, Action<T> onLoaded) where T : Object;
        IEnumerator LoadAssetAsyncCoroutine<T>(string address, Action<T> onLoaded) where T : Object;
        IEnumerator LoadAssetsAsyncCoroutine(IEnumerable<string> labels, Action<float> onProgress = null, Action<string> onLoaded = null, Action<IEnumerable<Object>> onFinished = null);
    }
}
