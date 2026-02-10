using SkiaSharp;

namespace Task5_MusicShowcase.Services.CoverGen;

public static class ThemePainters
{
    private const int Size = 512;

    public static void DrawGeometric(SKCanvas canvas, SKPaint paint, Random random)
    {
        var baseColor = paint.Color;
        ThemeUtils.ConfigureFill(paint, baseColor, ThemeAssets.AlphaHigh);

        var count = random.Next(ThemeAssets.MinShapes, ThemeAssets.MaxShapes);
        for (var i = 0; i < count; i++)
        {
            ThemeUtils.RandomizeAlpha(paint, random, ThemeAssets.AlphaLow, ThemeAssets.AlphaMedium);

            if (random.Next(2) == 0)
                DrawCircle(canvas, paint, random, ThemeAssets.SizeSmall, ThemeAssets.SizeLarge);
            else
                DrawRect(canvas, paint, random, ThemeAssets.SizeSmall, ThemeAssets.SizeLarge);
        }
    }

    public static void DrawStripes(SKCanvas canvas, SKPaint paint, Random random)
    {
        var baseColor = paint.Color;
        ThemeUtils.ConfigureFill(paint, baseColor, ThemeAssets.AlphaMedium);

        var config = new StripeConfig(
            IsDiagonal: random.Next(2) == 0,
            Count: random.Next(ThemeAssets.MinStripes, ThemeAssets.MaxStripes),
            Step: Size / random.Next(ThemeAssets.MinStripes, ThemeAssets.MaxStripes)
        );

        config = config with { Step = Size / config.Count };

        for (var i = 0; i < config.Count; i++)
        {
            if (random.NextDouble() > ThemeAssets.StripeSkipProbability) continue;

            if (config.IsDiagonal)
            {
                using var path = new SKPath();
                float x = i * config.Step;
                path.MoveTo(x, 0);
                path.LineTo(x + ThemeAssets.StripeWidth, 0);
                path.LineTo(x - ThemeAssets.StripeSlantX1, Size);
                path.LineTo(x - ThemeAssets.StripeSlantX2, Size);
                canvas.DrawPath(path, paint);
            }
            else
            {
                float height = random.Next(ThemeAssets.StripeMinHeight, config.Step);
                canvas.DrawRect(0, i * config.Step, Size, height, paint);
            }
        }
    }

    public static void DrawMinimal(SKCanvas canvas, SKPaint paint, Random random)
    {
        ThemeUtils.ConfigureStroke(paint, paint.Color, random.Next(ThemeAssets.StrokeMin, ThemeAssets.StrokeMax));
        float radius = random.Next(ThemeAssets.SizeMedium, ThemeAssets.SizeXLarge);
        canvas.DrawCircle(ThemeAssets.Center, ThemeAssets.Center, radius, paint);
    }

    public static void DrawGrunge(SKCanvas canvas, SKPaint paint, Random random)
    {
        paint.Style = SKPaintStyle.Stroke;

        for (var i = 0; i < ThemeAssets.GrungeLineCount; i++)
        {
            paint.StrokeWidth = random.Next(ThemeAssets.StrokeMin, ThemeAssets.StrokeMax);
            ThemeUtils.RandomizeAlpha(paint, random, ThemeAssets.AlphaLow, ThemeAssets.AlphaHigh);

            var start = ThemeUtils.GetRandomPoint(random, Size);
            var endX = start.X + random.Next(-ThemeAssets.GrungeScatter, ThemeAssets.GrungeScatter);
            var endY = start.Y + random.Next(-ThemeAssets.GrungeScatter, ThemeAssets.GrungeScatter);

            canvas.DrawLine(start.X, start.Y, endX, endY, paint);
        }
    }

    public static void DrawGrid(SKCanvas canvas, SKPaint paint, Random random)
    {
        ThemeUtils.ConfigureStroke(paint, paint.Color.WithAlpha(ThemeAssets.AlphaVeryLow), ThemeAssets.StrokeThin);

        var step = random.Next(ThemeAssets.GridStepMin, ThemeAssets.GridStepMax);
        for (var i = 0; i < Size; i += step)
        {
            canvas.DrawLine(i, 0, i, Size, paint);
            canvas.DrawLine(0, i, Size, i, paint);
        }
    }

    public static void DrawAura(SKCanvas canvas, SKPaint paint, Random random)
    {
        ThemeUtils.ConfigureFill(paint, paint.Color, ThemeAssets.AlphaLow);
        for (var i = 0; i < ThemeAssets.AuraCircleCount; i++)
        {
            DrawCircle(canvas, paint, random, ThemeAssets.SizeMedium, ThemeAssets.SizeMassive);
        }
    }

    private static void DrawCircle(SKCanvas canvas, SKPaint paint, Random random, int minR, int maxR)
    {
        float r = random.Next(minR, maxR);
        var pt = ThemeUtils.GetRandomPoint(random, Size);
        canvas.DrawCircle(pt.X, pt.Y, r, paint);
    }

    private static void DrawRect(SKCanvas canvas, SKPaint paint, Random random, int minS, int maxS)
    {
        float w = random.Next(minS, maxS);
        float h = random.Next(minS, maxS);
        var pt = ThemeUtils.GetRandomPoint(random, Size);
        canvas.DrawRect(pt.X, pt.Y, w, h, paint);
    }
}