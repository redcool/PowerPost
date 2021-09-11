using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public class PowerPostFeature : ScriptableRendererFeature
    {

        [Serializable]
        public class Settings
        {
            public string[] currentSettingTypes;
        }
        public Settings settings;

        public PowerPostData assetData;

        Dictionary<IPostProcessingSetting, BasePostExPass> postPassDict = new Dictionary<IPostProcessingSetting, BasePostExPass>();

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!assetData)
                return;

            AddPostPassByTypes(renderer, assetData.GetSettingTypes());
        }

        public override void Create()
        {
            if (settings != null && assetData)
                settings.currentSettingTypes = assetData.SettingNames;
            
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            postPassDict.Clear();
        }

        void AddPostPassByTypes(ScriptableRenderer renderer,Type[] postSettingTypes)
        {
            foreach (var t in postSettingTypes)
            {
                if (t == null)
                    continue;

                var settings = VolumeManager.instance.stack.GetComponent(t) as IPostProcessingSetting;
                if (settings == null)
                    continue;

                BasePostExPass pass;
                if (!postPassDict.TryGetValue(settings,out pass))
                {
                    postPassDict[settings] = pass = settings.CreateNewInstance();
                    //pass.ConfigureTarget(renderer.cameraColorTarget, renderer.cameraDepthTarget);
                    pass.Renderer = renderer;
                    pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
                }
                renderer.EnqueuePass(pass);
            }
        }
    }


}
