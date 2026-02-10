using Task5_MusicShowcase.Models.DTOs;

namespace Task5_MusicShowcase.Services.Interfaces;

public interface ISongService
{
    SongsResponse GenerateBatch(GenerationRequest request);
    byte[] GenerateExportArchive(GenerationRequest request);
}