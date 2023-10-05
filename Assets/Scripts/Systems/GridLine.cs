using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[WorldSystemFilter
 (WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial struct GridLineUpdateSystem : ISystem
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
        var job = new GridLineUpdateJob
        {
            Space = SystemAPI.GetSingleton<GridSpace>(),
            Appear = SystemAPI.GetSingleton<Appearance>(),
            Colors = SystemAPI.GetSingleton<ColorScheme>()
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
partial struct GridLineUpdateJob : IJobEntity
{
    public GridSpace Space;
    public Appearance Appear;
    public ColorScheme Colors;

    void Execute(in GridLine line, ref RenderElement render)
    {
        const float delay = 0.2f;
        var anim = (float2)Appear.GridLineParam;
        anim *= 1 + (float2)Space.Dimensions * delay;
        anim -=(float2)line.Index * delay;
        anim = MathUtil.smootherstep(anim);

        var pos = line.Index - (float2)Space.Dimensions * 0.5f;
        var scale = Space.Dimensions * anim;

        if (!line.IsVertical)
        {
            pos.x = 0;
            scale.y = Appear.GridLineBoldness;
        }
        else
        {
            pos.y = 0;
            scale.x = Appear.GridLineBoldness;
        }

        render = new RenderElement
        {
            Type = RenderElement.ElementType.Quad,
            Position = pos,
            Depth = Appear.GridLineDepth,
            Angle = 0,
            Scale = scale,
            Color = Colors.LineColor
        };
    }
}
