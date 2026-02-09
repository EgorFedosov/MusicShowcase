namespace Task5_MusicShowcase.Services.Interfaces
{
    public interface ICoverGenerator
    {
        byte[] GenerateCover(long seed, string title, string artist);
    }
}