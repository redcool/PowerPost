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
        [Tooltip("径向模糊")]
        [Header("Radial Blur Layer")]
        public Vector2Parameter center = new Vector2Parameter(new Vector2(.5f, .5f));
        public ClampedFloatParameter radiusMin = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter radiusMax = new ClampedFloatParameter(.1f, 0, 1);
        public ClampedFloatParameter blurSize = new ClampedFloatParameter(0,0,0.4f);
        public BoolParameter roundness = new BoolParameter(false);

        [Tooltip("开启径向效果")]
        [Header("Radial Layer")]
        public BoolParameter radialTexOn = new BoolParameter(false);
        public TextureParameter radialTex = new TextureParameter(null);
        public Vector2Parameter radialScale = new Vector2Parameter(new Vector2(1,1));
        public MinFloatParameter radialLength = new MinFloatParameter(1,0);

        [Tooltip("径向层的扰动效果")]
        [Header("Radial Layer Noise")]
        public BoolParameter noiseMapOn = new BoolParameter(false);
        public TextureParameter noiseMap = new TextureParameter(null);
        public Vector4Parameter noiseMapST = new Vector4Parameter(new Vector4(1, 1, 0, 0));
        public ClampedFloatParameter noiseMapScale = new ClampedFloatParameter(1, 0, 1);

        [Tooltip("径向层的溶解")]
        [Header("Radial Layer Attenuation")]
        public BoolParameter attenMapOn = new BoolParameter(false);
        public TextureParameter attenMap = new TextureParameter(null);
        public Vector4Parameter attenMapST = new Vector4Parameter(new Vector4(1, 1, 0, 0));
        public ClampedFloatParameter dissolveRate = new ClampedFloatParameter(0, 0, 1);
        public BoolParameter clipOn = new BoolParameter(false);

        [Tooltip("灰度显示")]
        [Header("Gray")]
        public BoolParameter isGrayScale = new BoolParameter(false);
        public MinFloatParameter grayScale = new MinFloatParameter(1,0);
        public ColorParameter minColor = new ColorParameter(Color.black);
        public ColorParameter maxColor = new ColorParameter(Color.white);

        [Tooltip("开启底纹效果")]
        [Header("Base Line")]
        public BoolParameter isBaseLineOn = new BoolParameter(false);
        public TextureParameter baseLineMap = new TextureParameter(null);
        public Vector4Parameter baseLineMap_ST = new Vector4Parameter(new Vector4(1,1,0,0));
        public ClampedFloatParameter rotateRate = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter baseLineMapIntensity = new ClampedFloatParameter(0.5f, 0, 1);

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
