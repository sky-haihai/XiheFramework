using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace XiheFramework.Utility.Playables.Video {
    [Serializable]
    [TrackClipType(typeof(VideoScriptPlayableAsset))]
    [TrackColor(0.008f, 0.698f, 0.655f)]
    public class VideoScriptPlayableTrack : TrackAsset {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
            var playableDirector = go.GetComponent<PlayableDirector>();

            var playable =
                ScriptPlayable<VideoSchedulerPlayableBehaviour>.Create(graph, inputCount);

            var videoSchedulerPlayableBehaviour =
                playable.GetBehaviour();

            if (videoSchedulerPlayableBehaviour != null) {
                videoSchedulerPlayableBehaviour.director = playableDirector;
                videoSchedulerPlayableBehaviour.clips = GetClips();
            }

            return playable;
        }
    }
}