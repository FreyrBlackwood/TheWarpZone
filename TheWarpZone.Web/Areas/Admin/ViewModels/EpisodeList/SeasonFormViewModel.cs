using System.ComponentModel.DataAnnotations;

namespace TheWarpZone.Web.Areas.Admin.ViewModels.EpisodeList
{
    public class SeasonFormViewModel
    {
        public int Id { get; set; }
        public int TVShowId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Season number must be 1 or greater.")]
        public int SeasonNumber { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }
    }
}