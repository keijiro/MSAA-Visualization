using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using System;

public partial class SceneControllerSystem : SystemBase
{
    protected override void OnCreate()
      => ControllerFunctionFragments();

    protected override void OnUpdate() {}

    Appearance GlobalAppearance {
        get => SystemAPI.GetSingleton<Appearance>();
        set => SystemAPI.SetSingleton<Appearance>(value);
    }

    ColorScheme GlobalColors {
        get => SystemAPI.GetSingleton<ColorScheme>();
        set => SystemAPI.SetSingleton<ColorScheme>(value);
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

    async void ControllerFunctionBasics()
    {
        while (!SystemAPI.HasSingleton<Appearance>())
            await Awaitable.NextFrameAsync();

        var appear = GlobalAppearance;
        appear.GridLineParam = 0;
        appear.SamplePointParam = 0;
        appear.PixelParam = 0;
        appear.TriangleParam = 0;
        GlobalAppearance = appear;

        await Linear(0, 1, 1.5f, (x) => {
            appear.GridLineParam = x;
            GlobalAppearance = appear;
        });

        // No MSAA

        appear.ActiveLayer = 0;

        await Linear(0, 1, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(0, 1, 1.5f, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(1, 0, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(0, 1, 1.5f, (x) => {
            appear.PixelParam = x;
            appear.SamplePointParam = 1 + x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(1, 2, 1.5f, (x) => {
            appear.PixelParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        // x2 MSAA

        appear.ActiveLayer = 1;

        await Linear(0, 1, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(0, 1, 1.5f, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(1, 0, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(0, 1, 1.5f, (x) => {
            appear.PixelParam = x;
            appear.SamplePointParam = 1 + x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(1);

        // x4 MSAA

        await Linear(0, 1, 1.5f, (x) => {
            appear.ActiveLayer = 1 + x;
            appear.SamplePointParam = x;
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(1);

        // x8 MSAA

        await Linear(0, 1, 1.5f, (x) => {
            appear.ActiveLayer = 2 + x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(1, 2, 1.5f, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(1, 0, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });
    }

    async void ControllerFunctionFragments()
    {
        while (!SystemAPI.HasSingleton<Appearance>())
            await Awaitable.NextFrameAsync();

        var appear = GlobalAppearance;
        var colors = GlobalColors;
        var palette = colors;

        appear.ActiveLayer = 2;
        appear.GridLineParam = 1;
        appear.SamplePointSnap = 1;
        appear.PixelParam = 1;
        appear.TriangleParam = 1;
        colors.HitColor = palette.MissColor;
        colors.PixelColor.a = 0;

        GlobalAppearance = appear;
        GlobalColors = colors;

        await Linear(0, 1, 1, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(1, 0, 0.2f, (x) => {
            colors.HitColor.a = colors.MissColor.a = x;
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        appear.SamplePointSnap = 0;
        GlobalAppearance = appear;

        colors.HitColor = palette.HitColor;

        await Linear(0, 1, 0.2f, (x) => {
            colors.HitColor.a =  x;
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(0, 1, 0.5f, (x) => {
            appear.SamplePointSnap = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(1, 0, 0.5f, (x) => {
            appear.SamplePointSnap = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Linear(0, 1, 0.2f, (x) => {
            appear.TriangleParam = 1 - x;
            colors.HitColor.a = 1 - x;
            colors.PixelColor.a = x;
            GlobalAppearance = appear;
            GlobalColors = colors;
        });
    }
}
