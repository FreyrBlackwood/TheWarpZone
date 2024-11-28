using System.ComponentModel.DataAnnotations;

namespace TheWarpZone.Web.Areas.Admin.ViewModels.EpisodeList
{
    public class EpisodeFormViewModel
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int TVShowId { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

    }
}