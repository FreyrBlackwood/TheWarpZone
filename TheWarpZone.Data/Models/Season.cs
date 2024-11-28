using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class Season
{
    public int Id { get; set; }

    [Required]
    [Range(SeasonConstraints.SeasonNumberMin, int.MaxValue)]
    public int SeasonNumber { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();

    public int TVShowId { get; set; }
    public TVShow TVShow { get; set; }
}
