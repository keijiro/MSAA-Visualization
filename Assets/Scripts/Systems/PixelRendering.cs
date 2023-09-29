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
        RequireForUpdate<SamplePointAppearance>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var appear = SystemAPI.GetSingleton<SamplePointAppearance>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var props = MaterialUtil.SharedPropertyBlock;
        var rparams = new RenderParams(assets.PixelMaterial)
          { matProps = props };

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in Pixel pixel) =>
        {
            var color = colors.PixelColor * pixel.Coverage;
            color.a *=
              math.saturate(1 - math.abs(layer.Index - appear.CurrentLayer));

            var p_gs = math.float2(coords.Value) + 0.5f;
            var p_ss = CoordUtil.GridToScreen(space, p_gs);

            var mtx = MatrixUtil.TRS(p_ss, 0, 1);

            props.SetColor("_Color", color);
            Graphics.RenderMesh(rparams, assets.PixelMesh, 0, mtx);
        })
        .WithoutBurst().Run();
    }
}
