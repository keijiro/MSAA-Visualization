using UnityEngine;
using Unity.Entities;

public class TriangleRenderingAuthoring : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;

    class Baker : Baker<TriangleRenderingAuthoring>
    {
        public override void Bake(TriangleRenderingAuthoring src)
          => AddComponentObject(GetEntity(TransformUsageFlags.None), 
                                new TriangleRendering()
                                { Mesh = src.Mesh, Material = src.Material });
    }
}
