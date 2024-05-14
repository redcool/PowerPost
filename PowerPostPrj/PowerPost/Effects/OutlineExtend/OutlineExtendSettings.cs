namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/OutlineExtend")]
    public class OutlineExtendSettings : BasePostExSettings
    {
        [Header("Base Settings")]
        public BoolParameter outlineToggle = new BoolParameter(false);
        //public ClampedIntParameter downSamples = new ClampedIntParameter(0, 0, 4);
        //public LayerMaskParameter layer = new LayerMaskParameter(0);
        public ColorParameter outlineColor = new ColorParameter(Color.black);
        //public ClampedIntParameter outlineWidth = new ClampedIntParameter(1, 0, 5);
        public ClampedIntParameter thickness = new ClampedIntParameter(1, 0, 5);
        

        //[Range(0, 5)]
        //public int thickness = 1;

        [Tooltip("If enabled, the line width will stay constant regardless of the rendering resolution. " +
                 "However, some of the lines may appear blurry.")]
        //public bool resolutionInvariant = false;
        //public BoolParameter resolutionInvariant = new BoolParameter(false);

        [Space]
        public BoolParameter useDepth = new BoolParameter(true);
        public BoolParameter useNormals = new BoolParameter(true);
        public BoolParameter useColor = new BoolParameter(true);

        //public bool useDepth = true;
        //public bool useNormals = false;
        //public bool useColor = false;

        [Header("Advanced Settings")]
        //public FloatRangeParameter depthThreshold = new FloatRangeParameter(new Vector2(0f,0.25f), 0f, 0.25f);
        public ClampedFloatParameter depthEdgeWidth = new ClampedFloatParameter(1, 0, 20f);
        //public float minDepthThreshold = 0f;
        //public float maxDepthThreshold = 0.25f;

        //public FloatRangeParameter normalThreshold = new FloatRangeParameter(new Vector2(0f,0.25f), 0f, 0.25f);
        public ClampedFloatParameter normalEdgeWidth = new ClampedFloatParameter(1, 0, 1);
        //public float minNormalsThreshold = 0f;
        //public float maxNormalsThreshold = 0.25f;
        //public FloatRangeParameter colorThreshold = new FloatRangeParameter(new Vector2(0f,0.25f), 0f, 0.25f);
        public ClampedFloatParameter colorEdgeWidth = new ClampedFloatParameter(1, 0, 1);
        [Space]
        public ClampedFloatParameter distInfluence = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter distInfluenceSmooth = new ClampedFloatParameter(0f, 0f, 1f);
        //public float minColorThreshold = 0f;
        //public float maxColorThreshold = 0.25f;
        [Space]
        public BoolParameter outlineOnly = new BoolParameter(false);

        [Tooltip("Whether the effect should be applied in the Scene view as well as in the Game view. Please keep in " +
                 "mind that Unity always renders the scene view with the default Renderer settings of the URP config.")]
        public BoolParameter applyInSceneView = new BoolParameter(true);
        

        //public bool applyInSceneView = true;
        public override bool NeedWriteToTarget() => false;
        public override BasePostExPass CreateNewInstance()
        {
            var pass = new OutlineExtendPass();
            
            pass.renderPassEvent = UnityEngine.Rendering.Universal.RenderPassEvent.BeforeRenderingTransparents - 11;
            return pass;
        }

        public override bool IsActive()
        {
            //return true;
            return outlineToggle.value;
            //return active ;
        }
        public override int Order => int.MaxValue -3;
    }
}