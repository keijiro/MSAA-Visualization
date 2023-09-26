using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class SamplePointRenderer : IComponentData
{
    public Mesh Mesh;
    public Material Material;
}

public class SamplePointRendererAuthoring : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;

    class Baker : Baker<SamplePointRendererAuthoring>
    {
        public override void Bake(SamplePointRendererAuthoring src)
        {
            var data = new SamplePointRenderer()
              { Mesh = src.Mesh, Material = src.Material };

            AddComponentObject(GetEntity(TransformUsageFlags.None), data);

            var grid = GetComponent<GridConfigAuthoring>();

            for (var x = 0u; x < grid.Dimensions.x; x++)
            {
                for (var y = 0u; y < grid.Dimensions.y; y++)
                {
                    var add = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddComponent(add, new PixelCoords() { Value = math.uint2(x, y) });
                    AddComponent(add, new SamplePoint() { Index = 0 });
                }
            }
        }
    }
}
