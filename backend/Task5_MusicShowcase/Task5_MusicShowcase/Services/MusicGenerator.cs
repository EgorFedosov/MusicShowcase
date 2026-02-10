using System.Text;
using Task5_MusicShowcase.Services.Interfaces;
using Task5_MusicShowcase.Services.MusicGen;

namespace Task5_MusicShowcase.Services
{
    public class MusicGenerator : IMusicGenerator
    {
        private const int SampleRate = 44100;
        private const short BitsPerSample = 16;
        private const short Channels = 1;
        private const int DurationSeconds = 30;
        private const short MaxAmplitude = 10000;
        private const int TotalSamples = SampleRate * DurationSeconds;
        private const int FileSize = 36 + TotalSamples * Channels * BitsPerSample / 8;
        private const int PatternLength = 16;
        private const int BaseRootNote = 60;
        private const int BassRootNote = 36;

        public byte[] GenerateTrack(long seed, string genre)
        {
            var random = CreateRandom(seed);
            var style = StyleRegistry.GetStyle(genre);

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            WriteWavHeader(writer);
            GenerateComposition(writer, random, style);

            return stream.ToArray();
        }

        private void GenerateComposition(BinaryWriter writer, Random random, MusicStyle style)
        {
            var rhythm = CalculateRhythm(style);
            var melodyPattern = GenerateMelodyPattern(random);
            var state = new CompositionState();

            while (state.CurrentSample < TotalSamples)
            {
                var samplesThisStep = Math.Min(rhythm.SamplesPerStep, TotalSamples - state.CurrentSample);
                var context = CreateStepContext(style, melodyPattern, state.StepIndex, random);

                GenerateSamplesForStep(writer, context, samplesThisStep, state.CurrentSample);

                state.CurrentSample += samplesThisStep;
                state.StepIndex++;
            }
        }

        private RhythmConfig CalculateRhythm(MusicStyle style)
        {
            var secondsPerBeat = 60.0 / style.Bpm;
            var samplesPerBeat = (int)(SampleRate * secondsPerBeat);
            var samplesPerStep = style.IsChiptune ? samplesPerBeat / 4 : samplesPerBeat / 2;

            return new RhythmConfig(samplesPerStep);
        }

        private int[] GenerateMelodyPattern(Random random)
        {
            var pattern = new int[PatternLength];
            var currentNote = 0;

            for (var i = 0; i < PatternLength; i++)
            {
                currentNote += random.Next(-2, 3);
                currentNote = ClampNote(currentNote);
                pattern[i] = currentNote;
            }

            return pattern;
        }

        private static int ClampNote(int note)
        {
            return note switch
            {
                > 8 => note - 4,
                < -8 => note + 4,
                _ => note
            };
        }

        private StepContext CreateStepContext(MusicStyle style, int[] pattern, int stepIndex, Random random)
        {
            var noteIndex = pattern[stepIndex % PatternLength];

            if (random.NextDouble() < 0.05)
                noteIndex = random.Next(-5, 5);

            var isRest = !style.IsChiptune && random.NextDouble() < 0.1;

            var leadFreq = Synthesizer.GetFrequency(noteIndex, style.ScaleIntervals, BaseRootNote);
            var bassFreq = Synthesizer.GetFrequency(pattern[0], style.ScaleIntervals, BassRootNote);

            return new StepContext(style, leadFreq, bassFreq, isRest, stepIndex, random);
        }

        private void GenerateSamplesForStep(BinaryWriter writer, StepContext ctx, int samplesCount,
            int globalSampleStart)
        {
            var secondsPerBeat = 60.0 / ctx.Style.Bpm;

            for (var i = 0; i < samplesCount; i++)
            {
                var t = (double)i / SampleRate;
                var globalTime = (double)(globalSampleStart + i) / SampleRate;

                var lead = GenerateLeadSample(ctx, t, samplesCount);
                var bass = GenerateBassSample(ctx, t);
                var drums = GenerateDrumSample(ctx, globalTime, secondsPerBeat);

                WriteMixedSample(writer, lead, bass, drums);
            }
        }

        private static double GenerateLeadSample(StepContext ctx, double t, int totalSamples)
        {
            if (ctx.IsRest) return 0;

            var raw = Synthesizer.GenerateWave(ctx.Style.LeadWave, ctx.LeadFreq, t);
            return Synthesizer.ApplyEnvelope(raw, t, (double)totalSamples / SampleRate, ctx.Style.EnvelopeAttack,
                ctx.Style.EnvelopeRelease);
        }

        private static double GenerateBassSample(StepContext ctx, double t)
        {
            if (ctx.StepIndex % 2 != 0 && !ctx.Style.IsChiptune) return 0;

            var raw = Synthesizer.GenerateWave(ctx.Style.BassWave, ctx.BassFreq, t);
            var envelope = Math.Max(0, 1.0 - (t * 3));
            return raw * 0.5 * envelope;
        }

        private static double GenerateDrumSample(StepContext ctx, double globalTime, double secondsPerBeat)
        {
            if (!ctx.Style.HasDrums) return 0;

            var beatTime = globalTime % secondsPerBeat;
            var sample = 0.0;

            if (beatTime < 0.1)
            {
                var kickFreq = 100.0 * (1.0 - (beatTime / 0.1));
                sample += Math.Sin(2 * Math.PI * kickFreq * beatTime) * 0.7;
            }

            var isBackBeat = ((int)(globalTime / secondsPerBeat) % 2) == 1;
            if (isBackBeat && beatTime < 0.05)
            {
                sample += (ctx.Random.NextDouble() * 2 - 1) * 0.2;
            }

            return sample;
        }

        private void WriteMixedSample(BinaryWriter writer, double lead, double bass, double drums)
        {
            var mixed = (lead * 0.45) + (bass * 0.35) + (drums * 0.3);

            if (mixed > 1.0) mixed = 1.0;
            if (mixed < -1.0) mixed = -1.0;

            writer.Write((short)(mixed * MaxAmplitude));
        }

        private static void WriteWavHeader(BinaryWriter writer)
        {
            writer.Write("RIFF"u8.ToArray());
            writer.Write(FileSize);
            writer.Write("WAVE"u8.ToArray());
            writer.Write("fmt "u8.ToArray());
            writer.Write(16);
            writer.Write((short)1);
            writer.Write(Channels);
            writer.Write(SampleRate);
            writer.Write(SampleRate * Channels * BitsPerSample / 8);
            writer.Write((short)(Channels * BitsPerSample / 8));
            writer.Write(BitsPerSample);
            writer.Write("data"u8.ToArray());
            writer.Write(TotalSamples * Channels * BitsPerSample / 8);
        }

        private static Random CreateRandom(long seed)
        {
            return new Random((int)(seed ^ (seed >> 32)));
        }

        private record RhythmConfig(int SamplesPerStep);

        private class CompositionState
        {
            public int CurrentSample { get; set; }
            public int StepIndex { get; set; }
        }

        private record StepContext(
            MusicStyle Style,
            double LeadFreq,
            double BassFreq,
            bool IsRest,
            int StepIndex,
            Random Random);
    }
}