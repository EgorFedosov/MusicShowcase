using System.IO.Compression;
using Bogus;
using Task5_MusicShowcase.Models.DTOs;
using Task5_MusicShowcase.Services.Interfaces;

namespace Task5_MusicShowcase.Services
{
    public class SongService(
        ISeedCalculator seedCalculator,
        IMusicGenerator musicGenerator,
        ICoverGenerator coverGenerator) : ISongService
    {
        private const string CoverRoute = "/api/songs/cover";
        private const string AudioRoute = "/api/songs/audio";
        private const float AlbumProbability = 0.8f;
        private const string SingleAlbumTitle = "Single";

        public SongsResponse GenerateBatch(GenerationRequest request)
        {
            var songs = new List<SongDto>(request.PageSize);
            var startIndex = (request.Page - 1) * request.PageSize;

            for (var i = 0; i < request.PageSize; i++)
            {
                songs.Add(GenerateSong(request, startIndex + i + 1));
            }

            return new SongsResponse
            {
                Songs = songs,
                Page = request.Page
            };
        }

        public byte[] GenerateExportArchive(GenerationRequest request)
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var startIndex = (request.Page - 1) * request.PageSize;

                for (var i = 0; i < request.PageSize; i++)
                {
                    AddSongToArchive(archive, request, startIndex + i + 1);
                }
            }

            return memoryStream.ToArray();
        }

        private void AddSongToArchive(ZipArchive archive, GenerationRequest request, int index)
        {
            var (seed, title, artist) = GetSongMetadata(request, index);
            var fileNameBase = $"{SanitizeFileName(artist)} - {SanitizeFileName(title)}";
            
            var audioBytes = musicGenerator.GenerateTrack(seed);
            AddZipEntry(archive, $"{fileNameBase}.wav", audioBytes);
            
            var coverBytes = coverGenerator.GenerateCover(seed, title, artist);
            AddZipEntry(archive, $"{fileNameBase}.png", coverBytes);
        }

        private (long Seed, string Title, string Artist) GetSongMetadata(GenerationRequest request, int index)
        {
            var contentSeed = seedCalculator.GetContentSeed(request.Seed, request.Region, index);
            var faker = CreateFaker(request.Region, contentSeed);

            return (contentSeed, faker.Commerce.ProductName(), faker.Name.FullName());
        }

        private static void AddZipEntry(ZipArchive archive, string fileName, byte[] content)
        {
            var entry = archive.CreateEntry(fileName);
            using var stream = entry.Open();
            stream.Write(content);
        }

        private static string SanitizeFileName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).Trim();
        }

        private SongDto GenerateSong(GenerationRequest request, int index)
        {
            var contentSeed = seedCalculator.GetContentSeed(request.Seed, request.Region, index);
            var faker = CreateFaker(request.Region, contentSeed);
            var title = faker.Lorem.Sentence(faker.Random.Int(2, 5)).TrimEnd('.');
            var artist = faker.Name.FullName();

            return new SongDto
            {
                Index = index,
                Title = title,
                Artist = artist,
                Album = faker.Random.Bool(AlbumProbability) ? faker.Commerce.ProductName() : SingleAlbumTitle,
                Genre = faker.Music.Genre(),
                Review = faker.Rant.Review(),
                Likes = GenerateLikes(request, index),
                CoverUrl = GetCoverUrl(contentSeed, title, artist),
                AudioUrl = GetAudioUrl(contentSeed)
            };
        }

        private static Faker CreateFaker(string region, int seed) => new(region)
        {
            Random = new Randomizer(seed)
        };

        private int GenerateLikes(GenerationRequest request, int index)
        {
            var seed = seedCalculator.GetLikesSeed(request.Seed, request.LikesAverage, index);
            var random = new Random(seed);
            return CalculateLikesCount(request.LikesAverage, random);
        }

        private static int CalculateLikesCount(double average, Random random)
        {
            var baseLikes = (int)average;
            return random.NextDouble() < average - (int)average ? baseLikes + 1 : baseLikes;
        }

        private static string GetCoverUrl(int seed, string title, string artist) =>
            $"{CoverRoute}?seed={seed}&title={Uri.EscapeDataString(title)}&artist={Uri.EscapeDataString(artist)}";

        private static string GetAudioUrl(int seed) => $"{AudioRoute}?seed={seed}";
    }
}