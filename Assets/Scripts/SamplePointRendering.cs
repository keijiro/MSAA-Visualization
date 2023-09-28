using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class SamplePointRenderingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<SamplePointAppearance>();
        RequireForUpdate<GridConfig>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var appear = SystemAPI.GetSingleton<SamplePointAppearance>();
        var grid = SystemAPI.GetSingleton<GridConfig>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var props = MaterialUtil.SharedPropertyBlock;
        var rparams = new RenderParams(assets.PointMaterial) { matProps = props };

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in SamplePoint point,
                          in SampleResult result) =>
        {
            var color = result.Hit ? colors.HitColor : colors.MissColor;
            color.a *=
              math.saturate(1 - math.abs(layer.Index - appear.CurrentLayer));

            var p_gs = point.GetPosition(layer, coords);
            var p_ss = GridUtil.ToScreenSpace(p_gs, grid);

            var mtx = MatrixUtil.TRS(p_ss, 0, appear.Radius);

            props.SetColor("_Color", color);
            Graphics.RenderMesh(rparams, assets.PointMesh, 0, mtx);
        })
        .WithoutBurst().Run();
    }
}
