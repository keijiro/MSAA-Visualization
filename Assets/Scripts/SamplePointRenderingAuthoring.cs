using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class SamplePointRenderingAuthoring : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;
    public float Radius = 0.1f;

    class Baker : Baker<SamplePointRenderingAuthoring>
    {
        public override void Bake(SamplePointRenderingAuthoring src)
        {
            var data = new SamplePointRendering()
              { Mesh = src.Mesh,
                Material = src.Material,
                Radius = src.Radius};

            AddComponentObject(GetEntity(TransformUsageFlags.None), data);

            var grid = GetComponent<GridConfigAuthoring>();

            for (var x = 0u; x < grid.Dimensions.x; x++)
            {
                for (var y = 0u; y < grid.Dimensions.y; y++)
                {
                    var e = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddComponent(e, new PixelCoords(){ Value = math.uint2(x, y) });
                    AddComponent(e, new SamplePoint(){ Index = 0 });
                    AddComponent(e, new Layer(){ Index = 0 });
                }
            }
        }
    }
}
