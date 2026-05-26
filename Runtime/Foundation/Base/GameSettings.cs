using System;
using UnityEngine;

namespace XiheFramework.Runtime.Base {
    [Serializable]
    public struct GameSettings {
        public int maxFramerate;
        public FullScreenMode fullscreenMode;
        public Resolution resolution;

        public void Apply() {
            Application.targetFrameRate = maxFramerate;
            Screen.fullScreenMode = fullscreenMode;
            Screen.SetResolution(resolution.width, resolution.height, fullscreenMode);
        }
    }
}