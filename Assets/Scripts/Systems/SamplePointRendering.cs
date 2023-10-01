using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class SamplePointRenderingSystem : SystemBase
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
        var render = new RenderUtil(assets.PointMesh, assets.PointMaterial);

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in SamplePoint point,
                          in SampleResult result) =>
        {
            var color = result.Hit ? colors.HitColor : colors.MissColor;
            color = LayerUtil.ApplyAlpha(color, layer, appear.ActiveLayer);

            var anim = appear.SamplePointParam;
            anim -= math.dot(coords.Value, math.float2(0.2f, 0.6f));
            anim = MathUtil.smootherstep(anim);

            var fade = math.saturate(1 - (appear.SamplePointParam - 10));
            color.a *= fade;

            var p_gs = point.GetPosition(layer, coords);
            var p_ss = CoordUtil.GridToScreen(space, p_gs);
            var scale = appear.SamplePointRadius * anim;

            render.Draw(p_ss, 0, 0, scale, color);
        })
        .WithoutBurst().Run();
    }
}
