using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial struct SamplePointUpdateSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GridSpace>();
        state.RequireForUpdate<Appearance>();
        state.RequireForUpdate<ColorScheme>();
        state.RequireForUpdate<Triangle>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new SamplePointUpdateJob
        {
            Space = SystemAPI.GetSingleton<GridSpace>(),
            Appear = SystemAPI.GetSingleton<Appearance>(),
            Colors = SystemAPI.GetSingleton<ColorScheme>(),
            Triangle = SystemAPI.GetSingleton<Triangle>()
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
partial struct SamplePointUpdateJob : IJobEntity
{
    public GridSpace Space;
    public Appearance Appear;
    public ColorScheme Colors;
    public Triangle Triangle;

    void Execute(in Layer layer,
                 in PixelCoords coords,
                 in SamplePoint point,
                 ref SampleResult result,
                 ref RenderElement render)
    {
        var pos = point.GetPosition(layer, coords);
        var hit = TriangleUtil.TestPoint(pos, Triangle);

        var color = hit ? Colors.HitColor : Colors.MissColor;
        color = LayerUtil.ApplyAlpha(color, layer, Appear.ActiveLayer);
        color.a *= math.saturate(1 - (Appear.SamplePointParam - 10));

        var anim = Appear.SamplePointParam;
        anim -= math.dot(coords.Value, math.float2(0.2f, 0.6f));
        anim = MathUtil.smootherstep(anim);

        result = new SampleResult{ Hit = hit };

        render = new RenderElement
        {
            Type = RenderElement.ElementType.Point,
            Position = CoordUtil.GridToScreen(Space, pos),
            Depth = 0,
            Angle = 0,
            Scale = Appear.SamplePointRadius * anim,
            Color = color
        };
    }
}
