using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class TVShow
{
    public int Id { get; set; }

    [Required]
    [MaxLength(TVShowConstraints.TitleMaxLength)]
    [MinLength(TVShowConstraints.TitleMinLength)]
    public string Title { get; set; }

    [MaxLength(TVShowConstraints.DescriptionMaxLength)]
    public string Description { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    public ICollection<Season> Seasons { get; set; } = new List<Season>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<CastMember> CastMembers { get; set; } = new List<CastMember>();
}
