using Unity.Entities;
using Unity.Mathematics;

public struct GridConfig : IComponentData
{
    public uint2 Dimensions;
}

public struct Layer : IComponentData
{
    public uint Index;
}

public struct PixelCoords : IComponentData
{
    public uint2 Value;
}

public struct SamplePoint : IComponentData
{
    public uint Index;
}
