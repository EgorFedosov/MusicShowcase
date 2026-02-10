using System.Security.Cryptography;
using System.Text;
using Task5_MusicShowcase.Services.Interfaces;

namespace Task5_MusicShowcase.Services
{
    public class SeedCalculator : ISeedCalculator
    {
        public int GetContentSeed(long userSeed, string region, int songIndex)
        {
            return GetStableHash($"{userSeed}_{region}_{songIndex}");
        }

        public int GetLikesSeed(long userSeed, double likesAverage, int songIndex)
        {
            return GetStableHash($"{userSeed}_{likesAverage}_{songIndex}");
        }

        private static int GetStableHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = MD5.HashData(inputBytes);

            return BitConverter.ToInt32(hashBytes, 0);
        }
    }
}