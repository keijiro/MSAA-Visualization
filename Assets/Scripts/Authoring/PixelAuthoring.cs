using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[RequireComponent(typeof(GridSpaceAuthoring))]
public class PixelAuthoring : MonoBehaviour
{
    class Baker : Baker<PixelAuthoring>
    {
        public override void Bake(PixelAuthoring src)
        {
            var space = GetComponent<GridSpaceAuthoring>();
            for (var layer = 0u; layer < 4; layer++)
                for (var x = 0u; x < space.Dimensions.x; x++)
                    for (var y = 0u; y < space.Dimensions.y; y++)
                        AddPixel(layer, x, y);
        }

        void AddPixel(uint layer, uint x, uint y)
        {
            var e = CreateAdditionalEntity(TransformUsageFlags.None);
            AddComponent(e, new Layer(){ Index = layer });
            AddComponent(e, new PixelCoords(){ Value = math.uint2(x, y) });
            AddComponent(e, new Pixel());
        }
    }
}
