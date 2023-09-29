using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class GridLineRenderingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<GridSpace>();
        RequireForUpdate<GridLineAppearance>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var appear = SystemAPI.GetSingleton<GridLineAppearance>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var props = MaterialUtil.SharedPropertyBlock;
        props.SetColor("_Color", colors.LineColor);

        var rparams = new RenderParams(assets.LineMaterial) { matProps = props };

        Entities.ForEach((in GridLine line) =>
        {
            var t = (float2)line.Index - (float2)space.Dimensions * 0.5f;
            t = line.IsVertical ? math.float2(t.x, 0) : math.float2(0, t.y);

            var r = line.IsVertical ? math.PI / 2 : 0;
            var s = line.IsVertical ? space.Dimensions.x : space.Dimensions.y;
            var m = MatrixUtil.TRS(t, r, math.float2(s, appear.Boldness));

            Graphics.RenderMesh(rparams, assets.LineMesh, 0, m);
        })
        .WithoutBurst().Run();
    }
}
