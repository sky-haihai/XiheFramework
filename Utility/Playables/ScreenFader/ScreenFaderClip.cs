using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace XiheFramework.Utility.Playables.ScreenFader {
    [Serializable]
    public class ScreenFaderClip : PlayableAsset, ITimelineClipAsset {
        public ScreenFaderBehaviour template = new();

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var playable = ScriptPlayable<ScreenFaderBehaviour>.Create(graph, template);
            return playable;
        }
    }
}