using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct GridConfig : IComponentData
{
    public uint2 Dimensions;
}

public class GridConfigAuthoring : MonoBehaviour
{
    public uint2 Dimensions = 4;

    class Baker : Baker<GridConfigAuthoring>
    {
        public override void Bake(GridConfigAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None),
                          new GridConfig(){ Dimensions = src.Dimensions });
    }
}