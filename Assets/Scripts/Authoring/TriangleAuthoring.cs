using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class TriangleAuthoring : MonoBehaviour
{
    public float2 Vertex1 = math.float2(0, 0);
    public float2 Vertex2 = math.float2(1, 0);
    public float2 Vertex3 = math.float2(0, 1);

    class Baker : Baker<TriangleAuthoring>
    {
        public override void Bake(TriangleAuthoring src)
          => AddComponent(GetEntity(TransformUsageFlags.None),
                          new Triangle(){ Vertex1 = src.Vertex1,
                                          Vertex2 = src.Vertex2,
                                          Vertex3 = src.Vertex3 });
    }
}
