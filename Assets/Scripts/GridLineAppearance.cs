using UnityEngine;
using Unity.Entities;

public class GridLineAppearanceAuthoring : MonoBehaviour
{
    public float Boldness = 0.01f;

    class Baker : Baker<GridLineAppearanceAuthoring>
    {
        public override void Bake(GridLineAppearanceAuthoring src)
          => AddComponent
               (GetEntity(TransformUsageFlags.None),
                new GridLineAppearance(){ Boldness = src.Boldness });
    }
}
