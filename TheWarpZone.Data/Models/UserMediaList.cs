using System.ComponentModel.DataAnnotations;

public enum MediaStatus
{
    Watched,
    ToWatch,
    CurrentlyWatching
}

public class UserMediaList
{
    public int Id { get; set; }

    [Required]
    public MediaStatus Status { get; set; }

    public int? MovieId { get; set; }
    public Movie Movie { get; set; }

    public int? TVShowId { get; set; }
    public TVShow TVShow { get; set; }

    [Required]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
