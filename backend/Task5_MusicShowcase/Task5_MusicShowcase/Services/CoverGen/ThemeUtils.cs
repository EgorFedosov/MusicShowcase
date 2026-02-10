using SkiaSharp;

namespace Task5_MusicShowcase.Services.CoverGen;

public static class ThemeUtils
{
    public static SKColor ShiftColorHue(SKColor color, int amount)
    {
        color.ToHsv(out var h, out var s, out var v);
        h = (h + amount) % 360;
        if (h < 0) h += 360;
        return SKColor.FromHsv(h, s, v, color.Alpha);
    }

    public static void ConfigureFill(SKPaint paint, SKColor baseColor, byte alpha)
    {
        paint.Style = SKPaintStyle.Fill;
        paint.Color = baseColor.WithAlpha(alpha);
    }

    public static void ConfigureStroke(SKPaint paint, SKColor baseColor, float width)
    {
        paint.Style = SKPaintStyle.Stroke;
        paint.StrokeWidth = width;
        paint.Color = baseColor;
    }

    public static void RandomizeAlpha(SKPaint paint, Random random, int min, int max)
        => paint.Color = paint.Color.WithAlpha((byte)random.Next(min, max));


    public static SKPoint GetRandomPoint(Random random, int canvasSize) =>
        new(random.Next(canvasSize), random.Next(canvasSize));
}