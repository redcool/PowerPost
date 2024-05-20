namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;
    using Random = UnityEngine.Random;

    public class SSAOPass : BasePostExPass<SSAOSettings>
    {
        int _Intensity = Shader.PropertyToID(nameof(_Intensity));
        int _Radius = Shader.PropertyToID(nameof(_Radius));
        int _Downsample = Shader.PropertyToID(nameof(_Downsample));
        int _SampleCount = Shader.PropertyToID(nameof(_SampleCount));

        int _SSAOMask = Shader.PropertyToID(nameof(_SSAOMask));
        int _BlurTex = Shader.PropertyToID(nameof(_BlurTex));

        public override string PassName => nameof(SSAOPass);
        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SSAOSettings settings,CommandBuffer cmd)
        {
            var cam = renderingData.cameraData.camera;

            var mat = GetTargetMaterial("Hidden/PowerPost/Obscurance");
            
            mat.SetFloat(_Intensity,settings.intensity.value);
            mat.SetFloat(_Radius, settings.radius.value);
            mat.SetFloat(_Downsample, settings.downSample.value ? 0.5f : 1f);
            mat.SetInt(_SampleCount, settings.sampleCount.value);
            mat.SetColor("_AOColor", settings.aoColor.value);
            
            InitTextures(cmd, cam, settings);

            //// 0 calc ssao mask 
            cmd.BlitColorDepth(sourceTex, _SSAOMask, _SSAOMask, mat, 0);

            //// 1 h blur
            cmd.BlitColorDepth(_SSAOMask, _BlurTex, _BlurTex, mat, 3);
            //// 2 v blur
            cmd.BlitColorDepth(_BlurTex, _SSAOMask, _SSAOMask, mat, 5);

            //cmd.BlitColorDepth(_SSAOMask, targetTex, targetTex, DefaultBlitMaterial);
            //return;

            //// 3 composite
            cmd.BlitColorDepth(sourceTex, targetTex, targetTex, mat, 6);
        }

        void InitTextures(CommandBuffer cmd,Camera cam, SSAOSettings settings)
        {
            var w = cam.pixelWidth >> (settings.downSample.value ? 1 : 0);
            var h = cam.pixelHeight >> (settings.downSample.value ? 1 : 0);

            cmd.GetTemporaryRT(_SSAOMask, w, h, 0, FilterMode.Bilinear, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            cmd.GetTemporaryRT(_BlurTex, w, h, 0,FilterMode.Bilinear, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        }
    }
}