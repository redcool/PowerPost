using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TestRenderObjects : ScriptableRendererFeature
{

    [Serializable]public class Settings
    {
        public LayerMask layer;
    }

    public Settings settings;
    TestRenderPass pass;


    public class TestRenderPass : ScriptableRenderPass
    {
        public Settings settings = new Settings();

        static List<ShaderTagId>tagIds = new List<ShaderTagId>{
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("UniversalForward"),
            new ShaderTagId("UniversalForwardOnly"),
            new ShaderTagId("LightweightForward"),
        };
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var sortingSettings = new SortingSettings(renderingData.cameraData.camera);
            sortingSettings.criteria = SortingCriteria.CommonOpaque;

            var drawSettings = new DrawingSettings();
            for (int i = 0; i < tagIds.Count; i++)
            {
                drawSettings.SetShaderPassName(i, tagIds[i]);
            }
            drawSettings.sortingSettings = sortingSettings;
            //drawSettings.mainLightIndex = renderingData.lightData.mainLightIndex;
            drawSettings.perObjectData = renderingData.perObjectData;

            //var drawSettings = CreateDrawingSettings(tagIds, ref renderingData, SortingCriteria.CommonOpaque);

            var filterSettings = new FilteringSettings(RenderQueueRange.all);
            filterSettings.layerMask = settings.layer;
            
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filterSettings);
        }
    }


    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }

    public override void Create()
    {
        pass = new TestRenderPass();
        pass.settings = settings;
        pass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }
}
