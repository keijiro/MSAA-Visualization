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

public struct Triangle : IComponentData
{
    public float2 Vertex1;
    public float2 Vertex2;
    public float2 Vertex3;
}

public struct GridLine : IComponentData
{
    public bool IsVertical;
    public uint Index;
}

public struct GridLineAppearance : IComponentData
{
    public float Boldness;
}

public struct SamplePoint : IComponentData
{
    public uint Index;
}

public struct SamplePointAppearance : IComponentData
{
    public float Radius;
    public float CurrentLayer;
}

public struct SampleResult : IComponentData
{
    public bool Hit;
}

public struct ColorScheme : IComponentData
{
    public Color HitColor;
    public Color MissColor;
    public Color LineColor;
    public Color TriangleColor;
}

public class RenderingAssets : IComponentData
{
    public Mesh PointMesh;
    public Mesh LineMesh;
    public Mesh TriangleMesh;
    public Material PointMaterial;
    public Material LineMaterial;
    public Material TriangleMaterial;
}
