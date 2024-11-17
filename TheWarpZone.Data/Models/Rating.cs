using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class Rating
{
    public int Id { get; set; }

    [Required]
    [Range(RatingConstraints.RatingMin, RatingConstraints.RatingMax)]
    public int Value { get; set; }

    public int? MovieId { get; set; }
    public Movie Movie { get; set; }

    public int? TVShowId { get; set; }
    public TVShow TVShow { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
