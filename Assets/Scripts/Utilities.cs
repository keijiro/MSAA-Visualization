using Unity.Mathematics;

public static class MatrixUtil
{
    public static float4x4 TRS(float2 position, float angle, float scale)
      => float4x4.TRS(math.float3(position, 0),
                      quaternion.RotateZ(angle),
                      scale);
}
