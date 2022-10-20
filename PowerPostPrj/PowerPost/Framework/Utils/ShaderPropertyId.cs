using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerPost
{
    public static class ShaderPropertyIds
    {
        public static readonly int
            _MainTex = Shader.PropertyToID("_MainTex"),
            _CameraOpaqueTexture = Shader.PropertyToID(nameof(_CameraOpaqueTexture)),
            _CameraDepthTexture = Shader.PropertyToID(nameof(_CameraDepthTexture))
            ;

        public const string _DEBUG = nameof(_DEBUG)
            ;
    }
}