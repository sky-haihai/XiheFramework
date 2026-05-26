using UnityEngine;

namespace XiheFramework.Runtime.Audio.UnityAudio {
    public class XiheUnityAudioModule : XiheAudioModuleBase {
        #region Life Cycle

        protected override void OnInstantiated() {
            base.OnInstantiated();

        }

        #endregion

        #region Public Methods

        public GameObject Play(AudioClip audioClip, GameObject follow = null, bool loop = false) {
            GameObject container;
            if (follow == null) {
                container = new GameObject("Unity Audio Player (Generated)");
                container.transform.position = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
            }
            else {
                container = follow;
            }

            var audioPlayerComponent = container.AddComponent<AudioPlayer>();
            audioPlayerComponent.AudioClip = audioClip;
            audioPlayerComponent.Play();

            return container;
        }

        #endregion
    }
}
