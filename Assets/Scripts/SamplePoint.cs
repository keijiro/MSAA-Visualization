using Unity.Entities;
using Unity.Mathematics;

public struct SamplePoint : IComponentData
{
    public uint Level;
    public float3 Position;
}

public struct GridLine : IComponentData
{
    public bool IsVertical;
    public uint Index;
}
