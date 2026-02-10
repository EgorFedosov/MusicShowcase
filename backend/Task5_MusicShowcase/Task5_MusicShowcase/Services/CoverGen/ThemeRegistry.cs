namespace Task5_MusicShowcase.Services.CoverGen;

public static class ThemeRegistry
{
    public static CoverTheme GetTheme(int seed)
    {
        if (ThemeAssets.Templates.Count == 0) return new CoverTheme();

        var random = new Random(seed);

        var templateIndex = random.Next(ThemeAssets.Templates.Count);
        var theme = ThemeAssets.Templates[templateIndex].Clone();

        var paletteIndex = random.Next(ThemeAssets.Palettes.Count);
        var palette = ThemeAssets.Palettes[paletteIndex];

        theme.BackgroundColor = palette.Background;
        theme.PrimaryColor = palette.Primary;
        theme.AccentColor = palette.Accent;
        theme.UseDarkOverlay = palette.DarkOverlay;

        if (!(random.NextDouble() < ThemeAssets.MutationProbability)) return theme;
        var shift = random.Next(-ThemeAssets.HueShiftRange, ThemeAssets.HueShiftRange);
        theme.AccentColor = ThemeUtils.ShiftColorHue(theme.AccentColor, shift);

        return theme;
    }
}