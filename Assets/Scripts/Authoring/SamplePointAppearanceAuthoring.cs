using UnityEngine;
using Unity.Entities;

public class SamplePointAppearanceAuthoring : MonoBehaviour
{
    public float Radius = 0.1f;
    [Range(0, 3)] public float CurrentLayer = 0;

    class Baker : Baker<SamplePointAppearanceAuthoring>
    {
        public override void Bake(SamplePointAppearanceAuthoring src)
          => AddComponent
               (GetEntity(TransformUsageFlags.None),
                new SamplePointAppearance()
                  { Radius = src.Radius,
                    CurrentLayer = src.CurrentLayer });
    }
}
