using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[RequireComponent(typeof(GridSpaceAuthoring))]
public class SamplePointAuthoring : MonoBehaviour
{
    class Baker : Baker<SamplePointAuthoring>
    {
        public override void Bake(SamplePointAuthoring src)
        {
            var space = GetComponent<GridSpaceAuthoring>();
            for (var layer = 0u; layer < 4; layer++)
                for (var x = 0u; x < space.Dimensions.x; x++)
                    for (var y = 0u; y < space.Dimensions.y; y++)
                        for (var i = 0u; i < math.pow(2, layer); i++)
                            AddSamplePoint(layer, x, y, i);
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
