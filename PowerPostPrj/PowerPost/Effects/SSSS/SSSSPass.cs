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

        int _SceneColorRT = Shader.PropertyToID("_SceneColorRT");
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

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SSSSSettings settings)
        {
            //update kernel 
            CalcSSSSKernel(settings, kernels);

            // command
            var cmd = CommandBufferUtils.Get(context, nameof(SSSSPass));

            var layer = settings.layer.value;
            if(layer != 0)
            {
                int stencilRef = settings.stencilRef.value;
                GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, layer, (ref RenderStateBlock block) => {
                    GraphicsUtils.SetStencilState(ref block, stencilRef, new StencilState(passOperation: StencilOp.Replace));
                });
            }

            // update material
            var mat = GetTargetMaterial(DIFFUSE_PROFILE_SHADER);
            mat.SetVectorArray(_Kernel,kernels);
            mat.SetFloat(_BlurSize,settings.blurScale.value);
            mat.SetInt(_StencilRef, settings.stencilRef.value);

            cmd.BlitColorDepth(ColorTarget, _SceneColorRT, DepthTarget, mat, 0);
            cmd.BlitColorDepth(_SceneColorRT, ColorTarget, DepthTarget, mat, 1);
            context.ExecuteCommandBuffer(cmd);

            CommandBufferUtils.Release(cmd);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(_SceneColorRT, cameraTextureDescriptor);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_SceneColorRT);
        }
    }
}