using Microsoft.AspNetCore.Mvc;
using Task5_MusicShowcase.Models.DTOs;
using Task5_MusicShowcase.Services.Interfaces;

namespace Task5_MusicShowcase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController(
        ISongService songService,
        ICoverGenerator coverGenerator,
        IMusicGenerator musicGenerator)
        : ControllerBase
    {
        private const string ZipMimeType = "application/zip";
        private const string ExportFileName = "songs.zip";
        private const string PngMimeType = "image/png";
        private const string WavMimeType = "audio/wav";

        [HttpGet]
        public ActionResult<SongsResponse> GetBatch([FromQuery] GenerationRequest request)
        {
            var response = songService.GenerateBatch(request);
            return Ok(response);
        }

        [HttpGet("export")]
        public IActionResult GetExport([FromQuery] GenerationRequest request)
        {
            var zipBytes = songService.GenerateExportArchive(request);
            return File(zipBytes, ZipMimeType, ExportFileName);
        }

        [HttpGet("cover")]
        public IActionResult GetCover([FromQuery] long seed, [FromQuery] string title, [FromQuery] string artist)
        {
            var imageBytes = coverGenerator.GenerateCover(seed, title, artist);
            return File(imageBytes, PngMimeType);
        }

        [HttpGet("audio")]
        public IActionResult GetAudio([FromQuery] long seed, [FromQuery] string genre = "Pop")
        {
            var audioBytes = musicGenerator.GenerateTrack(seed, genre);
            return File(audioBytes, WavMimeType);
        }
    }
}