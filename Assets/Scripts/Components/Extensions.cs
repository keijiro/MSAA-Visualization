using Unity.Mathematics;

public static class SamplePointExtensions
{
    // MSAA sampling pattern from:
    // https://learn.microsoft.com/en-us/windows/win32/api/d3d11/ne-d3d11-d3d11_standard_multisample_quality_levels

    static readonly float2[] Pattern2 = new []
    {
        math.float2( 4,  4) / 16,
        math.float2(-4, -4) / 16
    };

    static readonly float2[] Pattern4 = new []
    {
        math.float2(-2, -6) / 16,
        math.float2( 6, -2) / 16,
        math.float2(-6,  2) / 16,
        math.float2( 2,  6) / 16
    };

    static readonly float2[] Pattern8 = new []
    {
        math.float2( 1, -3) / 16,
        math.float2(-1,  3) / 16,
        math.float2( 5,  1) / 16,
        math.float2(-3, -5) / 16,
        math.float2(-5,  5) / 16,
        math.float2(-7, -1) / 16,
        math.float2( 3,  7) / 16,
        math.float2( 7, -7) / 16
    };

    static float2 GetSamplePattern(uint layer, uint index)
      => layer switch {
           1 => Pattern2[index],
           2 => Pattern4[index],
           3 => Pattern8[index],
           _ => (float2)0
         };

    public static float2 GetPosition
      (this in SamplePoint pt, in Layer layer, in PixelCoords pc)
      => math.float2(pc.Value) + 0.5f
         + GetSamplePattern(layer.Index, pt.Index);

    public static int GetElementCount(this in GridSpace space)
      => (int)(space.Dimensions.x * space.Dimensions.y);
}
