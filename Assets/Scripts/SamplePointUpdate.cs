using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SamplePointUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
      => new SamplePointUpdateJob().ScheduleParallel();
}

[BurstCompile]
partial struct SamplePointUpdateJob : IJobEntity
{
    void Execute(in SamplePoint point,
                 ref LocalToWorld l2w)
    {
        var pos = math.float3(math.float2(point.Indices) * 0.1f, 0);
        var mtx = float4x4.TRS(pos, quaternion.identity, 0.01f);
        l2w.Value = mtx;
    }
}
