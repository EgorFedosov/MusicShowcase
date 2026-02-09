namespace Task5_MusicShowcase.Models.DTOs
{
    public class SongsResponse
    {
        public List<SongDto> Songs { get; set; }
        public int Page { get; set; }
    }
}