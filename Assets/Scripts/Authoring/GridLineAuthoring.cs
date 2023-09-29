using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[RequireComponent(typeof(GridSpaceAuthoring))]
public class GridLineAuthoring : MonoBehaviour
{
    class Baker : Baker<GridLineAuthoring>
    {
        public override void Bake(GridLineAuthoring src)
        {
            var space = GetComponent<GridSpaceAuthoring>();

            for (var i = 0u; i <= space.Dimensions.x; i++)
            {
                var e = CreateAdditionalEntity(TransformUsageFlags.None);
                AddComponent(e, new GridLine(){ IsVertical = false, Index = i });
            }

            for (var i = 0u; i <= space.Dimensions.y; i++)
            {
                var e = CreateAdditionalEntity(TransformUsageFlags.None);
                AddComponent(e, new GridLine(){ IsVertical = true, Index = i });
            }
        }
    }
}
