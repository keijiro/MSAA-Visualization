using Unity.Entities;
using Unity.Mathematics;

public struct SamplePointOptions : IComponentData
{
    public float PointRadius;
    public uint RowCount;
}

public struct SamplePoint : IComponentData
{
    public uint2 Indices;
}
