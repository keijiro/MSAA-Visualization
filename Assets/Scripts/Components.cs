using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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

public struct GridLine : IComponentData
{
    public bool IsVertical;
    public uint Index;
}

public struct SamplePoint : IComponentData
{
    public uint Index;
}

public class GridLineRendering : IComponentData
{
    public Mesh Mesh;
    public Material Material;
}

public class SamplePointRendering : IComponentData
{
    public Mesh Mesh;
    public Material Material;
    public float Radius;
}
