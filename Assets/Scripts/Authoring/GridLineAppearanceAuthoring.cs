using UnityEngine;
using Unity.Entities;

public class GridLineAppearanceAuthoring : MonoBehaviour
{
    public float Boldness = 0.01f;
    public float Depth = 0;

    class Baker : Baker<GridLineAppearanceAuthoring>
    {
        public override void Bake(GridLineAppearanceAuthoring src)
          => AddComponent
               (GetEntity(TransformUsageFlags.None),
                new GridLineAppearance()
                  { Boldness = src.Boldness,
                    Depth = src.Depth });
    }
}
