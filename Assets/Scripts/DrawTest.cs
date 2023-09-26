using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class DrawTest : SystemBase
{
    protected override void OnCreate()
      => RequireForUpdate<Config>();

    protected override void OnUpdate()
    {
        //var config = SystemAPI.ManagedAPI.GetSingleton<Config>();

        Entities.ForEach((in Config config) => {
            var rparams = new RenderParams(config.Material);
            Graphics.RenderMesh(rparams, config.PointMesh, 0,
                Matrix4x4.Translate(Vector3.right * config.PointRadius));
            Debug.Log(config.PointRadius);
        }).WithoutBurst().Run();
    }
}
