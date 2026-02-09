using Task5_MusicShowcase.Services.Interfaces;

namespace Task5_MusicShowcase.Services
{
    public class MusicGenerator : IMusicGenerator
    {
        private const int SampleRate = 44100;
        private const short BitsPerSample = 16;
        private const short Channels = 1;
        private const int DurationSeconds = 30;
        private const short MaxAmplitude = 10000;
        private const double NoteDurationStep = 0.2;
        private const int TotalSamples = SampleRate * DurationSeconds;
        private const int ByteRate = SampleRate * Channels * BitsPerSample / 8;
        private const int BlockAlign = Channels * BitsPerSample / 8;
        private const int DataSize = TotalSamples * Channels * BitsPerSample / 8;
        private const int FileSize = 36 + DataSize;
        private const int SeedShift = 32;
        private const int DurationMultiplierMin = 1;
        private const int DurationMultiplierMax = 3;
        private const short AudioFormatPcm = 1;
        private const int Subchunk1Size = 16;
        private static readonly byte[] RiffHeader = "RIFF"u8.ToArray();
        private static readonly byte[] WaveHeader = "WAVE"u8.ToArray();
        private static readonly byte[] FmtHeader = "fmt "u8.ToArray();
        private static readonly byte[] DataHeader = "data"u8.ToArray();

        private static readonly double[] HappyScale =
        [
            261.63, 293.66, 329.63, 392.00, 440.00, 523.25
        ];

        public byte[] GenerateTrack(long seed)
        {
            var random = new Random((int)(seed ^ (seed >> SeedShift)));
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            WriteWavHeader(writer);
            GenerateMelody(writer, random);
            return stream.ToArray();
        }

        private static void GenerateMelody(BinaryWriter writer, Random random)
        {
            var samplesWritten = 0;
            while (samplesWritten < TotalSamples)
            {
                var frequency = HappyScale[random.Next(HappyScale.Length)];
                var calculatedSamples = CalculateNextNoteDuration(random);
                var samplesInNote = Math.Min(calculatedSamples, TotalSamples - samplesWritten);

                WriteNoteSamples(writer, frequency, samplesInNote);
                samplesWritten += samplesInNote;
            }
        }

        private static int CalculateNextNoteDuration(Random random)
        {
            var durationMultiplier = random.Next(DurationMultiplierMin, DurationMultiplierMax);
            var durationSeconds = durationMultiplier * NoteDurationStep;
            return (int)(SampleRate * durationSeconds);
        }

        private static void WriteNoteSamples(BinaryWriter writer, double frequency, int samples)
        {
            for (var i = 0; i < samples; i++)
            {
                var t = (double)i / SampleRate;
                var wave = Math.Sin(2 * Math.PI * frequency * t);
                var envelope = 1.0 - ((double)i / samples);
                var sampleValue = (short)(wave * MaxAmplitude * envelope);
                writer.Write(sampleValue);
            }
        }

        private static void WriteWavHeader(BinaryWriter writer)
        {
            writer.Write(RiffHeader);
            writer.Write(FileSize);
            writer.Write(WaveHeader);
            writer.Write(FmtHeader);
            writer.Write(Subchunk1Size);
            writer.Write(AudioFormatPcm);
            writer.Write(Channels);
            writer.Write(SampleRate);
            writer.Write(ByteRate);
            writer.Write((short)BlockAlign);
            writer.Write(BitsPerSample);
            writer.Write(DataHeader);
            writer.Write(DataSize);
        }
    }
}