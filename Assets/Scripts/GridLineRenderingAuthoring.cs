using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class GridLineRenderingAuthoring : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;

    class Baker : Baker<GridLineRenderingAuthoring>
    {
        public override void Bake(GridLineRenderingAuthoring src)
        {
            var data = new GridLineRendering()
              { Mesh = src.Mesh,
                Material = src.Material };

            AddComponentObject(GetEntity(TransformUsageFlags.None), data);

            var grid = GetComponent<GridConfigAuthoring>();

            for (var i = 0u; i <= grid.Dimensions.x; i++)
            {
                var e = CreateAdditionalEntity(TransformUsageFlags.None);
                AddComponent(e, new GridLine(){ IsVertical = false, Index = i });
            }

            for (var i = 0u; i <= grid.Dimensions.y; i++)
            {
                var e = CreateAdditionalEntity(TransformUsageFlags.None);
                AddComponent(e, new GridLine(){ IsVertical = true, Index = i });
            }
        }
    }
}
