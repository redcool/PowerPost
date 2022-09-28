using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace PowerPost
{
    [ VolumeComponentMenu("PowerPostEx/RadialBlur")]
    public class RadialBlurSettings : BasePostExSettings
    {
        public Vector2Parameter center = new Vector2Parameter(new Vector2(.5f, .5f));
        public ClampedFloatParameter radiusMin = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter radiusMax = new ClampedFloatParameter(.1f, 0, 1);
        public ClampedFloatParameter blurSize = new ClampedFloatParameter(0,0,0.4f);
        public BoolParameter roundness = new BoolParameter(false);

        [Header("Radial Layer")]
        public BoolParameter radialTexOn = new BoolParameter(false);
        public TextureParameter radialTex = new TextureParameter(null);
        public Vector2Parameter radialScale = new Vector2Parameter(new Vector2(1,1));
        public MinFloatParameter radialLength = new MinFloatParameter(1,0);

        [Header("Radial Layer Noise")]
        public BoolParameter noiseMapOn = new BoolParameter(false);
        public TextureParameter noiseMap = new TextureParameter(null);
        public Vector4Parameter noiseMapST = new Vector4Parameter(new Vector4(1, 1, 0, 0));
        public ClampedFloatParameter noiseMapScale = new ClampedFloatParameter(1, 0, 1);

        [Header("Gray")]
        public BoolParameter isGrayScale = new BoolParameter(false);
        public MinFloatParameter grayScale = new MinFloatParameter(1,0);

        public override BasePostExPass CreateNewInstance()
        {
            return new RadialBlurPass();
        }

        public override bool IsActive()
        {
            return blurSize.value > 0;
        }
    }
}
