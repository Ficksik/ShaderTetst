Shader "Unlit/СrackShader"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CrackTexture ("Texture", 2D) = "white" {}
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
            #include "Crack.hlsl"
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
                float2 uvCrack: TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            float4 _MainTex_ST;
            float _DropAnimation;

            SAMPLER(sampler_MainTex);
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.vertex = TransformObjectToHClip(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.uvCrack = TR_CRACK_TEX(IN.uv);
                return OUT;
            }


            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, IN.uv);
                return crack(color,IN.uvCrack,_DropAnimation);
            }
            ENDHLSL
        }
    }
}