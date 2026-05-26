using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework.Runtime.Audio.Wwise {
    public class XiheWwiseAudioModule : XiheAudioModuleBase {
        public GameObject sharedAudioObject;

#if USE_WWISE
        private float m_MasterVolume = 50;

        private List<uint> m_PlayingEvents = new();

        #region Life Cycle

        protected override void OnInstantiated() {
            base.OnInstantiated();

        }


        protected override void OnDestroyed() {
            base.OnDestroyed();

            AkSoundEngine.StopAll();
        }

        #endregion

        #region Public Methods

        public void Play(AK.Wwise.Event audioEvent, GameObject container = null) {
            if (container == null) {
                container = sharedAudioObject;
            }

            var id = audioEvent.Post(container);
            m_PlayingEvents.Add(id);
        }

        public void Play(AK.Wwise.Event audioEvent, GameObject container = null, AkCallbackManager.EventCallback callback = null) {
            if (container == null) {
                container = sharedAudioObject;
            }

            var id = audioEvent.Post(container, (uint)AkCallbackType.AK_EndOfEvent, callback);
            m_PlayingEvents.Add(id);
        }

        public void Stop(uint playingId, int fadeOutTime = 500, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear) {
            AkSoundEngine.StopPlayingID(playingId, fadeOutTime, curveInterpolation);
        }

        public void SetMasterVolume(float volume) {
            volume = Mathf.Clamp(volume, 0, 100);
            m_MasterVolume = volume;
            AkSoundEngine.SetRTPCValue("MasterVolume", m_MasterVolume);
        }

        public void SetVariable(string variableName, float value, GameObject target) {
            AkSoundEngine.SetRTPCValue(variableName, value, target);
        }

        public void SetVariableGlobal(string variableName, float value, GameObject target) {
            AkSoundEngine.SetRTPCValue(variableName, value, target);
        }

        public void SetState(string stateGroupName, string state) {
            AkSoundEngine.SetState(stateGroupName, state);
        }

        #endregion
#endif
    }
}
