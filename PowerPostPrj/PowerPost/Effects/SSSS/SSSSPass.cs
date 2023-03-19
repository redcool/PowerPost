using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost {
    public class SSSSPass : BasePostExPass<SSSSSettings>
    {
        const string DIFFUSE_PROFILE_SHADER = "Hidden/PowerPost/ScreenDiffuseProfile";

        int _Kernel = Shader.PropertyToID("_Kernel");
        int _BlurSize = Shader.PropertyToID("_BlurSize");
        int _StencilRef = Shader.PropertyToID("_StencilRef");

        List<Vector4> kernels = new List<Vector4>();
        Color lastStrength, lastFalloff;

        void CalcSSSSKernel(SSSSSettings settings, List<Vector4> kernels)
        {
            var needUpdate = lastStrength != settings.strength.value || lastFalloff != settings.falloff.value;
            if (!needUpdate)
                return;

            lastStrength = settings.strength.value;
            lastFalloff = settings.falloff.value;
            SSSSKernel.CalculateKernel(kernels, 25, lastStrength, lastFalloff);
        }

        public override string PassName => nameof(SSSSPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SSSSSettings settings, CommandBuffer cmd)
        {
            //update kernel 
            CalcSSSSKernel(settings, kernels);

            var layer = settings.layer.value;
            int stencilRef = settings.stencilRef.value;
            if (layer != 0)
            {
                GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, layer, (ref RenderStateBlock block) =>
                {
                    GraphicsUtils.SetStencilState(ref block, stencilRef, new StencilState(passOperation: StencilOp.Replace));
                });
            }

            // update material
            var mat = GetTargetMaterial(DIFFUSE_PROFILE_SHADER);
            mat.SetVectorArray(_Kernel, kernels);
            mat.SetFloat(_BlurSize, settings.blurScale.value);
            mat.SetInt(_StencilRef, layer != 0 ? stencilRef : 0);


            cmd.BlitColorDepth(sourceTex, targetTex, targetTex, DefaultBlitMaterial);
            cmd.BlitColorDepth(sourceTex, targetTex, ShaderPropertyIds._CameraDepthAttachment, mat, 0);
        }

    }
}