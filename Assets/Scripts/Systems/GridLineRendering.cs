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

        var rparams = new RenderParams(assets.LineMaterial);
        rparams.matProps = MaterialUtil.SharedPropertyBlock;
        rparams.matProps.SetColor("_Color", colors.LineColor);

        Entities.ForEach((in GridLine line) =>
        {
            var t = (float2)0;
            var s = (float2)appear.GridLineBoldness;

            var anim = appear.GridLineParam - line.Index * 0.4f;
            anim = math.smoothstep(0, 1, anim);

            if (!line.IsVertical)
            {
                t.y = line.Index - space.Dimensions.y * 0.5f;
                s.x = space.Dimensions.x * anim;
            }
            else
            {
                t.x = line.Index - space.Dimensions.x * 0.5f;
                s.y = space.Dimensions.y * anim;
            }

            var m = MatrixUtil.TRS2D(t, appear.GridLineDepth, 0, s);
            Graphics.RenderMesh(rparams, assets.LineMesh, 0, m);
        })
        .WithoutBurst().Run();
    }
}
