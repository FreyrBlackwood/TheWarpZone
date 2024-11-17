using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class Episode
{
    public int Id { get; set; }

    [Required]
    public int EpisodeNumber { get; set; }

    [Required]
    [MaxLength(EpisodeConstraints.EpisodeTitleMaxLength)]
    [MinLength(EpisodeConstraints.EpisodeTitleMinLength)]
    public string Title { get; set; }

    [MaxLength(EpisodeConstraints.EpisodeDescriptionMaxLength)]
    [MinLength(EpisodeConstraints.EpisodeDescriptionMinLength)]
    public string EpisodeDescription { get; set; }

    public int SeasonId { get; set; }
    public Season Season { get; set; }
}
