namespace Task5_MusicShowcase.Services.MusicGen;

public static class StyleLibrary
{
    public static Dictionary<string, MusicStyle> LoadStyles()
    {
        var styles = new Dictionary<string, MusicStyle>(StringComparer.OrdinalIgnoreCase);

        RegisterRockStyles(styles);
        RegisterPopStyles(styles);
        RegisterElectronicStyles(styles);
        RegisterJazzStyles(styles);
        RegisterClassicalStyles(styles);
        RegisterReggaeStyles(styles);
        RegisterFolkStyles(styles);

        return styles;
    }

    public static MusicStyle CreateDefault()
    {
        return new MusicStyle
        {
            ScaleIntervals = [0, 2, 4, 5, 7, 9, 11, 12]
        };
    }

    private static void RegisterRockStyles(Dictionary<string, MusicStyle> styles)
    {
        var style = CreateStyle(
            bpm: 125,
            scale: [0, 3, 5, 7, 10, 12],
            lead: WaveformType.Triangle,
            bass: WaveformType.Square,
            drums: true,
            attack: 0.02,
            release: 0.4
        );

        styles[MusicConstants.RockGenre] = style;
        styles[MusicConstants.MetalGenre] = style;
        styles[MusicConstants.PunkGenre] = style;
    }

    private static void RegisterPopStyles(Dictionary<string, MusicStyle> styles)
    {
        styles[MusicConstants.PopGenre] = CreateStyle(
            bpm: 110,
            scale: [0, 2, 4, 5, 7, 9, 11, 12],
            lead: WaveformType.Triangle,
            bass: WaveformType.Sine,
            drums: true,
            attack: 0.05,
            release: 0.5
        );
    }

    private static void RegisterElectronicStyles(Dictionary<string, MusicStyle> styles)
    {
        var style = CreateStyle(
            bpm: 128,
            scale: [0, 3, 5, 7, 10, 12],
            lead: WaveformType.Square,
            bass: WaveformType.Sawtooth,
            drums: true,
            attack: 0.01,
            release: 0.3,
            chiptune: true
        );

        styles[MusicConstants.ElectronicGenre] = style;
        styles[MusicConstants.TechnoGenre] = style;
    }

    private static void RegisterJazzStyles(Dictionary<string, MusicStyle> styles)
    {
        var style = CreateStyle(
            bpm: 90,
            scale: [0, 3, 5, 6, 7, 10, 12],
            lead: WaveformType.Sine,
            bass: WaveformType.Sine,
            drums: true,
            attack: 0.03,
            release: 0.7
        );

        styles[MusicConstants.JazzGenre] = style;
        styles[MusicConstants.BluesGenre] = style;
        styles[MusicConstants.SoulGenre] = style;
        styles[MusicConstants.FunkGenre] = style;
    }

    private static void RegisterClassicalStyles(Dictionary<string, MusicStyle> styles)
    {
        styles[MusicConstants.ClassicalGenre] = CreateStyle(
            bpm: 75,
            scale: [0, 2, 4, 5, 7, 9, 11, 12],
            lead: WaveformType.Triangle,
            bass: WaveformType.Sine,
            drums: false,
            attack: 0.1,
            release: 0.8
        );
    }

    private static void RegisterReggaeStyles(Dictionary<string, MusicStyle> styles)
    {
        styles[MusicConstants.ReggaeGenre] = CreateStyle(
            bpm: 70,
            scale: [0, 2, 4, 5, 7, 9, 11, 12],
            lead: WaveformType.Triangle,
            bass: WaveformType.Sine,
            drums: true,
            attack: 0.02,
            release: 0.3
        );
    }

    private static void RegisterFolkStyles(Dictionary<string, MusicStyle> styles)
    {
        var style = CreateStyle(
            bpm: 100,
            scale: [0, 2, 4, 7, 9, 12],
            lead: WaveformType.Sine,
            bass: WaveformType.Sine,
            drums: false,
            attack: 0.04,
            release: 0.6
        );

        styles[MusicConstants.FolkGenre] = style;
        styles[MusicConstants.CountryGenre] = style;
        styles[MusicConstants.WorldGenre] = style;
        styles[MusicConstants.LatinGenre] = style;
    }

    private static MusicStyle CreateStyle(
        int bpm, int[] scale, WaveformType lead, WaveformType bass,
        bool drums, double attack, double release, bool chiptune = false)
    {
        return new MusicStyle
        {
            Bpm = bpm,
            ScaleIntervals = scale,
            LeadWave = lead,
            BassWave = bass,
            HasDrums = drums,
            EnvelopeAttack = attack,
            EnvelopeRelease = release,
            IsChiptune = chiptune
        };
    }
}