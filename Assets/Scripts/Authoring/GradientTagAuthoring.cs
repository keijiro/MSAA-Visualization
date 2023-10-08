using UnityEngine;
using Unity.Entities;

public class GradientTagAuthoring : MonoBehaviour
{
    public Color Color = Color.white;

    class Baker : Baker<GradientTagAuthoring>
    {
        public override void Bake(GradientTagAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None),
                          new GradientTag(){ Color = src.Color });
    }
}
