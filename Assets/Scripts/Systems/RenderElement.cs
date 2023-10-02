using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class RenderElementSystem : SystemBase
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

        var point = new RenderUtil(assets.PointMesh, assets.PointMaterial);
        var quad = new RenderUtil(assets.QuadMesh, assets.QuadMaterial);

        Entities.ForEach((in RenderElement e) =>
        {
            if (e.Type == RenderElement.ElementType.Point)
                point.Draw(e.Position, e.Depth, e.Angle, e.Scale, e.Color);
            else if (e.Type == RenderElement.ElementType.Quad)
                quad.Draw(e.Position, e.Depth, e.Angle, e.Scale, e.Color);
        })
        .WithoutBurst().Run();
    }
}
