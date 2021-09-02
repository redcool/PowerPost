using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace PowerPost
{

    [ExecuteInEditMode]
    public class SSSSCommand : MonoBehaviour
    {
        public Color strength = Color.red, falloff = Color.green;
        public Material mat;

        CommandBuffer buf;

        int sceneColorRTId = Shader.PropertyToID("_SceneColorRT");
        Camera cam;

        private void OnEnable()
        {
            buf = new CommandBuffer { name = "ssss command" };
            cam = GetComponent<Camera>();
            cam.AddCommandBuffer(CameraEvent.AfterForwardOpaque, buf);
        }
        private void OnDisable()
        {
            if (buf == null)
                return;
            buf.ReleaseTemporaryRT(sceneColorRTId);
            cam.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, buf);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCommand();
        }


        private void UpdateCommand()
        {
            var kernels = new List<Vector4>();
            SSSSKernel.CalculateKernel(kernels, 25, new Vector3(strength.r, strength.g, strength.b), new Vector3(falloff.r, falloff.g, falloff.b));
            mat.SetVectorArray("_Kernel", kernels);

            buf.Clear();
            buf.GetTemporaryRT(sceneColorRTId, Screen.width, Screen.height);

            buf.BlitColorDepth(BuiltinRenderTextureType.CameraTarget, sceneColorRTId, BuiltinRenderTextureType.CameraTarget, mat, 0);

            //buf.SetGlobalTexture("_MainTex",BuiltinRenderTextureType.CameraTarget);
            //buf.SetRenderTarget(sceneColorRTId, BuiltinRenderTextureType.CameraTarget);
            //buf.DrawMesh(CommandBufferEx.FullscreenQuad, Matrix4x4.identity, mat, 0, 0);

            buf.BlitColorDepth(sceneColorRTId, BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget, mat, 1);
            //buf.SetGlobalTexture("_MainTex",sceneColorRTId);
            //buf.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            //buf.DrawMesh(CommandBufferEx.FullscreenQuad, Matrix4x4.identity, mat, 0, 1);
        }
    }
}