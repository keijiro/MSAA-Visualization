using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class SceneControllerSystem : SystemBase
{
    #region System implementation

    protected override void OnCreate() => RunControllerAsync();
    protected override void OnUpdate() {}

    #endregion

    #region Singleton accessors

    Appearance GlobalAppearance {
        get => SystemAPI.GetSingleton<Appearance>();
        set => SystemAPI.SetSingleton<Appearance>(value);
    }

    ColorScheme GlobalColors {
        get => SystemAPI.GetSingleton<ColorScheme>();
        set => SystemAPI.SetSingleton<ColorScheme>(value);
    }

    #endregion

    #region Material override control

    void SetGradientOpacity(float x)
      => Entities.ForEach((ref GradientTag tag) =>
                           tag.Color = new Color(1, 1, 1, x)).Run();

    void SetMaskOpacity(float x)
      => Entities.ForEach((ref MaskTag tag) =>
                          tag.Color = new Color(0, 0, 0, x)).Run();

    #endregion

    #region Interpolators

    async Awaitable Tween
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

    #endregion

    #region Async controllers

    async void RunControllerAsync()
    {
        // Synchronization with singleton components
        while (!SystemAPI.HasSingleton<SceneConfig>() ||
               !SystemAPI.HasSingleton<Appearance>() ||
               !SystemAPI.HasSingleton<ColorScheme>())
            await Awaitable.NextFrameAsync();

        // Sequence startup
        switch (SystemAPI.GetSingleton<SceneConfig>().Sequence)
        {
            case 0: await RunSequence1Async(); break;
            case 1: await RunSequence2Async(); break;
            case 2: await RunSequence3Async(); break;
        }
    }

    async Awaitable RunSequence1Async()
    {
        var appear = GlobalAppearance;
        appear.GridLineParam = 0;
        appear.SamplePointParam = 0;
        appear.PixelParam = 0;
        appear.TriangleParam = 0;
        GlobalAppearance = appear;

        await Tween(0, 1, 1.5f, (x) => {
            appear.GridLineParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        // No MSAA

        appear.ActiveLayer = 0;

        await Tween(0, 1, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 1.5f, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 0, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 1.5f, (x) => {
            appear.PixelParam = x;
            appear.SamplePointParam = 1 + x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 2, 1.5f, (x) => {
            appear.PixelParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        // x2 MSAA

        appear.ActiveLayer = 1;

        await Tween(0, 1, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 1.5f, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 0, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 1.5f, (x) => {
            appear.PixelParam = x;
            appear.SamplePointParam = 1 + x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(1);

        // x4 MSAA

        await Tween(0, 1, 1.5f, (x) => {
            appear.ActiveLayer = 1 + x;
            appear.SamplePointParam = x;
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(1);

        // x8 MSAA

        await Tween(0, 1, 1.5f, (x) => {
            appear.ActiveLayer = 2 + x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 2, 1.5f, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 0, 0.5f, (x) => {
            appear.TriangleParam = x;
            GlobalAppearance = appear;
        });
    }

    async Awaitable RunSequence2Async()
    {
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

        await Tween(0, 1, 1, (x) => {
            appear.SamplePointParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 0, 0.2f, (x) => {
            colors.HitColor.a = colors.MissColor.a = x;
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        appear.SamplePointSnap = 0;
        GlobalAppearance = appear;

        colors.HitColor = palette.HitColor;

        await Tween(0, 1, 0.2f, (x) => {
            colors.HitColor.a =  x;
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 0.5f, (x) => {
            appear.SamplePointSnap = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 0, 0.5f, (x) => {
            appear.SamplePointSnap = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 0.2f, (x) => {
            appear.TriangleParam = 1 - x;
            colors.HitColor.a = 1 - x;
            colors.PixelColor.a = x;
            GlobalAppearance = appear;
            GlobalColors = colors;
        });
    }

    async Awaitable RunSequence3Async()
    {
        var appear = GlobalAppearance;
        var colors = GlobalColors;
        var scheme = colors;

        appear.ActiveLayer = 2;
        appear.GridLineParam = 1;
        appear.SamplePointParam = 1;
        appear.SamplePointSnap = 1;
        appear.SampleSource = Appearance.Source.Threshold;
        appear.PixelParam = 0;
        appear.TriangleParam = 0;
        colors.HitColor = scheme.MissColor;

        GlobalAppearance = appear;
        GlobalColors = colors;

        SetGradientOpacity(0);
        SetMaskOpacity(0);

        await Tween(0, 0.2f, 0.5f, (x) => SetGradientOpacity(x));

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 0, 0.5f, (x) => {
            appear.SamplePointSnap = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 0.2f, (x) => {
            colors.HitColor = Color.Lerp(scheme.MissColor, scheme.HitColor, x);
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 0.5f, (x) => {
            appear.SamplePointSnap = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 1.5f, (x) => {
            appear.PixelParam = x;
            GlobalAppearance = appear;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(1, 0, 0.5f, (x) => {
            colors.PixelColor.a = x;
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 0.2f, (x) => {
            colors.HitColor = Color.Lerp(scheme.HitColor, scheme.MissColor, x);
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        appear.SampleSource = Appearance.Source.Gradient;
        GlobalAppearance = appear;

        await Tween(0, 1, 0.2f, (x) => {
            colors.MissColor = colors.HitColor =
                Color.Lerp(scheme.MissColor, scheme.HitColor, x);
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 0.2f, (x) => {
            colors.HitColor = Color.Lerp(scheme.HitColor, scheme.FocusColor, x);
            colors.MissColor = Color.Lerp(scheme.HitColor, scheme.MissColor, x);
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 0.5f, 0.5f, (x) => SetMaskOpacity(x));

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0.5f, 0, 0.5f, (x) => SetMaskOpacity(x));

        await Awaitable.WaitForSecondsAsync(0.5f);

        await Tween(0, 1, 0.2f, (x) => {
            colors.HitColor = Color.Lerp(scheme.FocusColor, scheme.HitColor, x);
            GlobalColors = colors;
        });

        await Awaitable.WaitForSecondsAsync(0.5f);

        colors.PixelColor = scheme.PixelColor;
        GlobalColors = colors;

        await Tween(0, 1, 1.5f, (x) => {
            appear.PixelParam = x;
            GlobalAppearance = appear;
        });
    }

    #endregion
}
