using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine;

namespace PowerUtilities.Timeline
{
    [Serializable]
    public class VolumeControlClipPowerPost: PlayableAsset//VolumeControlClip
    {
        public  PowerPostVolumeControlBehaviour template;
        string guid;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var sp = ScriptPlayable<PowerPostVolumeControlBehaviour>.Create(graph, template);
            var b = sp.GetBehaviour();
            b.TrySetup(owner, sp, VolumeControlClip.GetGUID(guid));

            //instance = b;

            //template.TrySetup(owner);
            return sp;
        }
    }
}
