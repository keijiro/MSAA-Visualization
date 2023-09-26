using UnityEngine;
using Unity.Entities;

public struct SamplePointConfig : IComponentData
{
    public float Radius;
}

public class SamplePointConfigAuthoring : MonoBehaviour
{
    public float Radius = 0.1f;

    class Baker : Baker<SamplePointConfigAuthoring>
    {
        public override void Bake(SamplePointConfigAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None),
                          new SamplePointConfig(){ Radius = src.Radius });
    }
}
