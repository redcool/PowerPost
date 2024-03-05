namespace PowerPost {
    using PowerUtilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class OutlineExtendPass : BasePostExPass<OutlineExtendSettings>
    {
        private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");
        private static readonly int DepthEdgeWidth = Shader.PropertyToID("_DepthEdgeWidth");
        private static readonly int NormalEdgeWidth = Shader.PropertyToID("_NormalEdgeWidth");
        private static readonly int ColorEdgeWidth = Shader.PropertyToID("_ColorEdgeWidth");
        private static readonly int DepthThresholdMin = Shader.PropertyToID("_DepthThresholdMin");
        private static readonly int DepthThresholdMax = Shader.PropertyToID("_DepthThresholdMax");
        private static readonly int NormalThresholdMin = Shader.PropertyToID("_NormalThresholdMin");
        private static readonly int NormalThresholdMax = Shader.PropertyToID("_NormalThresholdMax");
        private static readonly int ColorThresholdMin = Shader.PropertyToID("_ColorThresholdMin");
        private static readonly int ColorThresholdMax = Shader.PropertyToID("_ColorThresholdMax");
        private static readonly int DistInfluence = Shader.PropertyToID("_DistInfluence");
        private static readonly int DistInfluenceSmooth = Shader.PropertyToID("_DistInfluenceSmooth");
        public override string PassName => nameof(OutlineExtendPass);

        public  static int tempTex = Shader.PropertyToID(nameof(tempTex));

        private RenderTargetHandle tempRT;

        private Material outlineMat;
        private OutlineExtendSettings settings;

        

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, OutlineExtendSettings settings, CommandBuffer cmd)
        {
            this.settings = settings;
            outlineMat = GetTargetMaterial("Hidden/PowerPost/ExtendOutline");
            var cam = renderingData.cameraData.camera;
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            SetMaterialProperties();

            cmd.BlitColorDepth(ShaderPropertyIds._CameraColorAttachmentA,ShaderPropertyIds._CameraOpaqueTexture,ShaderPropertyIds._CameraDepthAttachment,outlineMat);

            //cmd.BlitColor(ShaderPropertyIds._CameraColorAttachmentA,ShaderPropertyIds._CameraOpaqueTexture,outlineMat,0);
            cmd.BlitColorDepth(ShaderPropertyIds._CameraOpaqueTexture,ShaderPropertyIds._CameraColorAttachmentA,ShaderPropertyIds._CameraDepthAttachment,DefaultBlitMaterial);
        }

        private void SetMaterialProperties()
        {
            if (outlineMat == null)
            {
                return;
            }

            const string depthKeyword = "OUTLINE_USE_DEPTH";
            if (settings.useDepth.value)
            {
                outlineMat.EnableKeyword(depthKeyword);
            }
            else
            {
                outlineMat.DisableKeyword(depthKeyword);
            }

            const string normalsKeyword = "OUTLINE_USE_NORMALS";
            if (settings.useNormals.value)
            {
                outlineMat.EnableKeyword(normalsKeyword);
            }
            else
            {
                outlineMat.DisableKeyword(normalsKeyword);
            }

            const string colorKeyword = "OUTLINE_USE_COLOR";
            if (settings.useColor.value)
            {
                outlineMat.EnableKeyword(colorKeyword);
            }
            else
            {
                outlineMat.DisableKeyword(colorKeyword);
            }

            const string outlineOnlyKeyword = "OUTLINE_ONLY";
            if (settings.outlineOnly.value)
            {
                outlineMat.EnableKeyword(outlineOnlyKeyword);
            }
            else
            {
                outlineMat.DisableKeyword(outlineOnlyKeyword);
            }

            // const string resolutionInvariantKeyword = "RESOLUTION_INVARIANT_THICKNESS";
            // if (settings.resolutionInvariant.value)
            // {
            //     outlineMat.EnableKeyword(resolutionInvariantKeyword);
            // }
            // else
            // {
            //     outlineMat.DisableKeyword(resolutionInvariantKeyword);
            // }

            outlineMat.SetColor(EdgeColor, settings.outlineColor.value);
            outlineMat.SetFloat(Thickness,settings.thickness.value);
            outlineMat.SetFloat(DepthEdgeWidth,settings.depthEdgeWidth .value);
            outlineMat.SetFloat(NormalEdgeWidth,settings.normalEdgeWidth.value);
            outlineMat.SetFloat(ColorEdgeWidth,settings.colorEdgeWidth.value);

            // outlineMat.SetFloat(DepthThresholdMin, settings.depthThreshold.value.x);
            // outlineMat.SetFloat(DepthThresholdMax, settings.depthThreshold.value.y);

            // outlineMat.SetFloat(NormalThresholdMin, settings.normalThreshold.value.x);
            // outlineMat.SetFloat(NormalThresholdMax, settings.normalThreshold.value.y);

            // outlineMat.SetFloat(ColorThresholdMin, settings.colorThreshold.value.x);
            // outlineMat.SetFloat(ColorThresholdMax, settings.colorThreshold.value.y);

            outlineMat.SetFloat(DepthThresholdMin, 0f);
            outlineMat.SetFloat(DepthThresholdMax, 0.25f);

            outlineMat.SetFloat(NormalThresholdMin, 0f);
            outlineMat.SetFloat(NormalThresholdMax, 0.25f);

            outlineMat.SetFloat(ColorThresholdMin, 0f);
            outlineMat.SetFloat(ColorThresholdMax, 0.25f);

            outlineMat.SetFloat(DistInfluence,1 - settings.distInfluence.value);
            outlineMat.SetFloat(DistInfluenceSmooth,settings.distInfluenceSmooth.value);

        }
    }
}