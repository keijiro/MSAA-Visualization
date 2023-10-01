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
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var appear = SystemAPI.GetSingleton<Appearance>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var render = new RenderUtil(assets.PixelMesh, assets.PixelMaterial);

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in Pixel pixel) =>
        {
            var color = colors.PixelColor;
            color = LayerUtil.ApplyAlpha(color, layer, appear.ActiveLayer);
            color.a *= pixel.Coverage * appear.PixelParam;

            var p_gs = math.float2(coords.Value) + 0.5f;
            var p_ss = CoordUtil.GridToScreen(space, p_gs);

            render.Draw(p_ss, appear.GridLineDepth, 0, 1, color);
        })
        .WithoutBurst().Run();
    }
}
