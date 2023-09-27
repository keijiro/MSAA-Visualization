using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class SamplePointRenderingSystem : SystemBase
{
    protected override void OnCreate()
      => RequireForUpdate<SamplePointRenderingConfig>();

    protected override void OnUpdate()
    {
        var config = SystemAPI.ManagedAPI.
          GetSingleton<SamplePointRenderingConfig>();

        var grid = SystemAPI.GetSingleton<GridConfig>();

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in SamplePoint point) =>
        {
            var rparams = new RenderParams(config.Material);

            var sp = point.GetScreenSpacePosition(layer, coords, grid);
            var pos = math.float3(sp, 0);

            var matrix = float4x4.TRS(pos, quaternion.identity, config.Radius);

            Graphics.RenderMesh(rparams, config.Mesh, 0, matrix);
        })
        .WithoutBurst().Run();
    }
}
