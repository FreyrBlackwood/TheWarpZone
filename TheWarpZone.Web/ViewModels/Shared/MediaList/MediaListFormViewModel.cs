using Microsoft.Build.Framework;

public class MediaListFormViewModel
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Status { get; set; }

    public int? MovieId { get; set; } 

    public int? TVShowId { get; set; }
}