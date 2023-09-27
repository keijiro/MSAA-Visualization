using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class SamplePointRenderingAuthoring : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;
    public float Radius = 0.1f;
    public float CurrentLayer = 0;

    class Baker : Baker<SamplePointRenderingAuthoring>
    {
        public override void Bake(SamplePointRenderingAuthoring src)
        {
            var data = new SamplePointRendering()
              { Mesh = src.Mesh,
                Material = src.Material,
                Radius = src.Radius,
                CurrentLayer = src.CurrentLayer };

            AddComponentObject(GetEntity(TransformUsageFlags.None), data);

            var grid = GetComponent<GridConfigAuthoring>();

            for (var layer = 0u; layer < 4; layer++)
            {
                for (var x = 0u; x < grid.Dimensions.x; x++)
                {
                    for (var y = 0u; y < grid.Dimensions.y; y++)
                    {
                        for (var i = 0u; i < math.pow(2, layer); i++)
                        {
                            var e = CreateAdditionalEntity(TransformUsageFlags.None);
                            AddComponent(e, new PixelCoords(){ Value = math.uint2(x, y) });
                            AddComponent(e, new SamplePoint(){ Index = i });
                            AddComponent(e, new Layer(){ Index = layer });
                        }
                    }
                }
            }
        }
    }
}