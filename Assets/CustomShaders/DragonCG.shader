Shader "Custom/DragonCG"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #pragma multi_compile_fwdbase //shadows
            #include "AutoLight.cginc" //shadows
 
 
            #include "UnityCG.cginc"
            #define Z_TEXTURE_CHANNELS 6
            #define Z_DEFINE_MESH_ATTRIBUTES COLOR UV3 UV4
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                LIGHTING_COORDS(3, 4) //shadows
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NormalMap;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)//shadows
                return o;
            }

           fixed4 frag (v2f i) : SV_Target {
                const float3 light = _WorldSpaceLightPos0.xyz;
               
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 normal = tex2D(_NormalMap, i.uv).rgb * 2.0 - 1.0;
                float diff = max(0, dot(normal, light));
                return col * diff;
            }
            ENDCG
        }
         Pass
       {
           Tags {"LightMode" = "ShadowCaster"}
 
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #pragma multi_compile_shadowcaster
           #include "UnityCG.cginc"
 
           struct v2f {
               V2F_SHADOW_CASTER;
           };
 
           v2f vert(appdata_base v)
           {
               v2f o;
               TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
               return o;
           }
 
           float4 frag(v2f i) : SV_Target
           {
               SHADOW_CASTER_FRAGMENT(i)
           }
           ENDCG
       }
    }
}
