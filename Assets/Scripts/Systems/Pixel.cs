using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial struct PixelUpdateSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GridSpace>();
        state.RequireForUpdate<Appearance>();
        state.RequireForUpdate<ColorScheme>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var space = SystemAPI.GetSingleton<GridSpace>() ;
        var appear = SystemAPI.GetSingleton<Appearance>();
        var colors = SystemAPI.GetSingleton<ColorScheme>();

        var buffer = new NativeArray<float>
          (4 * space.GetElementCount(), Allocator.TempJob);

        var job1 = new PixelAccumulationJob
          { Space = space, Buffer = buffer };

        var job2 = new PixelUpdateJob
          { Space = space, Appear = appear, Colors = colors, Buffer = buffer };

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
    public Appearance Appear;
    public ColorScheme Colors;

    [ReadOnly] public NativeArray<float> Buffer;

    void Execute(in Layer layer,
                 in PixelCoords coords,
                 ref Pixel pixel,
                 ref RenderElement render)
    {
        var cover = Buffer[CoordUtil.PixelToIndex(Space, layer, coords)];

        var color = Colors.PixelColor;
        color = LayerUtil.ApplyAlpha(color, layer, Appear.ActiveLayer);
        color.a *= cover * AnimUtil.Scan(Space, coords, Appear.PixelParam);

        var pos = math.float2(coords.Value) + 0.5f;

        pixel = new Pixel{ Coverage = cover };

        render = new RenderElement
        {
            Type = RenderElement.ElementType.Quad,
            Position = CoordUtil.GridToScreen(Space, pos),
            Depth = Appear.GridLineDepth,
            Angle = 0,
            Scale = 1,
            Color = color
        };
    }
}
