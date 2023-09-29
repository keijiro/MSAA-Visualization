using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class GridLineRenderingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<GridConfig>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<ColorScheme>();
        RequireForUpdate<GridLineAppearance>();
    }

    protected override void OnUpdate()
    {
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var grid = SystemAPI.GetSingleton<GridConfig>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();
        var appear = SystemAPI.GetSingleton<GridLineAppearance>();

        var props = MaterialUtil.SharedPropertyBlock;
        props.SetColor("_Color", colors.LineColor);

        var rparams = new RenderParams(assets.LineMaterial) { matProps = props };

        Entities.ForEach((in GridLine line) =>
        {
            var t = (float2)line.Index - (float2)grid.Dimensions * 0.5f;
            t = line.IsVertical ? math.float2(t.x, 0) : math.float2(0, t.y);

            var r = line.IsVertical ? math.PI / 2 : 0;
            var s = line.IsVertical ? grid.Dimensions.x : grid.Dimensions.y;
            var m = MatrixUtil.TRS(t, r, math.float2(s, appear.Boldness));

            Graphics.RenderMesh(rparams, assets.LineMesh, 0, m);
        })
        .WithoutBurst().Run();
    }
}
