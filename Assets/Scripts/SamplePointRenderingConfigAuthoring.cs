using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class SamplePointRenderingConfig : IComponentData
{
    public Mesh Mesh;
    public Material Material;
    public float Radius;
}

public class SamplePointRenderingConfigAuthoring : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;
    public float Radius = 0.1f;

    class Baker : Baker<SamplePointRenderingConfigAuthoring>
    {
        public override void Bake(SamplePointRenderingConfigAuthoring src)
        {
            var data = new SamplePointRenderingConfig()
              { Mesh = src.Mesh, Material = src.Material, Radius = src.Radius};

            AddComponentObject(GetEntity(TransformUsageFlags.None), data);

            var grid = GetComponent<GridConfigAuthoring>();

            for (var x = 0u; x < grid.Dimensions.x; x++)
            {
                for (var y = 0u; y < grid.Dimensions.y; y++)
                {
                    var add = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddComponent(add, new PixelCoords() { Value = math.uint2(x, y) });
                    AddComponent(add, new SamplePoint() { Index = 0 });
                    AddComponent(add, new Layer() { Index = 0 });
                }
            }
        }
    }
}
