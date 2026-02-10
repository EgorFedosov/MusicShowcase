namespace Task5_MusicShowcase.Services.MusicGen;

public static class StyleRegistry
{
    private static readonly Dictionary<string, MusicStyle> Styles = StyleLibrary.LoadStyles();
    private static readonly MusicStyle DefaultStyle = StyleLibrary.CreateDefault();

    public static MusicStyle GetStyle(string genre)
        => string.IsNullOrEmpty(genre) ? DefaultStyle : FindStyle(genre);


    private static MusicStyle FindStyle(string genre)
        => Styles.TryGetValue(genre, out var style) ? style : FindPartialMatch(genre);


    private static MusicStyle FindPartialMatch(string genre)
    {
        var key = Styles.Keys.FirstOrDefault(k => genre.Contains(k, StringComparison.OrdinalIgnoreCase));
        return key != null ? Styles[key] : DefaultStyle;
    }
}