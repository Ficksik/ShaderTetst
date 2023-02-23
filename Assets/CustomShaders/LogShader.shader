Shader "Unlit/LogShader"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SpeedShake ("SpeedShake", float) = 2
        _DistanceShake ("DistanceShake", float) = 8
        _DropHeight ("DropHeight", float) = 5
        _DropSide("DropSide", float) = 8
    }

    // The SubShader block containing the Shader code.
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags
        {
            "Queue"="Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull front
        LOD 100

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
                float4 vertex : SV_POSITION;
                float2 uv: TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            //Shake Animation  
            float _SpeedShake;
            float _ShakeAmount;
            float _DistanceShake;
            float3 _ShakePoint;
            float4 _ForwardNormal;

            //Destoy Animation
            float _DropHeight;
            float _DropSide;
            float _DropAnimation;

            float distanceCenter(float3 worldPos)
            {
                float dist = distance(_ShakePoint, worldPos);
                return _DistanceShake - dist;
            }

            float4 shake_pos(Attributes IN)
            {
                if (_ShakeAmount <= 0) return TransformObjectToHClip(IN.vertex);
                float3 worldPos = TransformObjectToWorld(IN.vertex);
                float dist = distanceCenter(worldPos);
                if (dist < 0) dist = 0;
                if (dist <= 0) return TransformObjectToHClip(IN.vertex);

                //float3 worldNormal = TransformObjectToWorldNormal(IN.normal); 
                float2 norm = saturate(normalize(abs(_ForwardNormal.xz)));
                norm = sin(norm * _Time.y * _SpeedShake) * _ShakeAmount;


                worldPos.xz += norm * dist;

                return TransformWorldToHClip(worldPos);
            }

            float4 destroy_position_animate(Attributes IN)
            {
                float3 worldPos = TransformObjectToWorld(IN.vertex);

                // calculate drop position based on world Y coordinate
                float dropPosition = worldPos.y - (_DropHeight);
                // offset vertex position by drop position and time
                worldPos.y = lerp(worldPos.y, dropPosition, _DropAnimation);


                float2 norm = saturate(normalize(abs(_ForwardNormal.xz)));
                float2 sidePos = worldPos.xz + norm * _DropSide;
                worldPos.xz = lerp(worldPos.xz, sidePos, _DropAnimation);

                return TransformWorldToHClip(worldPos);
            }

            float4 calc_new_pos(Attributes IN)
            {
                if (_DropAnimation > 0)
                {
                    return destroy_position_animate(IN);
                };
                return shake_pos(IN);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.vertex = calc_new_pos(IN);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 disolve(half4 texColor, Varyings IN)
            {
                texColor.a = lerp(texColor.a, 0, _DropAnimation);
                return texColor;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.uv);
                if (_DropAnimation > 0) return disolve(color, IN);
                return color;
            }
            ENDHLSL
        }
    }
}