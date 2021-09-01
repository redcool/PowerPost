Shader "Hidden/PostProcessingEx/ScreenDiffuseProfile"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _BlurSize("_BlurSize",range(0,10)) = 0.1
    }
    CGINCLUDE
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = (v.vertex);
                o.uv = v.uv;
                return o;
            }
    ENDCG
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ztest always
        zwrite off
        cull off

        stencil{
            ref 5
            pass keep
            comp equal
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "DiffuseProfile.cginc"


            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _BlurSize;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 mainColor = tex2D(_MainTex,i.uv);
                // return mainColor * 0.5;
                float3 c = DiffuseProfile(mainColor,_MainTex,i.uv,float2(_MainTex_TexelSize.x,0) * _BlurSize,1);
                c += DiffuseProfile(mainColor,_MainTex,i.uv,float2(0,_MainTex_TexelSize.y)  * _BlurSize,1);
                c/=2;
                return float4(c,1);
            }
            ENDCG
        }

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            sampler2D _MainTex;

            fixed4 frag(v2f i):SV_TARGET{
                return tex2D(_MainTex,i.uv);
            }

            ENDCG
        }
    }
}
