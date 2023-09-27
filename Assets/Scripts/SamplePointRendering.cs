using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class SamplePointRenderingSystem : SystemBase
{
    MaterialPropertyBlock _sheet;

    protected override void OnCreate()
    {
        RequireForUpdate<SamplePointRendering>();
        _sheet = new MaterialPropertyBlock();
    }

    protected override void OnUpdate()
    {
        var render = SystemAPI.ManagedAPI.GetSingleton<SamplePointRendering>();
        var grid = SystemAPI.GetSingleton<GridConfig>();

        var rparams = new RenderParams(render.Material);
        rparams.matProps = _sheet;

        Entities.ForEach((in Layer layer,
                          in PixelCoords coords,
                          in SamplePoint point,
                          in SampleResult result) =>
        {
            var alpha =
              math.saturate(1 - math.abs(layer.Index - render.CurrentLayer));

            var p_gs = point.GetPosition(layer, coords);
            var p_ss = GridUtil.ToScreenSpace(p_gs, grid);

            var mtx = MatrixUtil.TRS(p_ss, 0, render.Radius);

            _sheet.SetColor("_Color", (result.Hit ? Color.red : Color.blue) * alpha);
            Graphics.RenderMesh(rparams, render.Mesh, 0, mtx);
        })
        .WithoutBurst().Run();
    }
}
