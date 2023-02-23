TEXTURE2D(_CrackTexture);
float4 _CrackTexture_ST;
SAMPLER(sampler_CrackTexture);

#define TR_CRACK_TEX(uv) (TRANSFORM_TEX(uv, _CrackTexture));

half4 crack(half4 texColor,float2 uv,float dropAnimation)
{
    half4 crackColor = SAMPLE_TEXTURE2D(_CrackTexture,sampler_CrackTexture,uv);
    half crack = clamp(1 - crackColor.r,0,1);
    texColor.rgb = lerp(texColor.rgb,texColor.rgb-crack,dropAnimation);
    return texColor;
}