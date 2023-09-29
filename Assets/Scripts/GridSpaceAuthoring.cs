using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class GridSpaceAuthoring : MonoBehaviour
{
    public uint2 Dimensions = 4;

    class Baker : Baker<GridSpaceAuthoring>
    {
        public override void Bake(GridSpaceAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None),
                          new GridSpace(){ Dimensions = src.Dimensions });
    }
}
