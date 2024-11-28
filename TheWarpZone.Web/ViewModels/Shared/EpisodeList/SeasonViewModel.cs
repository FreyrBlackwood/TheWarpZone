namespace TheWarpZone.Web.ViewModels.Shared.EpisodeList
{
    public class SeasonViewModel
    {
        public int Id { get; set; }
        public int SeasonNumber { get; set; }
        public string Title { get; set; }
        public List<EpisodeViewModel> Episodes { get; set; } = new List<EpisodeViewModel>();
    }
}