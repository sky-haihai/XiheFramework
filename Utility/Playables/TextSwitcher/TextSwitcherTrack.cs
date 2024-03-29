using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace XiheFramework.Utility.Playables.TextSwitcher {
    [TrackColor(0.1394896f, 0.4411765f, 0.3413077f)]
    [TrackClipType(typeof(TextSwitcherClip))]
    [TrackBindingType(typeof(Text))]
    public class TextSwitcherTrack : TrackAsset {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
            return ScriptPlayable<TextSwitcherMixerBehaviour>.Create(graph, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver) {
#if UNITY_EDITOR
            var trackBinding = director.GetGenericBinding(this) as Text;
            if (trackBinding == null)
                return;

            var serializedObject = new SerializedObject(trackBinding);
            var iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true)) {
                if (iterator.hasVisibleChildren)
                    continue;

                driver.AddFromName<Text>(trackBinding.gameObject, iterator.propertyPath);
            }
#endif
            base.GatherProperties(director, driver);
        }
    }
}