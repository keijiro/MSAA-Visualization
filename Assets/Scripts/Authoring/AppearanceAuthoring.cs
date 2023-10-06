using UnityEngine;
using Unity.Entities;

public class AppearanceAuthoring : MonoBehaviour
{
    public float GridLineBoldness = 0.01f;
    public float GridLineDepth = 1;
    public float SamplePointRadius = 0.1f;
    [Space]
    [Range(0, 3)] public float ActiveLayer = 0;
    public Appearance.Source SampleSource;
    [Space]
    [Range(0, 1)] public float GridLineParam = 0;
    [Range(0, 2)] public float SamplePointParam = 0;
    [Range(0, 1)] public float SamplePointSnap = 1;
    [Range(0, 2)] public float PixelParam = 0;
    [Range(0, 1)] public float TriangleParam = 0;

    class Baker : Baker<AppearanceAuthoring>
    {
        public override void Bake(AppearanceAuthoring src)
          => AddComponent
               (GetEntity(TransformUsageFlags.None),
                new Appearance
                  { GridLineBoldness = src.GridLineBoldness,
                    GridLineDepth = src.GridLineDepth,
                    GridLineParam = src.GridLineParam,
                    SamplePointRadius = src.SamplePointRadius,
                    SamplePointParam = src.SamplePointParam,
                    SamplePointSnap = src.SamplePointSnap,
                    SampleSource = src.SampleSource,
                    PixelParam = src.PixelParam,
                    TriangleParam = src.TriangleParam,
                    ActiveLayer = src.ActiveLayer });
    }
}
