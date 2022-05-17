using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace PowerPost
{
    public static class CommandBufferEx
    {
        public static readonly int _MAIN_TEX_ID = Shader.PropertyToID("_MainTex");
        static Mesh fullscreenQuad;
        static Mesh fullscreenTriangle;
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

        public static Mesh FullscreenTriangle
        {
            get{ 
                if(fullscreenTriangle)
                    return fullscreenTriangle;

                fullscreenTriangle = new Mesh();
                fullscreenTriangle.vertices = new Vector3[] {
                    //new Vector3(-1,-1),
                    //new Vector3(3,-1),
                    //new Vector3(-1,3)
                    Vector3.zero,Vector3.zero,Vector3.zero
                };
                fullscreenTriangle.uv = new Vector2[3]
                {
                    //new Vector2(0,0),
                    //new Vector2(2,0),
                    //new Vector2(0,2)
                    Vector2.zero,Vector2.zero,Vector2.zero
                };
                fullscreenTriangle.SetIndices(new[] { 0,1,2}, MeshTopology.Triangles, 0);
                return fullscreenTriangle;
            }
        }

        public static void BlitColorDepth(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier colorBuffer, RenderTargetIdentifier depthBuffer, Material mat, int pass)
        {
            cmd.SetGlobalTexture(_MAIN_TEX_ID, source);
            cmd.SetRenderTarget(colorBuffer, depthBuffer);
            cmd.DrawMesh(FullscreenQuad, Matrix4x4.identity, mat, 0, pass);

            //buf.DrawMesh(FullscreenTriangle, Matrix4x4.identity, mat, 0, pass);
            //buf.DrawProcedural(Matrix4x4.identity, mat, 0, MeshTopology.Triangles, 3);
        }

    }

}