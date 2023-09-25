using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public partial class GridSpawnSystem : SystemBase
{
    #region System implementation

    protected override void OnCreate()
      => RequireForUpdate<GridConfig>();

    protected override void OnUpdate()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var config = SystemAPI.ManagedAPI.GetSingleton<GridConfig>();

        var line = CreatePrototype(manager, config.GridMesh, config.Material);
        var point = CreatePrototype(manager, config.PointMesh, config.Material);

        for (var yi = 0; yi < config.GridCount; yi++)
        {
            for (var xi = 0; xi < config.GridCount; xi++)
            {
                Instantiate(manager, point,
                            math.float3(xi, yi, 0) * 0.1f,
                            quaternion.identity,
                            config.PointRadius);
            }

            Instantiate(manager, line,
                        math.float3(config.GridCount * -0.5f, yi, 0) * 0.1f,
                        quaternion.identity,
                        config.GridCount);
        }

        manager.DestroyEntity(line);
        manager.DestroyEntity(point);

        Enabled = false;
    }

    #endregion

    #region Private methods

    Entity CreatePrototype(EntityManager manager, Mesh mesh, Material material)
    {
        var entity = manager.CreateEntity();
        RenderMeshUtility.AddComponents
          (entity, manager,
           new RenderMeshDescription
            { FilterSettings = RenderFilterSettings.Default },
           new RenderMeshArray(new[]{material}, new[]{mesh}),
           MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
        return entity;
    }

    void Instantiate
      (EntityManager manager, Entity prototype,
       float3 position, quaternion rotation, float scale)
    {
        var instance = manager.Instantiate(prototype);
        var mtx = float4x4.TRS(position, rotation, scale);
        SystemAPI.SetComponent(instance, new LocalToWorld{Value = mtx});
    }

    #endregion
}
