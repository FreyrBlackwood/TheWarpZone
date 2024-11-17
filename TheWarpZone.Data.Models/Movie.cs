using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class Movie
{
    public int Id { get; set; }

    [Required]
    [MaxLength(MovieConstraints.TitleMaxLength)]
    [MinLength(MovieConstraints.TitleMinLength)]
    public string Title { get; set; }

    [MaxLength(MovieConstraints.DescriptionMaxLength)]
    public string Description { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [MaxLength(MovieConstraints.DirectorMaxLength)]
    [MinLength(MovieConstraints.DirectorMinLength)]
    public string Director { get; set; }

    public string ImageUrl { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<CastMember> CastMembers { get; set; } = new List<CastMember>();
}
