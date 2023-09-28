using UnityEngine;
using Unity.Entities;

public class ColorSchemeAuthoring : MonoBehaviour
{
    public Color HitColor = Color.red;
    public Color MissColor = Color.blue;
    public Color LineColor = Color.gray;

    class Baker : Baker<ColorSchemeAuthoring>
    {
        public override void Bake(ColorSchemeAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None), 
                          new ColorScheme()
                            { HitColor = src.HitColor,
                              MissColor = src.MissColor,
                              LineColor = src.LineColor });
    }
}
