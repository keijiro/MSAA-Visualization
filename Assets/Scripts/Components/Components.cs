using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#region Global configuration

public struct SceneConfig : IComponentData
{
    public uint Sequence;
}

public struct GridSpace : IComponentData
{
    public uint2 Dimensions;
}

#endregion

#region Screen elements

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

public struct SampleResult : IComponentData
{
    public bool Hit;
}

public struct Pixel : IComponentData
{
    public float Coverage;
}

public struct Triangle : IComponentData
{
    public float2 Vertex1;
    public float2 Vertex2;
    public float2 Vertex3;
}

#endregion

#region Rendering

public struct RenderElement : IComponentData
{
    public enum ElementType { None, Point, Quad }
    public ElementType Type;
    public float2 Position;
    public float Depth;
    public float Angle;
    public float2 Scale;
    public Color Color;
}

public struct Appearance : IComponentData
{
    public enum Source { Triangle, Threshold, Gradient }
    public float GridLineBoldness;
    public float GridLineDepth;
    public float GridLineParam;
    public float SamplePointRadius;
    public float SamplePointSnap;
    public float SamplePointParam;
    public Source SampleSource;
    public float PixelParam;
    public float TriangleParam;
    public float ActiveLayer;
}

public struct ColorScheme : IComponentData
{
    public Color HitColor;
    public Color MissColor;
    public Color FocusColor;
    public Color LineColor;
    public Color PixelColor;
    public Color TriangleColor;
}

public class RenderingAssets : IComponentData
{
    public Mesh PointMesh;
    public Mesh QuadMesh;
    public Mesh TriangleMesh;
    public Material PointMaterial;
    public Material QuadMaterial;
    public Material TriangleMaterial;
}

#endregion
