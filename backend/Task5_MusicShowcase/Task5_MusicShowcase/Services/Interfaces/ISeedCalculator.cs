namespace Task5_MusicShowcase.Services.Interfaces
{
    public interface ISeedCalculator
    {
        int GetContentSeed(long userSeed, string region, int songIndex);
        int GetLikesSeed(long userSeed, double likesAverage, int songIndex);
    }
}