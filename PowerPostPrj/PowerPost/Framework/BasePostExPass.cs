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

        public const string POWER_POST_DEFAULT_SHADER = "Hidden/PowerPost/Default";
        Material defaultMaterial;
        public Material DefaultMaterial
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

        Material material;
        public Material GetTargetMaterial(string shaderName)
        {
            if (!material)
                material = new Material(Shader.Find(shaderName));
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
            if (!settings.IsActive())
                return;

            OnExecute(context, ref renderingData, settings);
        }

        public abstract void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData,T settings);
    }

}
