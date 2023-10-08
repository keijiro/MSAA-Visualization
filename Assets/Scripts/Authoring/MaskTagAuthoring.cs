using UnityEngine;
using Unity.Entities;

public class MaskTagAuthoring : MonoBehaviour
{
    public Color Color = Color.black;

    class Baker : Baker<MaskTagAuthoring>
    {
        public override void Bake(MaskTagAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None),
                          new MaskTag(){ Color = src.Color });
    }
}
