using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace XiheFramework.Utility.Playables.NavMeshAgentControl {
    [Serializable]
    public class NavMeshAgentControlClip : PlayableAsset, ITimelineClipAsset {
        public ExposedReference<Transform> destination;

        [HideInInspector] public NavMeshAgentControlBehaviour template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var playable = ScriptPlayable<NavMeshAgentControlBehaviour>.Create(graph, template);
            var clone = playable.GetBehaviour();
            clone.destination = destination.Resolve(graph.GetResolver());
            return playable;
        }
    }
}