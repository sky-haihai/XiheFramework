using System;
using UnityEngine.SceneManagement;

namespace XiheFramework.Runtime.Scene {
    public interface IXiheSceneModule {
        void LoadScene(string sceneAddress, LoadSceneMode loadSceneMode, Action onSceneLoadComplete = null);
    }
}
