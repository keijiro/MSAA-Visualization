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
        RequireForUpdate<Triangle>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var triangle = SystemAPI.GetSingleton<Triangle>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var v1 = CoordUtil.GridToScreen(space, triangle.Vertex1);
        var v2 = CoordUtil.GridToScreen(space, triangle.Vertex2);
        var v3 = CoordUtil.GridToScreen(space, triangle.Vertex3);

        var props = MaterialUtil.SharedPropertyBlock;
        props.SetColor("_Color", colors.TriangleColor);
        props.SetVector("_Vertex1", math.float4(v1, 0, 0));
        props.SetVector("_Vertex2", math.float4(v2, 0, 0));
        props.SetVector("_Vertex3", math.float4(v3, 0, 0));

        var rparams = new RenderParams(assets.TriangleMaterial) { matProps = props };
        Graphics.RenderMesh(rparams, assets.TriangleMesh, 0, Matrix4x4.identity);
    }
}
