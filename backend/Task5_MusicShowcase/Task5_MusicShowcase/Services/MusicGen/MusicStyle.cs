namespace Task5_MusicShowcase.Services.MusicGen;

public record MusicStyle
{
    public int Bpm { get; init; } = MusicConstants.DefaultBpm;
    public int[] ScaleIntervals { get; init; } = [];
    public WaveformType LeadWave { get; init; }
    public WaveformType BassWave { get; init; }
    public bool HasDrums { get; init; }
    public double EnvelopeAttack { get; init; } = MusicConstants.DefaultAttack;
    public double EnvelopeRelease { get; init; } = MusicConstants.DefaultRelease;
    public bool IsChiptune { get; init; }
}