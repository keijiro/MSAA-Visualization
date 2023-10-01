using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
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
        appear.PixelParam = 0;
        appear.TriangleParam = 0;
        GlobalAppearance = appear;

        var label = GameObject.Find("Label").GetComponent<Text>();
        label.text = "";

        await Linear(0, 2.6f, 1.5f, (x) => {
            appear.GridLineParam = x;
            GlobalAppearance = appear;
        });

        for (var layer = 0; layer < 4; layer++)
        {
            appear.ActiveLayer = layer;

            await Linear(0, 1, 0.5f, (x) => {
                appear.TriangleParam = x;
                GlobalAppearance = appear;
            });

            await Awaitable.WaitForSecondsAsync(0.3f);

            label.text = layer == 0 ?
              "No AA" : $"MSAA x{math.pow(2, layer)}";

            await Linear(0, 6.6f, 1.5f, (x) => {
                appear.SamplePointParam = x;
                GlobalAppearance = appear;
            });

            await Awaitable.WaitForSecondsAsync(0.3f);

            await Linear(1, 0, 0.5f, (x) => {
                appear.TriangleParam = x;
                GlobalAppearance = appear;
            });

            await Awaitable.WaitForSecondsAsync(1);

            await Linear(0, 1, 0.5f, (x) => {
                appear.PixelParam = x;
                appear.SamplePointParam = 10 + x;
                GlobalAppearance = appear;
            });

            await Awaitable.WaitForSecondsAsync(2);

            label.text = "";

            await Linear(0, 1, 0.5f, (x) => {
                appear.PixelParam = 1 - x;
                GlobalAppearance = appear;
            });

            await Awaitable.WaitForSecondsAsync(0.5f);
        }
    }
}
