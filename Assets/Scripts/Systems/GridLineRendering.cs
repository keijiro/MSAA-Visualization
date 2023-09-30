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
        RequireForUpdate<GridLineAppearance>();
        RequireForUpdate<RenderingAssets>();
        RequireForUpdate<ColorScheme>();
    }

    protected override void OnUpdate()
    {
        var space = SystemAPI.GetSingleton<GridSpace>();
        var appear = SystemAPI.GetSingleton<GridLineAppearance>();
        var assets = SystemAPI.ManagedAPI.GetSingleton<RenderingAssets>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var props = MaterialUtil.SharedPropertyBlock;
        props.SetColor("_Color", colors.LineColor);

        var rparams = new RenderParams(assets.LineMaterial) { matProps = props };

        Entities.ForEach((in GridLine line) =>
        {
            var t = (float2)0;
            var s = (float2)appear.Boldness;

            if (!line.IsVertical)
            {
                t.y = line.Index - space.Dimensions.y * 0.5f;
                s.x = space.Dimensions.x;
            }
            else
            {
                t.x = line.Index - space.Dimensions.x * 0.5f;
                s.y = space.Dimensions.y;
            }

            var m = MatrixUtil.TRS2D(t, appear.Depth, 0, s);
            Graphics.RenderMesh(rparams, assets.LineMesh, 0, m);
        })
        .WithoutBurst().Run();
    }
}
