namespace TheWarpZone.Web.ViewModels.Shared.TVShow
{
    public class TVShowDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public double AverageRating { get; set; }
        public int? UserRating { get; set; }
    }
}