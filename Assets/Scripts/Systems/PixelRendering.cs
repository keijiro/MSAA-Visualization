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
        RequireForUpdate<Appearance>();
        RequireForUpdate<ColorScheme>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<Pixel>();
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var appear = SystemAPI.GetSingleton<Appearance>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();

        var rparams = new RenderParams(assets.PixelMaterial);
        rparams.matProps = MaterialUtil.SharedPropertyBlock;

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in Pixel pixel) =>
        {
            var color = colors.PixelColor;
            color = LayerUtil.ApplyAlpha(color, layer, appear.ActiveLayer);
            color.a *= pixel.Coverage * appear.PixelParam;

            var p_gs = math.float2(coords.Value) + 0.5f;
            var p_ss = CoordUtil.GridToScreen(space, p_gs);

            var mtx = MatrixUtil.TRS2D(p_ss, appear.GridLineDepth, 0, 1);

            rparams.matProps.SetColor("_Color", color);
            Graphics.RenderMesh(rparams, assets.PixelMesh, 0, mtx);
        })
        .WithoutBurst().Run();
    }
}
