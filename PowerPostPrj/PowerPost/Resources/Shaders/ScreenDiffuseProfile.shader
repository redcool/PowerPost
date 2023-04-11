Shader "Hidden/PowerPost/ScreenDiffuseProfile"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _BlurSize("_BlurSize",range(0,10)) = 0.1
        _StencilRef("_StencilRef",int) = 5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ztest always
        zwrite off
        cull off

        stencil{
            ref [_StencilRef]
            pass keep
            comp equal
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag

            #include "PowerPostLib.hlsl"
            #include "DiffuseProfile.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            
            float _BlurSize;

            half4 frag (VaryingsDefault i) : SV_Target
            {
                half4 mainColor = tex2D(_MainTex,i.texcoord);
                // return mainColor * 0.5;
                float3 c = DiffuseProfile(mainColor,_MainTex,i.texcoord,float2(_MainTex_TexelSize.x,0) * _BlurSize,1);
                c += DiffuseProfile(mainColor,_MainTex,i.texcoord,float2(0,_MainTex_TexelSize.y)  * _BlurSize,1);
                c/=2;
                return float4(c,1);
            }
            ENDHLSL
        }

    }
}
