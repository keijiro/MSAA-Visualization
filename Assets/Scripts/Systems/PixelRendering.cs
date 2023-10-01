using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class PixelRenderingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<GridSpace>();
        RequireForUpdate<GridLineAppearance>();
        RequireForUpdate<SamplePointAppearance>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var grid = SystemAPI.GetSingleton<GridLineAppearance>();
        var point = SystemAPI.GetSingleton<SamplePointAppearance>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var props = MaterialUtil.SharedPropertyBlock;
        var rparams = new RenderParams(assets.PixelMaterial)
          { matProps = props };

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in Pixel pixel) =>
        {
            var alpha = 1 - math.abs(layer.Index - point.CurrentLayer);

            var color = colors.PixelColor;
            color.a *= pixel.Coverage * math.saturate(alpha);

            var p_gs = math.float2(coords.Value) + 0.5f;
            var p_ss = CoordUtil.GridToScreen(space, p_gs);

            var mtx = MatrixUtil.TRS2D(p_ss, grid.Depth, 0, 1);

            props.SetColor("_Color", color);
            Graphics.RenderMesh(rparams, assets.PixelMesh, 0, mtx);
        })
        .WithoutBurst().Run();
    }
}
