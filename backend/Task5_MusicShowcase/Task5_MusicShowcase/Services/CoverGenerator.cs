using SkiaSharp;
using Task5_MusicShowcase.Services.Interfaces;

namespace Task5_MusicShowcase.Services
{
    public class CoverGenerator : ICoverGenerator
    {
        private const int ImageSize = 512;
        private const int TitleSize = 60;
        private const int ArtistSize = 35;
        private const float TitleYRatio = 0.45f;
        private const float ArtistYRatio = 0.6f;
        private const int ShadowOffset = 4;
        private const byte ShadowAlpha = 160;
        private const int ImageQuality = 100;
        private const int MinGradientColors = 3;
        private const int MaxGradientColors = 5;
        private const int SeedShift = 32;
        private const string DefaultFont = "Arial";
        private const string ResourcesDir = "Resources";
        private const string FontsDir = "Fonts";

        private static readonly string[] FontFiles =
        [
            "Anton-Regular.ttf",
            "Lobster-Regular.ttf",
            "PermanentMarker-Regular.ttf",
            "PlayfairDisplay-VariableFont_wght.ttf",
            "PressStart2P-Regular.ttf"
        ];

        public byte[] GenerateCover(long seed, string title, string artist)
        {
            var random = CreateRandom(seed);
            using var surface = SKSurface.Create(new SKImageInfo(ImageSize, ImageSize));
            var canvas = surface.Canvas;

            DrawGradientBackground(canvas, random);
            DrawTextContent(canvas, random, title, artist);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, ImageQuality);
            return data.ToArray();
        }

        private static Random CreateRandom(long seed)
        {
            return new Random((int)(seed ^ (seed >> SeedShift)));
        }

        private static void DrawGradientBackground(SKCanvas canvas, Random random)
        {
            var colors = GenerateRandomColors(random);
            var shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(ImageSize, ImageSize),
                colors,
                null,
                SKShaderTileMode.Clamp);

            using var paint = new SKPaint { Shader = shader };
            canvas.DrawRect(0, 0, ImageSize, ImageSize, paint);
        }

        private static SKColor[] GenerateRandomColors(Random random)
        {
            var count = random.Next(MinGradientColors, MaxGradientColors);
            var colors = new SKColor[count];

            for (var i = 0; i < count; i++)
            {
                colors[i] = new SKColor(
                    (byte)random.Next(256),
                    (byte)random.Next(256),
                    (byte)random.Next(256));
            }

            return colors;
        }

        private static void DrawTextContent(SKCanvas canvas, Random random, string title, string artist)
        {
            using var typeface = LoadRandomTypeface(random);
            using var titleFont = new SKFont(typeface, TitleSize);
            using var artistFont = new SKFont(typeface, ArtistSize);

            DrawCenteredLine(canvas, title, ImageSize * TitleYRatio, titleFont);
            DrawCenteredLine(canvas, artist, ImageSize * ArtistYRatio, artistFont);
        }

        private static void DrawCenteredLine(SKCanvas canvas, string text, float y, SKFont font)
        {
            var originalSize = font.Size;
            ScaleFontToFit(font, text);
            DrawTextLayers(canvas, text, y, font);
            font.Size = originalSize;
        }

        private static void ScaleFontToFit(SKFont font, string text)
        {
            const float padding = 40;
            var maxWidth = ImageSize - padding;
            var currentWidth = font.MeasureText(text);

            if (currentWidth > maxWidth)
            {
                font.Size *= maxWidth / currentWidth;
            }
        }

        private static void DrawTextLayers(SKCanvas canvas, string text, float y, SKFont font)
        {
            var x = (ImageSize - font.MeasureText(text)) / 2;
            DrawSingleLayer(canvas, text, x + ShadowOffset, y + ShadowOffset, font,
                SKColors.Black.WithAlpha(ShadowAlpha));
            DrawSingleLayer(canvas, text, x, y, font, SKColors.White);
        }

        private static void DrawSingleLayer(SKCanvas canvas, string text, float x, float y, SKFont font, SKColor color)
        {
            using var paint = new SKPaint { Color = color, IsAntialias = true };
            canvas.DrawText(text, x, y, font, paint);
        }

        private static SKTypeface LoadRandomTypeface(Random random)
        {
            var fontName = FontFiles[random.Next(FontFiles.Length)];
            var fontPath = Path.Combine(AppContext.BaseDirectory, ResourcesDir, FontsDir, fontName);

            return File.Exists(fontPath)
                ? SKTypeface.FromFile(fontPath)
                : SKTypeface.FromFamilyName(DefaultFont);
        }
    }
}