half4 disolve(half4 texColor,float dropAnimation)
{
    texColor.a = lerp(texColor.a, 0, dropAnimation);
    return texColor;
}