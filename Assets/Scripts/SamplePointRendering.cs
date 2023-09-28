using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class SamplePointRenderingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<SamplePointRendering>();
        RequireForUpdate<GridConfig>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var render = SystemAPI.ManagedAPI.GetSingleton<SamplePointRendering>();
        var grid = SystemAPI.GetSingleton<GridConfig>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var props = MaterialUtil.SharedPropertyBlock;
        var rparams = new RenderParams(render.Material) { matProps = props };

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in SamplePoint point,
                          in SampleResult result) =>
        {
            var color = result.Hit ? colors.HitColor : colors.MissColor;
            color.a *=
              math.saturate(1 - math.abs(layer.Index - render.CurrentLayer));

            var p_gs = point.GetPosition(layer, coords);
            var p_ss = GridUtil.ToScreenSpace(p_gs, grid);

            var mtx = MatrixUtil.TRS(p_ss, 0, render.Radius);

            props.SetColor("_Color", color);
            Graphics.RenderMesh(rparams, render.Mesh, 0, mtx);
        })
        .WithoutBurst().Run();
    }
}
