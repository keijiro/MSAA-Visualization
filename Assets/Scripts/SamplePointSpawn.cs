using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public partial class SamplePointSpawnSystem : SystemBase
{
    protected override void OnCreate()
      => RequireForUpdate<Config>();

    protected override void OnUpdate()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var config = SystemAPI.ManagedAPI.GetSingleton<Config>();

        // Entity prototype
        var proto = manager.CreateEntity();
        manager.AddComponent<SamplePoint>(proto);
        RenderMeshUtility.AddComponents
          (proto, manager,
           new RenderMeshDescription
             { FilterSettings = RenderFilterSettings.Default },
           new RenderMeshArray(new[]{config.Material}, new[]{config.PointMesh}),
           MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

        // Instantiation
        for (var y = 0u; y < config.RowCount; y++)
        {
            for (var x = 0u; x < config.RowCount; x++)
            {
                var instance = manager.Instantiate(proto);
                var data = new SamplePoint{Indices = math.uint2(x, y)};
                SystemAPI.SetComponent(instance, data);
            }
        }

        // Cleaning up
        manager.DestroyEntity(proto);
        Enabled = false;
    }
}
