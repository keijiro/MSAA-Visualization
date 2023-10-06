using UnityEngine;
using Unity.Entities;

public class SceneConfigAuthoring : MonoBehaviour
{
    public uint Sequence;

    class Baker : Baker<SceneConfigAuthoring>
    {
        public override void Bake(SceneConfigAuthoring src)
          => AddComponent
               (GetEntity(TransformUsageFlags.None),
                new SceneConfig{ Sequence = src.Sequence });
    }
}
