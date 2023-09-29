using Unity.Burst;
using Unity.Entities;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial struct SamplePointTriangleTestSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Triangle>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
      => new SamplePointTriangleTestJob()
           { Triangle = SystemAPI.GetSingleton<Triangle>() }
           .ScheduleParallel();
}

[BurstCompile]
partial struct SamplePointTriangleTestJob : IJobEntity
{
    public Triangle Triangle;

    void Execute(in Layer layer,
                 in PixelCoords coords,
                 in SamplePoint point,
                 ref SampleResult result)
    {
        var pos = point.GetPosition(layer, coords);
        result.Hit = TriangleUtil.TestPoint(pos, Triangle);
    }
}
