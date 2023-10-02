using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class GridSpaceAuthoring : MonoBehaviour
{
    public uint2 Dimensions = 4;

    class Baker : Baker<GridSpaceAuthoring>
    {
        public override void Bake(GridSpaceAuthoring src)
        {
            // GridSpace component
            AddComponent(GetEntity(TransformUsageFlags.None),
                         new GridSpace(){ Dimensions = src.Dimensions });

            // Additional entities: GridLine (horizontal)
            for (var i = 0u; i <= src.Dimensions.x; i++) AddGridLine(i, false);

            // Additional entities: GridLine (vertical)
            for (var i = 0u; i <= src.Dimensions.y; i++) AddGridLine(i, true);

            // Additional entities: Pixel & SamplePoint
            for (var layer = 0u; layer < 4; layer++)
                for (var x = 0u; x < src.Dimensions.x; x++)
                    for (var y = 0u; y < src.Dimensions.y; y++)
                        AddPixelAndSamplePoint(layer, x, y);
        }

        void AddGridLine(uint index, bool isVertical)
        {
            var e = CreateAdditionalEntity(TransformUsageFlags.None);
            AddComponent(e, new GridLine(){ IsVertical = isVertical, Index = index });
        }

        void AddPixelAndSamplePoint(uint layer, uint x, uint y)
        {
            AddPixel(layer, x, y);
            for (var i = 0u; i < math.pow(2, layer); i++)
                AddSamplePoint(layer, x, y, i);
        }

        void AddPixel(uint layer, uint x, uint y)
        {
            var e = CreateAdditionalEntity(TransformUsageFlags.None);
            AddComponent(e, new Layer(){ Index = layer });
            AddComponent(e, new PixelCoords(){ Value = math.uint2(x, y) });
            AddComponent(e, new Pixel());
        }

        void AddSamplePoint(uint layer, uint x, uint y, uint index)
        {
            var e = CreateAdditionalEntity(TransformUsageFlags.None);
            AddComponent(e, new Layer(){ Index = layer });
            AddComponent(e, new PixelCoords(){ Value = math.uint2(x, y) });
            AddComponent(e, new SamplePoint(){ Index = index });
            AddComponent(e, new SampleResult());
        }
    }
}
