using SkiaSharp;

namespace Task5_MusicShowcase.Services.CoverGen;

public enum CoverLayout
{
    ClassicCenter,
    BottomRightPoster,
    TopLeftCatalog,
    VerticalSpine,
    SplitHeaderFooter
}

public class CoverTheme
{
    public string Name { get; set; } = string.Empty;
    public string TitleFont { get; set; } = "Arial";
    public string ArtistFont { get; set; } = "Arial";
        
    public SKColor BackgroundColor { get; set; }
    public SKColor PrimaryColor { get; set; }
    public SKColor AccentColor { get; set; }
    public bool UseDarkOverlay { get; set; } = true;
        
    public List<CoverLayout> AllowedLayouts { get; set; } = new();
        
    public Action<SKCanvas, SKPaint, Random>? DrawGeometryAction { get; set; }

    public CoverTheme Clone()
    {
        return new CoverTheme
        {
            Name = Name,
            TitleFont = TitleFont,
            ArtistFont = ArtistFont,
            BackgroundColor = BackgroundColor,
            PrimaryColor = PrimaryColor,
            AccentColor = AccentColor,
            UseDarkOverlay = UseDarkOverlay,
            AllowedLayouts = [..AllowedLayouts],
            DrawGeometryAction = DrawGeometryAction
        };
    }
}

public record ColorPalette(SKColor Background, SKColor Primary, SKColor Accent, bool DarkOverlay);
    
public record StripeConfig(bool IsDiagonal, int Count, int Step);