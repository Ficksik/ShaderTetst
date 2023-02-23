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

float4 shake_pos(float4 vertex)
{
    float3 worldPos = TransformObjectToWorld(vertex);
    float dist = distanceCenter(worldPos);
    dist = max(0, dist);

    //float3 worldNormal = TransformObjectToWorldNormal(IN.normal); 
    float2 norm = saturate(normalize(abs(_ForwardNormal.xz)));
    norm = sin(norm * _Time.y * _SpeedShake) * _ShakeAmount;
    
    worldPos.xz += norm * dist;

    return TransformWorldToHClip(worldPos);
}

float4 destroy_position_animate(float4 vertex)
{
    float3 worldPos = TransformObjectToWorld(vertex);

    // calculate drop position based on world Y coordinate
    float dropPosition = worldPos.y - (_DropHeight);
    // offset vertex position by drop position and time
    worldPos.y = lerp(worldPos.y, dropPosition, _DropAnimation);


    float2 norm = saturate(normalize(abs(_ForwardNormal.xz)));
    float2 sidePos = worldPos.xz + norm * _DropSide;
    worldPos.xz = lerp(worldPos.xz, sidePos, _DropAnimation);

    return TransformWorldToHClip(worldPos);
}

float4 DropDownCalcNewPos(float4 vertex)
{
    return _DropAnimation > 0 ?
        destroy_position_animate(vertex) :
    _ShakeAmount <= 0 ? TransformObjectToHClip(vertex) :shake_pos(vertex);
}
