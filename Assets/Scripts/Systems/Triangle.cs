using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class TriangleRenderingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<GridSpace>();
        RequireForUpdate<Appearance>();
        RequireForUpdate<ColorScheme>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<Triangle>();
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var appear = SystemAPI.GetSingleton<Appearance>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var render = new RenderUtil(assets.TriangleMesh, assets.TriangleMaterial);

        var triangle = SystemAPI.GetSingleton<Triangle>();
        var v1 = CoordUtil.GridToScreen(space, triangle.Vertex1);
        var v2 = CoordUtil.GridToScreen(space, triangle.Vertex2);
        var v3 = CoordUtil.GridToScreen(space, triangle.Vertex3);

        var color = colors.TriangleColor;
        color.a *= appear.TriangleParam;

        var props = render.PropertyBlock;
        props.SetVector("_Vertex1", math.float4(v1, 0, 0));
        props.SetVector("_Vertex2", math.float4(v2, 0, 0));
        props.SetVector("_Vertex3", math.float4(v3, 0, 0));
        render.Draw(color);
    }
}
