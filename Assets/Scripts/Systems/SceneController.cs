using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using System;

public partial class SceneControllerSystem : SystemBase
{
    protected override void OnCreate()
      => ControllerFunction();

    protected override void OnUpdate() {}

    Appearance GlobalAppearance {
        get => SystemAPI.GetSingleton<Appearance>();
        set => SystemAPI.SetSingleton<Appearance>(value);
    }

    async Awaitable Linear
      (float start, float end, float duration, Action<float> action)
    {
        var t = 0.0f;
        action.Invoke(start);
        while (true)
        {
            await Awaitable.NextFrameAsync();
            t += SystemAPI.Time.DeltaTime;
            if (t >= duration) break;
            action.Invoke(math.lerp(start, end, t / duration));
        }
        action.Invoke(end);
    }

    async void ControllerFunction()
    {
        while (!SystemAPI.HasSingleton<Appearance>())
            await Awaitable.NextFrameAsync();

        var appear = GlobalAppearance;
        appear.GridLineParam = 0;
        appear.SamplePointParam = 0;
        GlobalAppearance = appear;

        await Linear(0, 2.6f, 1.5f, (x) => {
            appear.GridLineParam = x;
            GlobalAppearance = appear;
        });

        await Linear(0, 6.6f, 1.5f, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });
    }
}
