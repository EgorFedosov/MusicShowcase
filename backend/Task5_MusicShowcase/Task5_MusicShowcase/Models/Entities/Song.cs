namespace Task5_MusicShowcase.Models.Entities
{
    public class Song
    {
        public int Index { get; set; }
        public long ContentSeed { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string Review { get; set; }
        public int LikeCount { get; set; }
    }
}