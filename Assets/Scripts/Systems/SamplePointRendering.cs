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

        var rparams = new RenderParams(assets.PointMaterial);
        rparams.matProps = MaterialUtil.SharedPropertyBlock;

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in SamplePoint point,
                          in SampleResult result) =>
        {
            var color = result.Hit ? colors.HitColor : colors.MissColor;
            color = LayerUtil.ApplyAlpha(color, layer, appear.ActiveLayer);

            var anim = appear.SamplePointParam;
            anim -= math.dot(coords.Value, math.float2(0.2f, 0.6f));
            anim = math.smoothstep(0, 1, anim);

            var p_gs = point.GetPosition(layer, coords);
            var p_ss = CoordUtil.GridToScreen(space, p_gs);
            var scale = appear.SamplePointRadius * anim;

            var mtx = MatrixUtil.TRS(p_ss, 0, scale);

            rparams.matProps.SetColor("_Color", color);
            Graphics.RenderMesh(rparams, assets.PointMesh, 0, mtx);
        })
        .WithoutBurst().Run();
    }
}
