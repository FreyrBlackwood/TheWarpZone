using System.ComponentModel.DataAnnotations;

namespace TheWarpZone.Web.ViewModels.Review
{
    public class ReviewFormViewModel
    {
        public int? Id { get; set; }
        public int? MovieId { get; set; }
        public int? TVShowId { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Comment must be between 10 and 1000 characters.")]
        public string Comment { get; set; }
    }
}