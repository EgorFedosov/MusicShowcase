using SkiaSharp;

namespace Task5_MusicShowcase.Services.CoverGen;

public static class ThemeAssets
{
    public const int Center = 256;
        
    public const int HueShiftRange = 15;
    public const double MutationProbability = 0.3;
    public const double StripeSkipProbability = 0.5;

    public const byte AlphaVeryLow = 40;
    public const byte AlphaLow = 50;
    public const byte AlphaMedium = 100;
    public const byte AlphaHigh = 180;

    public const int SizeSmall = 50;
    public const int SizeMedium = 100;
    public const int SizeLarge = 200;
    public const int SizeXLarge = 240;
    public const int SizeMassive = 300;
    public const int StrokeThin = 1;
    public const int StrokeMin = 1;
    public const int StrokeMax = 4;

    public const int MinShapes = 3;
    public const int MaxShapes = 8;
    public const int MinStripes = 3;
    public const int MaxStripes = 10;
        
    public const int StripeWidth = 100;
    public const int StripeSlantX1 = 200;
    public const int StripeSlantX2 = 300;
    public const int StripeMinHeight = 10;

    public const int GrungeLineCount = 30;
    public const int GrungeScatter = 50;
    public const int GridStepMin = 30;
    public const int GridStepMax = 80;
    public const int AuraCircleCount = 5;

    private const string FontAnton = "Anton-Regular.ttf";
    private const string FontBebas = "BebasNeue-Regular.ttf";
    private const string FontStaatliches = "Staatliches-Regular.ttf";
    private const string FontPlayfair = "PlayfairDisplay-VariableFont_wght.ttf";
    private const string FontMontserrat = "Montserrat-Regular.ttf";
    private const string FontRockSalt = "RockSalt-Regular.ttf";
    private const string FontPermanentMarker = "PermanentMarker-Regular.ttf";
    private const string FontOrbitron = "Orbitron-VariableFont_wght.ttf";
    private const string FontMichroma = "Michroma-Regular.ttf";
    private const string FontRighteous = "Righteous-Regular.ttf";
    private const string FontSatisfy = "Satisfy-Regular.ttf";

    public static readonly List<CoverTheme> Templates = CreateTemplates();
    public static readonly List<ColorPalette> Palettes = CreatePalettes();

    private static List<CoverTheme> CreateTemplates()
    {
        return
        [
            Create("Geometric", FontAnton, FontBebas, ThemePainters.DrawGeometric, CoverLayout.TopLeftCatalog,
                CoverLayout.SplitHeaderFooter),
            Create("Stripes", FontBebas, FontStaatliches, ThemePainters.DrawStripes, CoverLayout.BottomRightPoster,
                CoverLayout.ClassicCenter),
            Create("Minimal", FontPlayfair, FontMontserrat, ThemePainters.DrawMinimal, CoverLayout.ClassicCenter),
            Create("Grunge", FontRockSalt, FontPermanentMarker, ThemePainters.DrawGrunge,
                CoverLayout.TopLeftCatalog, CoverLayout.BottomRightPoster),
            Create("Grid", FontOrbitron, FontMichroma, ThemePainters.DrawGrid, CoverLayout.VerticalSpine),
            Create("Aura", FontRighteous, FontSatisfy, ThemePainters.DrawAura, CoverLayout.ClassicCenter)
        ];
    }

    private static CoverTheme Create(string name, string titleFont, string artistFont, Action<SKCanvas, SKPaint, Random> action, params CoverLayout[] layouts)
    {
        return new CoverTheme
        {
            Name = name,
            TitleFont = titleFont,
            ArtistFont = artistFont,
            DrawGeometryAction = action,
            AllowedLayouts = [..layouts]
        };
    }

    private static List<ColorPalette> CreatePalettes()
    {
        var p = new List<ColorPalette>();

        Add("#121212", "#FFD700", "#333333", true);
        Add("#0b0c15", "#00f3ff", "#ff0099", true);
        Add("#1b4d3e", "#d4af37", "#133b2f", true);
        Add("#0f2027", "#ffffff", "#2c5364", true);
        Add("#2c3e50", "#e74c3c", "#34495e", true);
        Add("#240046", "#e0aaff", "#9d4edd", true);
        Add("#000000", "#00ff41", "#003b00", true);
            
        Add("#ff6b6b", "#ffe66d", "#4ecdc4", true);
        Add("#002366", "#ffffff", "#ffd700", true);
        Add("#4a4a4a", "#f2f2f2", "#1a1a1a", true);
        Add("#3e2723", "#b9f6ca", "#5d4037", true);
        Add("#2b0000", "#ff0000", "#800000", true);
        Add("#4682b4", "#f0f8ff", "#5f9ea0", true);
        Add("#7b2cbf", "#e0aaff", "#240046", true);
        Add("#111111", "#ccff00", "#333333", true);
            
        Add("#ff9a9e", "#ffffff", "#fecfef", false);
        Add("#ffffff", "#000000", "#cccccc", false);
        Add("#f5f5dc", "#4b3621", "#d2b48c", false);
        Add("#f1c40f", "#2c3e50", "#ffffff", false);
        Add("#e6e6fa", "#483d8b", "#9370db", false);
        Add("#dcdcdc", "#cf0000", "#2f2f2f", false);
        Add("#ffecd1", "#78909c", "#ff7e67", false);

        return p;

        SKColor Hex(string c) => SKColor.Parse(c);

        void Add(string bg, string main, string acc, bool dark) => p.Add(new ColorPalette(Hex(bg), Hex(main), Hex(acc), dark));
    }
}