using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class Tag
{
    public int Id { get; set; }

    [MaxLength(TagConstraints.NameMaxLength)]
    public string? Name { get; set; }

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    public ICollection<TVShow> TVShows { get; set; } = new List<TVShow>();
}
