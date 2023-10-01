using UnityEngine;
using Unity.Mathematics;

public static class MathUtil
{
    public static float smootherstep(float e0, float e1, float x)
      => smootherstep((x - e0) / (e1 - e0));

    public static float smootherstep(float x)
    {
        x = math.saturate(x);
        return x * x * x * (x * (x * 6 - 15) + 10);
    }
}

public readonly struct RenderUtil
{
    readonly RenderParams Params;
    readonly Mesh Mesh;

    public static MaterialPropertyBlock SharedPropertyBlock
      = new MaterialPropertyBlock();

    public MaterialPropertyBlock PropertyBlock => SharedPropertyBlock;

    public RenderUtil(Mesh mesh, Material material)
      => (Params, Mesh) =
           (new RenderParams(material){ matProps = SharedPropertyBlock },
            mesh);

    public void Draw
      (float2 pos, float depth, float angle, float2 scale, Color color)
    {
        Params.matProps.SetColor("_Color", color);
        Graphics.RenderMesh(Params, Mesh, 0, MatrixUtil.TRS2D(pos, depth, angle, scale));
    }

    public void Draw(Color color)
    {
        Params.matProps.SetColor("_Color", color);
        Graphics.RenderMesh(Params, Mesh, 0, float4x4.identity);
    }
}

public static class MatrixUtil
{
    public static float4x4 TRS2D(float2 t, float d, float r, float2 s)
      => float4x4.TRS(math.float3(t, d),
                      quaternion.RotateZ(r),
                      math.float3(s, 1));

    public static float4x4 TRS(float2 position, float angle, float scale)
      => float4x4.TRS(math.float3(position, 0),
                      quaternion.RotateZ(angle),
                      scale);

    public static float4x4 TRS(float2 position, float angle, float2 scale)
      => float4x4.TRS(math.float3(position, 0),
                      quaternion.RotateZ(angle),
                      math.float3(scale, 1));
}

public static class CoordUtil
{
    public static float2 GridToScreen(in GridSpace grid, float2 pos)
      => pos - (float2)grid.Dimensions / 2;

    public static int PixelToIndex
      (in GridSpace grid, in Layer layer, in PixelCoords coords)
      => (int)(layer.Index * grid.Dimensions.x * grid.Dimensions.y
         + coords.Value.y * grid.Dimensions.x + coords.Value.x);
}

public static class LayerUtil
{
    public static Color ApplyAlpha(Color color, in Layer layer, float select)
    {
        color.a *= math.saturate(1 - math.abs(layer.Index - select));
        return color;
    }
}

public static class TriangleUtil
{
    static float Cross(float2 v1, float2 v2)
      => v1.x * v2.y - v1.y * v2.x;

    public static bool TestPoint(float2 pt, float2 v1, float2 v2, float2 v3)
    {
        // Triangle interior test from:
        // https://mathworld.wolfram.com/TriangleInterior.html
        v2 -= v1;
        v3 -= v1;
        var div = math.rcp(Cross(v2, v3));
        var a =  (Cross(pt, v3) - Cross(v1, v3)) * div;
        var b = -(Cross(pt, v2) - Cross(v1, v2)) * div;
        return a > 0 && b > 0 && (a + b) < 1;
    }

    public static bool TestPoint(float2 pt, in Triangle tri)
      => TestPoint(pt, tri.Vertex1, tri.Vertex2, tri.Vertex3);
}
