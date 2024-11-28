using System.Collections.Generic;

namespace TheWarpZone.Web.ViewModels.Shared.EpisodeList
{
    public class EpisodeListViewModel
    {
        public int TVShowId { get; set; }
        public string TVShowTitle { get; set; }
        public List<SeasonViewModel> Seasons { get; set; } = new List<SeasonViewModel>();
        public int? ExpandedSeasonId { get; set; }
    }
}