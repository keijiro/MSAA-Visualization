using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class GridLineRenderingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<GridLineRendering>();
        RequireForUpdate<GridConfig>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var render = SystemAPI.ManagedAPI.GetSingleton<GridLineRendering>();
        var grid = SystemAPI.GetSingleton<GridConfig>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var props = MaterialUtil.SharedPropertyBlock;
        props.SetColor("_Color", colors.LineColor);

        var rparams = new RenderParams(render.Material) { matProps = props };

        Entities.ForEach((in GridLine line) =>
        {
            var t = (float2)line.Index - (float2)grid.Dimensions * 0.5f;
            t = line.IsVertical ? math.float2(t.x, 0) : math.float2(0, t.y);

            var r = line.IsVertical ? math.PI / 2 : 0;
            var s = line.IsVertical ? grid.Dimensions.x : grid.Dimensions.y;

            Graphics.RenderMesh(rparams, render.Mesh, 0, MatrixUtil.TRS(t, r, s));
        })
        .WithoutBurst().Run();
    }
}
