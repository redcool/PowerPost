using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class CommandBufferEx 
{
    public static readonly int _MAIN_TEX_ID = Shader.PropertyToID("_MainTex");
    static Mesh fullscreenQuad;
    public static Mesh FullscreenQuad
    {
        get
        {
            if (fullscreenQuad)
                return fullscreenQuad;

            fullscreenQuad = new Mesh();
            fullscreenQuad.vertices = new Vector3[] {
                new Vector3(-1,-1),
                new Vector3(-1,1),
                new Vector3(1,1),
                new Vector3(1,-1),
            };
            fullscreenQuad.uv = new Vector2[] {
                new Vector2(0,1),
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1),
            };
            fullscreenQuad.SetIndices(new[] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);
            return fullscreenQuad;
        }
    }

    public static void BlitColorDepth(this CommandBuffer buf,RenderTargetIdentifier source,RenderTargetIdentifier colorBuffer,RenderTargetIdentifier depthBuffer,Material mat,int pass)
    {
        buf.SetGlobalTexture(_MAIN_TEX_ID, source);
        buf.SetRenderTarget(colorBuffer, depthBuffer);
        buf.DrawMesh(FullscreenQuad, Matrix4x4.identity, mat, 0, pass);
    }

}
