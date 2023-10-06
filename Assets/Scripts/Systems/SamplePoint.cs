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
        var job1 = new SamplePointUpdateJob
        {
            Triangle = SystemAPI.GetSingleton<Triangle>()
        };

        var job2 = new SamplePointRenderElementJob
        {
            Space = SystemAPI.GetSingleton<GridSpace>(),
            Appear = SystemAPI.GetSingleton<Appearance>(),
            Colors = SystemAPI.GetSingleton<ColorScheme>(),
            Triangle = SystemAPI.GetSingleton<Triangle>()
        };

        state.Dependency = job1.Schedule(state.Dependency);
        state.Dependency = job2.Schedule(state.Dependency);
    }
}

[BurstCompile]
partial struct SamplePointUpdateJob : IJobEntity
{
    public Triangle Triangle;

    void Execute(in Layer layer,
                 in PixelCoords coords,
                 in SamplePoint point,
                 ref SampleResult result)
    {
        var pos = point.GetPosition(layer, coords);
        var hit = TriangleUtil.TestPoint(pos, Triangle);
        result = new SampleResult{ Hit = hit };
    }
}

[BurstCompile]
partial struct SamplePointRenderElementJob : IJobEntity
{
    public GridSpace Space;
    public Appearance Appear;
    public ColorScheme Colors;
    public Triangle Triangle;

    void Execute(in Layer layer,
                 in PixelCoords coords,
                 in SamplePoint point,
                 in SampleResult result,
                 ref RenderElement render)
    {
        var pos = point.GetPosition(layer, coords);
        var center = point.GetPosition(new Layer{Index = 0}, coords);
        var snap = MathUtil.smootherstep(Appear.SamplePointSnap);
        pos = math.lerp(center, pos, snap);

        var color = result.Hit ? Colors.HitColor : Colors.MissColor;
        color = LayerUtil.ApplyAlpha(color, layer, Appear.ActiveLayer);

        var scale = Appear.SamplePointRadius;
        scale *= AnimUtil.Scan(Space, coords, Appear.SamplePointParam);

        render = new RenderElement
        {
            Type = RenderElement.ElementType.Point,
            Position = CoordUtil.GridToScreen(Space, pos),
            Depth = 0,
            Angle = 0,
            Scale = scale,
            Color = color
        };
    }
}
