Shader "Unlit/LogShader"
{
      // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    { 
          _MainTex ("Texture", 2D) = "white" {}
          _SpeedShake ("SpeedShake", float) = 2
          _DistanceShake ("DistanceShake", float) = 8
    }

    // The SubShader block containing the Shader code.
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass 
        {
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader.
            #pragma vertex vert
            // This line defines the name of the fragment shader.
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 vertex  : SV_POSITION;
                float2 uv: TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _SpeedShake;
            float _ShakeAmount;
            float3 _ShakePoint;
            float _DistanceShake;
            float4 _ForwardNormal;     

            float distanceCenter(float3 worldPos)
            {
                float dist = distance(_ShakePoint , worldPos);
                float obs = _DistanceShake -dist;
                if (obs < 0) obs = 0;
                return obs;
            }
            
            float4 shake_pos(Attributes IN)
            {
                if(_ShakeAmount <= 0) return TransformObjectToHClip(IN.vertex);

                float3 worldPos =  TransformObjectToWorld(IN.vertex);   
                float dist = distanceCenter(worldPos);
                if(dist <= 0) return TransformObjectToHClip(IN.vertex);            
                
                //float3 worldNormal = TransformObjectToWorldNormal(IN.normal);
                float2 norm = saturate(normalize(abs(_ForwardNormal.xz)));
                norm = sin(norm * _Time.y  * _SpeedShake) * _ShakeAmount;

              
                worldPos.xz += norm * dist; 

                return TransformWorldToHClip(worldPos);
            }
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.vertex = shake_pos(IN);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

         
            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = tex2D(_MainTex,IN.uv);
                return color;
            }
            ENDHLSL
        }
    }
}
