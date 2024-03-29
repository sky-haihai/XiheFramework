using System;
using UnityEngine;
using UnityEngine.Playables;

namespace XiheFramework.Utility.Playables.LightControl {
    [Serializable]
    public class LightControlBehaviour : PlayableBehaviour {
        public Color color = Color.white;
        public float intensity = 1f;
        public float bounceIntensity = 1f;
        public float range = 10f;
    }
}