using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class GridLineRenderingSystem : SystemBase
{
    protected override void OnCreate()
      => RequireForUpdate<GridLineRendering>();

    protected override void OnUpdate()
    {
        var render = SystemAPI.ManagedAPI.GetSingleton<GridLineRendering>();
        var grid = SystemAPI.GetSingleton<GridConfig>();
        var rparams = new RenderParams(render.Material);

        Entities.ForEach((in GridLine line) =>
        {
            var t = math.float2(line.Index, 0);
            t = line.IsVertical ? t.xy : t.yx;
            t -= (float2)grid.Dimensions * 0.5f;

            var r = line.IsVertical ? math.PI / 2 : 0;
            var s = line.IsVertical ? grid.Dimensions.x : grid.Dimensions.y;

            Graphics.RenderMesh(rparams, render.Mesh, 0, MatrixUtil.TRS(t, r, s));
        })
        .WithoutBurst().Run();
    }
}
