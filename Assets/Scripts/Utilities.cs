using Unity.Mathematics;

public static class MatrixUtil
{
    public static float4x4 TRS(float2 position, float angle, float scale)
      => float4x4.TRS(math.float3(position, 0),
                      quaternion.RotateZ(angle),
                      scale);
}

public static class GridUtil
{
    public static float2 ToScreenSpace(float2 pos, in GridConfig config)
      => pos - (float2)config.Dimensions / 2;
}

public static class TriangleUtil
{
    static float Cross(float2 v1, float2 v2)
      => v1.x * v2.y - v1.y * v2.x;

    public static bool TestPoint(float2 pt, float2 v1, float2 v2, float2 v3)
    {
        var a =  (Cross(pt, v3) - Cross(v1, v3)) / Cross(v2, v3);
        var b = -(Cross(pt, v2) - Cross(v1, v2)) / Cross(v2, v3);
        return a > 0 && b > 0 && (a + b) < 1;
    }

    public static bool TestPoint(float2 pt, in Triangle tri)
      => TestPoint(pt, tri.Vertex1, tri.Vertex2, tri.Vertex3);
}
