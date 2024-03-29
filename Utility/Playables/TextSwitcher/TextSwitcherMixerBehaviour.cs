using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using XiheFramework.Entry;

namespace XiheFramework.Utility.Playables.TextSwitcher {
    public class TextSwitcherMixerBehaviour : PlayableBehaviour {
        private Color m_DefaultColor;
        private int m_DefaultFontSize;
        private string m_DefaultText;
        private bool m_FirstFrameHappened;

        private Text m_TrackBinding;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
            m_TrackBinding = playerData as Text;

            if (m_TrackBinding == null)
                return;

            if (!m_FirstFrameHappened) {
                m_DefaultColor = m_TrackBinding.color;
                m_DefaultFontSize = m_TrackBinding.fontSize;
                m_DefaultText = m_TrackBinding.text;
                m_FirstFrameHappened = true;
            }

            var inputCount = playable.GetInputCount();

            var totalWeight = 0f;

            for (var i = 0; i < inputCount; i++) {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<TextSwitcherBehaviour>)playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();

                totalWeight += inputWeight;

                if (!Mathf.Approximately(inputWeight, 1f)) continue;

                m_TrackBinding.color = input.color;
                m_TrackBinding.fontSize = input.fontSize;

                if (!input.localized) {
                    m_TrackBinding.text = GetText(input.text, input.progression, input.speed);
                }
                else {
                    var translated = Game.Localization.GetValue(input.text);
                    m_TrackBinding.text = GetText(translated, input.progression, input.speed);
                }
            }

            if (!Mathf.Approximately(totalWeight, 1f)) m_TrackBinding.text = m_DefaultText;
        }

        /// <summary>
        ///     cut text
        /// </summary>
        /// <param name="original"> full text </param>
        /// <param name="progression"> 0-1 </param>
        /// <param name="speed"> 1 refers to full duration, 2 refer to half duration </param>
        /// <returns></returns>
        private string GetText(string original, float progression, float speed) {
            var length = Mathf.FloorToInt(original.Length * progression * speed);
            length = Mathf.Clamp(length, 0, original.Length);
            var result = original.Substring(0, length);
            return result;
        }

        public override void OnPlayableDestroy(Playable playable) {
            m_FirstFrameHappened = false;

            if (m_TrackBinding == null)
                return;

            m_TrackBinding.color = m_DefaultColor;
            m_TrackBinding.fontSize = m_DefaultFontSize;
            m_TrackBinding.text = m_DefaultText;
        }
    }
}