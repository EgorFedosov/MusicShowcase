namespace Task5_MusicShowcase.Services.Interfaces
{
    public interface IMusicGenerator
    {
        byte[] GenerateTrack(long seed);
    }
}