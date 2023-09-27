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
        RequireForUpdate<TriangleRendering>();
        _sheet = new MaterialPropertyBlock();
    }

    protected override void OnUpdate()
    {
        var render = SystemAPI.ManagedAPI.GetSingleton<TriangleRendering>();
        var triangle = SystemAPI.GetSingleton<Triangle>();

        var rparams = new RenderParams(render.Material);
        rparams.matProps = _sheet;

        Graphics.RenderMesh(rparams, render.Mesh, 0, Matrix4x4.identity);
    }
}
