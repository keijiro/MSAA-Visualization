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
        var pos = line.Index - (float2)Space.Dimensions * 0.5f;
        //var scale = (float2)Space.Dimensions;// * Appear.GridLineParam;
        var scale = (float2)Appear.GridLineParam;
        scale *= (1 + (float2)Space.Dimensions * 0.2f);

        scale -= (float2)line.Index * 0.2f;
        scale.x = MathUtil.smootherstep(scale.x);
        scale.y = MathUtil.smootherstep(scale.y);
        scale *= Space.Dimensions;

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

        /*
        var pos = (float2)0;
        var scale = (float2)Appear.GridLineBoldness;

        //var speed = math.float2(Space.Dimensions) * 0.2f + 1;
        //var anim = Appear.GridLineParam * speed - line.Index * 0.2f;
        //anim = MathUtil.smootherstep(anim);

        if (!line.IsVertical)
        {
            pos.y = line.Index - Space.Dimensions.y * 0.5f;
            scale.x = Space.Dimensions.x * anim;
        }
        else
        {
            pos.x = line.Index - Space.Dimensions.x * 0.5f;
            scale.y = Space.Dimensions.y * anim;
        }
        */

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
