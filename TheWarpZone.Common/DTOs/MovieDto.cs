namespace TheWarpZone.Common.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public List<string> Tags { get; set; }
    }
}
