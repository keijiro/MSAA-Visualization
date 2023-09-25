using UnityEngine;
using Unity.Entities;

public class GridConfig : IComponentData
{
    public uint GridCount;
    public Mesh GridMesh;
    public Material GridMaterial;
}

public class GridConfigAuthoring : MonoBehaviour
{
    public uint GridCount = 4;
    public Mesh GridMesh;
    public Material GridMaterial;

    class GridConfigBaker : Baker<GridConfigAuthoring>
    {
        public override void Bake(GridConfigAuthoring src)
        {
            var data = new GridConfig()
            {
                GridCount = src.GridCount,
                GridMesh = src.GridMesh,
                GridMaterial = src.GridMaterial
            };
            AddComponentObject(GetEntity(TransformUsageFlags.None), data);
        }
    }
}
