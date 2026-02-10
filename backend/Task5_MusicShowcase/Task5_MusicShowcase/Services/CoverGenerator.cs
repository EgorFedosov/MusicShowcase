using SkiaSharp;
using Task5_MusicShowcase.Services.Interfaces;
using Task5_MusicShowcase.Services.CoverGen;

namespace Task5_MusicShowcase.Services;

public class CoverGenerator : ICoverGenerator
{
    private const int ImageSize = 512;
    private const int ImageQuality = 100;
    private const string ResourcesDir = "Resources";
    private const string FontsDir = "Fonts";
    private const string CoversDir = "Covers";
    private const string DefaultFont = "Arial";
    private const double ImageUseProbability = 0.7;
    private const byte OverlayAlpha = 180;
    private const int TextShadowOffset = 2;
    private const byte ShadowAlpha = 160;
    private const float TextSizeTitle = 60;
    private const float TextSizeArtist = 35;
    private const float MinTextSize = 24;
    private const float LineHeightMultiplier = 1.15f;

    public byte[] GenerateCover(long seed, string title, string artist)
    {
        var random = CreateRandom(seed);
        var info = new SKImageInfo(ImageSize, ImageSize);

        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;
        var theme = ThemeRegistry.GetTheme((int)seed);

        DrawBackground(canvas, random, theme);
        DrawGeometry(canvas, random, theme);

        var layout = SelectLayout(theme, random);
        DrawTextContent(canvas, title, artist, theme, layout);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, ImageQuality);
        return data.ToArray();
    }

    private void DrawBackground(SKCanvas canvas, Random random, CoverTheme theme)
    {
        if (TryDrawImageBackground(canvas, random))
        {
            ApplyOverlayIfRequired(canvas, theme);
        }
        else
        {
            canvas.Clear(theme.BackgroundColor);
        }
    }

    private bool TryDrawImageBackground(SKCanvas canvas, Random random)
    {
        if (random.NextDouble() >= ImageUseProbability) return false;

        var coversPath = Path.Combine(AppContext.BaseDirectory, ResourcesDir, CoversDir);
        if (!Directory.Exists(coversPath)) return false;

        var files = GetImageFiles(coversPath);
        if (files.Length == 0) return false;

        var file = files[random.Next(files.Length)];
        using var bitmap = SKBitmap.Decode(file);

        var destRect = new SKRect(0, 0, ImageSize, ImageSize);
        canvas.DrawBitmap(bitmap, destRect);

        return true;
    }

    private string[] GetImageFiles(string path)
    {
        return Directory.GetFiles(path, "*.*")
            .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        f.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    private void ApplyOverlayIfRequired(SKCanvas canvas, CoverTheme theme)
    {
        if (!theme.UseDarkOverlay) return;

        using var paint = new SKPaint
        {
            Color = theme.BackgroundColor.WithAlpha(OverlayAlpha),
            Style = SKPaintStyle.Fill
        };
        canvas.DrawRect(0, 0, ImageSize, ImageSize, paint);
    }

    private static void DrawGeometry(SKCanvas canvas, Random random, CoverTheme theme)
    {
        using var paint = new SKPaint { IsAntialias = true };
        paint.Color = theme.AccentColor;
        theme.DrawGeometryAction?.Invoke(canvas, paint, random);
    }

    private CoverLayout SelectLayout(CoverTheme theme, Random random)
        => theme.AllowedLayouts[random.Next(theme.AllowedLayouts.Count)];


    private void DrawTextContent(SKCanvas canvas, string title, string artist, CoverTheme theme, CoverLayout layout)
    {
        using var titleTypeface = LoadTypeface(theme.TitleFont);
        using var artistTypeface = LoadTypeface(theme.ArtistFont);

        using var titleFont = new SKFont(titleTypeface, TextSizeTitle);
        using var artistFont = new SKFont(artistTypeface, TextSizeArtist);

        using var titlePaint = CreatePaint(theme.PrimaryColor);
        using var artistPaint = CreatePaint(theme.PrimaryColor.WithAlpha(200));
        using var borderPaint = CreateBorderPaint(theme.PrimaryColor);

        switch (layout)
        {
            case CoverLayout.TopLeftCatalog:
                DrawLayoutTopLeft(canvas, title, artist, titleFont, artistFont, titlePaint, artistPaint,
                    borderPaint);
                break;
            case CoverLayout.BottomRightPoster:
                DrawLayoutBottomRight(canvas, title, artist, titleFont, artistFont, titlePaint, artistPaint);
                break;
            case CoverLayout.ClassicCenter:
                DrawLayoutCenter(canvas, title, artist, titleFont, artistFont, titlePaint, artistPaint);
                break;
            case CoverLayout.SplitHeaderFooter:
                DrawLayoutSplit(canvas, title, artist, titleFont, artistFont, titlePaint, artistPaint, borderPaint);
                break;
            case CoverLayout.VerticalSpine:
                DrawLayoutVertical(canvas, title, artist, titleFont, artistFont, titlePaint, artistPaint);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layout), layout, null);
        }
    }

    private static SKPaint CreatePaint(SKColor color)
    {
        return new SKPaint
        {
            Color = color,
            IsAntialias = true
        };
    }

    private static SKPaint CreateBorderPaint(SKColor color)
    {
        return new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = color,
            StrokeWidth = 2,
            IsAntialias = true
        };
    }

    private void DrawLayoutTopLeft(SKCanvas canvas, string title, string artist, SKFont titleFont,
        SKFont artistFont, SKPaint titlePaint, SKPaint artistPaint, SKPaint borderPaint)
    {
        artistFont.Size = 30;
        titleFont.Size = 50;

        DrawText(canvas, artist.ToUpper(), 40, 60, artistFont, artistPaint, TextAlign.Left);
        canvas.DrawLine(40, 80, 200, 80, borderPaint);
        DrawTextMultiline(canvas, title, 40, 130, titleFont, titlePaint, 430, TextAlign.Left);
    }

    private void DrawLayoutBottomRight(SKCanvas canvas, string title, string artist, SKFont titleFont,
        SKFont artistFont, SKPaint titlePaint, SKPaint artistPaint)
    {
        titleFont.Size = 80;
        artistFont.Size = 40;

        DrawTextMultiline(canvas, title, 480, 250, titleFont, titlePaint, 460, TextAlign.Right);
        DrawText(canvas, artist, 480, 480, artistFont, artistPaint, TextAlign.Right);
    }

    private void DrawLayoutCenter(SKCanvas canvas, string title, string artist, SKFont titleFont, SKFont artistFont,
        SKPaint titlePaint, SKPaint artistPaint)
    {
        DrawTextMultiline(canvas, title, 256, 240, titleFont, titlePaint, 450, TextAlign.Center);
        DrawText(canvas, artist, 256, 400, artistFont, artistPaint, TextAlign.Center);
    }

    private void DrawLayoutSplit(SKCanvas canvas, string title, string artist, SKFont titleFont, SKFont artistFont,
        SKPaint titlePaint, SKPaint artistPaint, SKPaint borderPaint)
    {
        artistFont.Size = 30;
        DrawText(canvas, artist.ToUpper(), 256, 60, artistFont, artistPaint, TextAlign.Center);

        canvas.DrawLine(200, 80, 312, 80, borderPaint);

        titleFont.Size = 65;
        DrawTextMultiline(canvas, title, 256, 380, titleFont, titlePaint, 480, TextAlign.Center);
    }

    private void DrawLayoutVertical(SKCanvas canvas, string title, string artist, SKFont titleFont,
        SKFont artistFont, SKPaint titlePaint, SKPaint artistPaint)
    {
        canvas.Save();
        canvas.RotateDegrees(-90);
        DrawTextFitted(canvas, title.ToUpper(), -480, 60, titleFont, titlePaint, 460, TextAlign.Left);
        canvas.Restore();

        DrawText(canvas, artist, 480, 480, artistFont, artistPaint, TextAlign.Right);
    }

    private enum TextAlign
    {
        Left,
        Center,
        Right
    }

    private static void DrawText(SKCanvas canvas, string text, float x, float y, SKFont font, SKPaint paint,
        TextAlign align)
    {
        var drawX = CalculateX(text, x, font, align);
        DrawShadowIfLarge(canvas, text, drawX, y, font, paint);
        canvas.DrawText(text, drawX, y, font, paint);
    }

    private static float CalculateX(string text, float x, SKFont font, TextAlign align)
    {
        var width = font.MeasureText(text);
        return align switch
        {
            TextAlign.Center => x - width / 2,
            TextAlign.Right => x - width,
            _ => x
        };
    }

    private static void DrawShadowIfLarge(SKCanvas canvas, string text, float x, float y, SKFont font,
        SKPaint paint)
    {
        if (font.Size <= 30) return;

        using var shadowPaint = paint.Clone();
        shadowPaint.Color = SKColors.Black.WithAlpha(ShadowAlpha);
        canvas.DrawText(text, x + TextShadowOffset, y + TextShadowOffset, font, shadowPaint);
    }

    private void DrawTextFitted(SKCanvas canvas, string text, float x, float y, SKFont font, SKPaint paint,
        float maxWidth, TextAlign align)
    {
        var originalSize = font.Size;

        while (font.MeasureText(text) > maxWidth && font.Size > MinTextSize)
        {
            font.Size -= 2;
        }

        DrawText(canvas, text, x, y, font, paint, align);
        font.Size = originalSize;
    }

    private void DrawTextMultiline(SKCanvas canvas, string text, float x, float y, SKFont font, SKPaint paint,
        float maxWidth, TextAlign align)
    {
        var lines = BreakTextIntoLines(text, font, maxWidth);
        var lineHeight = font.Size * LineHeightMultiplier;
        var currentY = y;

        foreach (var line in lines)
        {
            DrawText(canvas, line, x, currentY, font, paint, align);
            currentY += lineHeight;
        }
    }

    private List<string> BreakTextIntoLines(string text, SKFont font, float maxWidth)
    {
        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = "";

        foreach (var word in words)
        {
            if (font.MeasureText(word) > maxWidth)
            {
                if (!string.IsNullOrEmpty(currentLine)) lines.Add(currentLine);
                lines.Add(word);
                currentLine = "";
                continue;
            }

            var testLine = string.IsNullOrEmpty(currentLine) ? word : $"{currentLine} {word}";

            if (font.MeasureText(testLine) > maxWidth)
            {
                lines.Add(currentLine);
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }

        if (!string.IsNullOrEmpty(currentLine)) lines.Add(currentLine);
        return lines;
    }

    private static SKTypeface LoadTypeface(string fontName)
    {
        var fontPath = Path.Combine(AppContext.BaseDirectory, ResourcesDir, FontsDir, fontName);
        return File.Exists(fontPath)
            ? SKTypeface.FromFile(fontPath)
            : SKTypeface.FromFamilyName(DefaultFont, SKFontStyle.Bold);
    }

    private static Random CreateRandom(long seed)
    {
        return new Random(seed.GetHashCode());
    }
}