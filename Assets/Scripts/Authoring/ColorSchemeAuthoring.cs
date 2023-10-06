using UnityEngine;
using Unity.Entities;

public class ColorSchemeAuthoring : MonoBehaviour
{
    public Color HitColor = Color.red;
    public Color MissColor = Color.blue;
    public Color FocusColor = Color.yellow;
    public Color LineColor = Color.gray;
    public Color PixelColor = Color.red;
    public Color TriangleColor = Color.gray;

    class Baker : Baker<ColorSchemeAuthoring>
    {
        public override void Bake(ColorSchemeAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None), 
                          new ColorScheme
                            { HitColor = src.HitColor,
                              MissColor = src.MissColor,
                              FocusColor = src.FocusColor,
                              LineColor = src.LineColor,
                              PixelColor = src.PixelColor,
                              TriangleColor = src.TriangleColor });
    }
}
