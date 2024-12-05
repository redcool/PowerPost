using PowerUtilities.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;

namespace PowerUtilities.Timeline
{
    //[TrackBindingType(typeof(VolumeProfile))]
    [TrackClipType(typeof(PowerPostVolumeControlClip))]
    public class PowerPostVolumeControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<PowerPostVolumeControlMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
