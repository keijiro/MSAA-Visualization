using UnityEngine;
using Unity.Entities;

public class GridConfig : IComponentData
{
    public uint GridCount;
    public float PointRadius;
    public Mesh GridMesh;
    public Mesh PointMesh;
    public Material Material;
}

public class GridConfigAuthoring : MonoBehaviour
{
    public uint GridCount = 4;
    public float PointRadius = 0.1f;
    public Mesh GridMesh;
    public Mesh PointMesh;
    public Material Material;

    class GridConfigBaker : Baker<GridConfigAuthoring>
    {
        public override void Bake(GridConfigAuthoring src)
        {
            var data = new GridConfig()
            {
                GridCount = src.GridCount,
                PointRadius = src.PointRadius,
                GridMesh = src.GridMesh,
                PointMesh = src.PointMesh,
                Material = src.Material
            };
            AddComponentObject(GetEntity(TransformUsageFlags.None), data);
        }
    }
}
