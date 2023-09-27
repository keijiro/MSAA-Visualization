using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class SamplePointRenderingSystem : SystemBase
{
    protected override void OnCreate()
      => RequireForUpdate<SamplePointRendering>();

    protected override void OnUpdate()
    {
        var render = SystemAPI.ManagedAPI.GetSingleton<SamplePointRendering>();
        var grid = SystemAPI.GetSingleton<GridConfig>();
        var rparams = new RenderParams(render.Material);

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in SamplePoint point) =>
        {
            var pos = point.GetScreenSpacePosition(layer, coords, grid);
            var matrix = MatrixUtil.TRS(pos, 0, render.Radius);
            Graphics.RenderMesh(rparams, render.Mesh, 0, matrix);
        })
        .WithoutBurst().Run();
    }
}
