namespace PowerPost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;


    public delegate void RefAction<T>(ref T t);



    public static class GraphicsUtils
    {
        public static readonly ShaderTagId[] shaderTags = new[] {
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("UniversalForward"),
            new ShaderTagId("UniversalForwardOnly"),
        };

        public static void DrawRenderers(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd, ShaderTagId[] shaderTags, LayerMask filterLayer, RefAction<RenderStateBlock> onSetState)
        {
            var sortingSettings = new SortingSettings { criteria = SortingCriteria.RenderQueue };
            var drawSettings = new DrawingSettings { sortingSettings = sortingSettings };
            for (int i = 0; i < shaderTags.Length; i++)
            {
                drawSettings.SetShaderPassName(i, shaderTags[i]);
            }
            drawSettings.mainLightIndex = renderingData.lightData.mainLightIndex;
            drawSettings.perObjectData = renderingData.perObjectData;

            var filterSettings = new FilteringSettings(RenderQueueRange.all, filterLayer);
            var stateBlock = new RenderStateBlock();
            if (onSetState != null)
                onSetState(ref stateBlock);

            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filterSettings,ref stateBlock);
            context.ExecuteCommandBuffer(cmd);
        }

        public static void SetStencilState(ref RenderStateBlock stateBlock, int stencilRef, StencilState state)
        {
            stateBlock.mask |= RenderStateMask.Stencil;
            stateBlock.stencilReference = stencilRef;
            stateBlock.stencilState = state;
        }

        public static void SetDepthState(ref RenderStateBlock block,DepthState state)
        {
            block.mask |= RenderStateMask.Depth;
            block.depthState = state;
        }
    }
}
