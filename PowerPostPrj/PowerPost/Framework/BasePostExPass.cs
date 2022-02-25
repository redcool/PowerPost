using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public abstract class BasePostExPass : ScriptableRenderPass
    {
        public ScriptableRenderer Renderer { set; get; }

        public const string POWER_POST_DEFAULT_SHADER = "Hidden/PowerPost/DefaultBlit";
        Material defaultMaterial;
        Material material;
        public Material DefaultMaterialBlit
        {
            get
            {
                if (!defaultMaterial)
                {
                    defaultMaterial = new Material(Shader.Find(POWER_POST_DEFAULT_SHADER));
                }
                return defaultMaterial;
            }
        }


        public BasePostExPass()
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public Material GetTargetMaterial(string shaderName)
        {
            if (!material)
                material = new Material(Shader.Find(shaderName));
            if (!material)
                throw new ArgumentException($"{shaderName} not found!");
            return material;
        }

        public T GetSettings<T>() where T :BasePostExSettings
        {
            return VolumeManager.instance.stack.GetComponent<T>();
        }

        public RenderTargetIdentifier DepthTarget
        {
            get
            {
                var urpAsset = UniversalRenderPipeline.asset;
                return urpAsset.supportsCameraDepthTexture ? Renderer.cameraDepthTarget : Renderer.cameraColorTarget;
            }
        }

        public RenderTargetIdentifier ColorTarget
        {
            get { return Renderer.cameraColorTarget; }
        }

        public void BlitToColorBuffer(CommandBuffer cmd,RenderTargetIdentifier id)
        {
            cmd.BlitColorDepth(id, ColorTarget, DepthTarget, DefaultMaterialBlit, 0);
        }
    }

    public abstract class BasePostExPass<T> : BasePostExPass where T : BasePostExSettings
    {
        public T GetSettings()
        {
            return GetSettings<T>();
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var settings = GetSettings();
            if (settings == null || !settings.IsActive())
                return;

            OnExecute(context, ref renderingData, settings);
        }

        public abstract void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData,T settings);
    }

}
