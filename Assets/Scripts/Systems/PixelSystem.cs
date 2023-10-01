using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial struct PixelUpdaetSystem : ISystem
{
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<GridSpace>();

    public void OnUpdate(ref SystemState state)
    {
        var space = SystemAPI.GetSingleton<GridSpace>() ;

        var buffer = new NativeArray<float>
          (4 * space.GetElementCount(), Allocator.TempJob);

        var job1 = new PixelAccumulationJob()
           { Space = space, Buffer = buffer };

        var job2 = new PixelUpdateJob()
           { Space = space, Buffer = buffer };

        state.Dependency = job1.Schedule(state.Dependency);
        state.Dependency = job2.Schedule(state.Dependency);
        state.Dependency = buffer.Dispose(state.Dependency);
    }

}

[BurstCompile]
partial struct PixelAccumulationJob : IJobEntity
{
    public GridSpace Space;
    public NativeArray<float> Buffer;

    void Execute(in Layer layer,
                 in PixelCoords coords,
                 in SamplePoint point,
                 in SampleResult result)
      => Buffer[CoordUtil.PixelToIndex(Space, layer, coords)]
           += result.Hit ? 1.0f / math.pow(2, layer.Index) : 0;
}

[BurstCompile]
partial struct PixelUpdateJob : IJobEntity
{
    public GridSpace Space;
    [ReadOnly] public NativeArray<float> Buffer;

    void Execute(in Layer layer,
                 in PixelCoords coords,
                 ref Pixel pixel)
      => pixel.Coverage = Buffer[CoordUtil.PixelToIndex(Space, layer, coords)];
}
