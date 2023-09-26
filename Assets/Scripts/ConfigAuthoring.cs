using UnityEngine;
using Unity.Entities;

public class Config : IComponentData
{
    public uint RowCount;
    public float PointRadius;
    public Mesh LineMesh;
    public Mesh PointMesh;
    public Material Material;
}

public class ConfigAuthoring : MonoBehaviour
{
    public uint RowCount = 4;
    public float PointRadius = 0.1f;
    public Mesh LineMesh;
    public Mesh PointMesh;
    public Material Material;

    class ConfigBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring src)
        {
            var data = new Config()
            {
                RowCount = src.RowCount,
                PointRadius = src.PointRadius,
                LineMesh = src.LineMesh,
                PointMesh = src.PointMesh,
                Material = src.Material
            };
            AddComponentObject(GetEntity(TransformUsageFlags.None), data);

            var data2 = new Config()
            {
                RowCount = src.RowCount,
                PointRadius = -src.PointRadius,
                LineMesh = src.LineMesh,
                PointMesh = src.PointMesh,
                Material = src.Material
            };
            var add = CreateAdditionalEntity(TransformUsageFlags.None);
            AddComponentObject(add, data2);
        }
    }
}
