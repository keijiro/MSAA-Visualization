using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class TriangleRenderingSystem : SystemBase
{
    MaterialPropertyBlock _sheet;

    protected override void OnCreate()
    {
        RequireForUpdate<Triangle>();
        RequireForUpdate<TriangleRendering>();
        _sheet = new MaterialPropertyBlock();
    }

    protected override void OnUpdate()
    {
        var render = SystemAPI.ManagedAPI.GetSingleton<TriangleRendering>();
        var grid = SystemAPI.GetSingleton<GridConfig>();
        var triangle = SystemAPI.GetSingleton<Triangle>();

        var rparams = new RenderParams(render.Material);
        rparams.matProps = _sheet;

        var v1 = GridUtil.ToScreenSpace(triangle.Vertex1, grid);
        var v2 = GridUtil.ToScreenSpace(triangle.Vertex2, grid);
        var v3 = GridUtil.ToScreenSpace(triangle.Vertex3, grid);

        _sheet.SetColor("_Color", new Color(1, 1, 1, 0.3f));
        _sheet.SetVector("_Vertex1", math.float4(v1, 0, 0));
        _sheet.SetVector("_Vertex2", math.float4(v2, 0, 0));
        _sheet.SetVector("_Vertex3", math.float4(v3, 0, 0));

        Graphics.RenderMesh(rparams, render.Mesh, 0, Matrix4x4.identity);
    }
}
