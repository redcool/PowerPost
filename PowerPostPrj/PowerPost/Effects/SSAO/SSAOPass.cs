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
        int _DepthTex = Shader.PropertyToID("_DepthTex");
        int _ColorTex = Shader.PropertyToID("_ColorTex");
        int _BlurColorTex = Shader.PropertyToID("_BlurColorTex");

        const int MAX_KERNEL_SIZE = 64;
        Vector4[] kernels;
        Vector4[] Kernels
        {
            get
            {
                if (kernels == null)
                {
                    kernels = new Vector4[MAX_KERNEL_SIZE];
                    for (int i = 0; i < MAX_KERNEL_SIZE; i++)
                    {
                        kernels[i] = Random.insideUnitSphere;
                    }
                }
                return kernels;
            }
        }

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SSAOSettings settings)
        {
            var cam = renderingData.cameraData.camera;

            var mat = GetTargetMaterial("Hidden/PowerPost/SSAO");
            //mat.SetFloat("_TanHalfFovY", Mathf.Tan(Mathf.Deg2Rad * cam.fieldOfView / 2));
            //mat.SetFloat("_Aspect", cam.aspect);
            //mat.SetFloat("_SampleRadius", settings.sampleRadius.value);
            //mat.SetVectorArray("_Kernel", Kernels);
            //mat.SetMatrix("_ProjMat", GL.GetGPUProjectionMatrix(cam.projectionMatrix, false));
            //mat.SetTexture("_NoiseMap", settings.noiseTex.value);

            //(intensity,radius,downsample,sampleCount)
            mat.SetVector("_SSAOParams",new Vector4(settings.intensity.value,
                settings.sampleRadius.value,
                settings.downSamples.value,
                settings.sampleCount.value
            ));

            var cmd = CommandBufferUtils.Get(context, nameof(SSAOPass));



            SetupTextures(cmd, cam, settings);

            //cmd.BlitColorDepth(Renderer.cameraDepthTarget, _DepthTex, _DepthTex, DefaultMaterialBlit, 0);
            //cmd.SetGlobalTexture(_BlurColorTex, _BlurColorTex);
            //cmd.SetGlobalTexture(_DepthTex, _DepthTex);


            cmd.BlitColorDepth(ColorTarget, _ColorTex, _ColorTex, mat, 0);
            cmd.BlitColorDepth(_ColorTex, ColorTarget, DepthTarget, DefaultMaterialBlit, 0);

            context.ExecuteCommandBuffer(cmd);
            ReleaseTextures(cmd);
            CommandBufferUtils.Release(cmd);
        }

        private void ReleaseTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_DepthTex);
            cmd.ReleaseTemporaryRT(_ColorTex);
        }

        private void SetupTextures(CommandBuffer cmd,Camera cam, SSAOSettings settings)
        {
            var w = cam.pixelWidth >> settings.downSamples.value;
            var h = cam.pixelHeight >> settings.downSamples.value;
            //cmd.GetTemporaryRT(_DepthTex, w, h,0,FilterMode.Bilinear,RenderTextureFormat.R16);
            cmd.GetTemporaryRT(_BlurColorTex, w, h, 16, FilterMode.Bilinear, RenderTextureFormat.Default);

            cmd.GetTemporaryRT(_ColorTex, cam.pixelWidth, cam.pixelHeight, 0, FilterMode.Bilinear);
        }
    }
}