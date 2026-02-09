namespace Task5_MusicShowcase.Models.DTOs
{
    public class GenerationRequest
    {
        public string Region { get; set; }
        public long Seed { get; set; }
        public double LikesAverage { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}