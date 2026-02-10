namespace Task5_MusicShowcase.Services.MusicGen;

public static class Synthesizer
{
    public static double GenerateWave(WaveformType type, double frequency, double time)
    {
        return type switch
        {
            WaveformType.Sine => CalculateSineWave(frequency, time),
            WaveformType.Square => CalculateSquareWave(frequency, time),
            WaveformType.Triangle => CalculateTriangleWave(frequency, time),
            WaveformType.Sawtooth => CalculateSawtoothWave(frequency, time),
            WaveformType.Noise => CalculateNoise(),
            _ => MusicConstants.MinAmplitude
        };
    }

    public static double ApplyEnvelope(double sample, double timeInNote, double duration, double attack,
        double release)
    {
        var amplitude = CalculateAmplitude(timeInNote, duration, attack, release);
        return sample * amplitude;
    }

    public static double GetFrequency(int noteIndex, int[] scale, int rootNote)
    {
        var midiNote = CalculateMidiNote(noteIndex, scale, rootNote);
        return ConvertMidiToFrequency(midiNote);
    }

    private static double CalculateSineWave(double frequency, double time)
        => Math.Sin(2 * Math.PI * frequency * time);


    private static double CalculateSquareWave(double frequency, double time)
    {
        var sineValue = CalculateSineWave(frequency, time);
        return Math.Sign(sineValue) * MusicConstants.SquareWaveMultiplier;
    }

    private static double CalculateTriangleWave(double frequency, double time)
    {
        var sineValue = CalculateSineWave(frequency, time);
        return (MusicConstants.TriangleMultiplier / Math.PI) * Math.Asin(sineValue);
    }

    private static double CalculateSawtoothWave(double frequency, double time)
    {
        var cyclePosition = frequency * time;
        return MusicConstants.SawtoothMultiplier *
               (cyclePosition - Math.Floor(cyclePosition + MusicConstants.SawtoothOffset));
    }

    private static double CalculateNoise()
        => new Random().NextDouble() * MusicConstants.NoiseRange - MusicConstants.NoiseOffset;


    private static double CalculateAmplitude(double time, double duration, double attack, double release)
    {
        if (time < attack) return CalculateAttackPhase(time, attack);
        return time > duration - release
            ? CalculateReleasePhase(time, duration, release)
            : CalculateDecayPhase(time, duration, attack);
    }

    private static double CalculateAttackPhase(double time, double attack)
        => time / attack;


    private static double CalculateReleasePhase(double time, double duration, double release)
    {
        var remainingTime = duration - time;
        return Math.Max(MusicConstants.MinAmplitude, remainingTime / release);
    }

    private static double CalculateDecayPhase(double time, double duration, double attack)
    {
        var progress = (time - attack) / (duration - attack);
        return MusicConstants.MaxAmplitude - (progress * MusicConstants.DecaySmoothingFactor);
    }

    private static int CalculateMidiNote(int noteIndex, int[] scale, int rootNote)
    {
        var octave = noteIndex / scale.Length;
        var step = noteIndex % scale.Length;

        if (step < 0)
        {
            step += scale.Length;
            octave--;
        }

        var semitoneOffset = scale[step] + (octave * (int)MusicConstants.SemitonesInOctave);
        return rootNote + semitoneOffset;
    }

    private static double ConvertMidiToFrequency(int midiNote)
    {
        var exponent = (midiNote - MusicConstants.MidiNoteOffset) / MusicConstants.SemitonesInOctave;
        return MusicConstants.BaseTuningFrequency * Math.Pow(MusicConstants.BaseFrequencyBase, exponent);
    }
}