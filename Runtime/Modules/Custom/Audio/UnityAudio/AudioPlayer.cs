using UnityEngine;

namespace XiheFramework.Runtime.Audio.UnityAudio {
    public class AudioPlayer : MonoBehaviour {
        public AudioClip AudioClip { get; set; }
        public bool Loop { get; set; }

        private AudioSource m_AudioSource;
        private bool m_Played;
        private float m_Timer;

        public void Play() {
            if (AudioClip) {
                m_AudioSource.clip = AudioClip;
                m_AudioSource.loop = Loop;
                m_AudioSource.Play();
            }

            m_Played = true;
        }

        private void Start() {
            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_AudioSource.playOnAwake = false;
            m_Played = false;
        }

        private void Update() {
            if (m_Played) {
                if (m_Timer >= AudioClip.length) {
                    if (!m_AudioSource.isPlaying) {
                        Destroy(gameObject);
                    }
                }

                m_Timer += Time.unscaledDeltaTime;
            }
        }
    }
}