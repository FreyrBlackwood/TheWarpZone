using System.Collections.Generic;

namespace TheWarpZone.Web.ViewModels.Review
{
    public class ReviewListViewModel
    {
        public int? MovieId { get; set; }
        public int? TVShowId { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool UserHasReview { get; set; }
    }
}