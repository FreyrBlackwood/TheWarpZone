namespace TheWarpZone.Web.ViewModels.Shared.Rating
{
    public class RatingFormViewModel
    {
        public int MediaId { get; set; }
        public string MediaType { get; set; } // "Movie" or "TVShow"
        public int RatingValue { get; set; }
    }
}