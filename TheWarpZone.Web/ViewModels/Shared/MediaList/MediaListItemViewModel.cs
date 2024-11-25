namespace TheWarpZone.Web.ViewModels.Shared.MediaList
{
    public class MediaListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public int? MovieId { get; set; }
        public int? TVShowId { get; set; }
    }
}