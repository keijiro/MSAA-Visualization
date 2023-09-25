using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SamplePointUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<SamplePointOptions>()) return;
        new SamplePointUpdateJob()
          { Options = SystemAPI.GetSingleton<SamplePointOptions>() }
          .ScheduleParallel();
    }
}

[BurstCompile]
partial struct SamplePointUpdateJob : IJobEntity
{
    public SamplePointOptions Options;

    void Execute(in SamplePoint point,
                 ref LocalToWorld l2w)
    {
        var p = math.float2(point.Indices) - (Options.RowCount - 1) * 0.5f;
        var s = Options.PointRadius;
        l2w.Value = float4x4.TRS(math.float3(p, 0), quaternion.identity, s);
    }
}
