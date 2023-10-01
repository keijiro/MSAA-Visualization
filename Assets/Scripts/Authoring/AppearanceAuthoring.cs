using UnityEngine;
using Unity.Entities;

public class AppearanceAuthoring : MonoBehaviour
{
    public float GridLineBoldness = 0.01f;
    public float GridLineDepth = 1;
    public float SamplePointRadius = 0.1f;
    public float ActiveLayer = 0;

    class Baker : Baker<AppearanceAuthoring>
    {
        public override void Bake(AppearanceAuthoring src)
          => AddComponent
               (GetEntity(TransformUsageFlags.None),
                new Appearance()
                  { GridLineBoldness = src.GridLineBoldness,
                    GridLineDepth = src.GridLineDepth,
                    SamplePointRadius = src.SamplePointRadius,
                    ActiveLayer = src.ActiveLayer });
    }
}
