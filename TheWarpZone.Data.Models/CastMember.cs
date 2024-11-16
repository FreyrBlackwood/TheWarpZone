using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class CastMember
{
    public int Id { get; set; }

    [Required]
    [MaxLength(CastMemberConstraints.NameMaxLength)]
    [MinLength(CastMemberConstraints.NameMinLength)]
    public string Name { get; set; }

    [Required]
    [MaxLength(CastMemberConstraints.RoleMaxLength)]
    [MinLength(CastMemberConstraints.RoleMinLength)]
    public string Role { get; set; }

    public int? MovieId { get; set; }
    public Movie Movie { get; set; }

    public int? TVShowId { get; set; }
    public TVShow TVShow { get; set; }
}
