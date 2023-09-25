using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public partial class GridSpawnSystem : SystemBase
{
    protected override void OnCreate()
      => RequireForUpdate<GridConfig>();

    protected override void OnUpdate()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var config = SystemAPI.ManagedAPI.GetSingleton<GridConfig>();

        var desc = new RenderMeshDescription
          { FilterSettings = RenderFilterSettings.Default };

        var array = new RenderMeshArray
            (new[]{config.GridMaterial},
             new[]{config.GridMesh});

        var prot = manager.CreateEntity();
        RenderMeshUtility.AddComponents
          (prot, manager, desc, array,
           MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

        for (var yi = 0; yi < config.GridCount; yi++)
        {
            for (var xi = 0; xi < config.GridCount; xi++)
            {
                var instance = manager.Instantiate(prot);
                var mtx = float4x4.Translate(math.float3(xi, yi, 0));
                SystemAPI.SetComponent(instance, new LocalToWorld{Value = mtx});
            }
        }

        manager.DestroyEntity(prot);

        Enabled = false;
    }
}
