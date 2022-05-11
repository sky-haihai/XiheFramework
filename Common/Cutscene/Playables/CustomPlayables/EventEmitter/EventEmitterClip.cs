using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class EventEmitterClip : PlayableAsset, ITimelineClipAsset {
    public EventEmitterBehaviour template = new EventEmitterBehaviour();

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
        var playable = ScriptPlayable<EventEmitterBehaviour>.Create(graph, template);
        return playable;
    }
}