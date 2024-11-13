public class Season
{
    public int Id { get; set; }
    public int SeasonNumber { get; set; }

    // Foreign Key to TVShow
    public int TVShowId { get; set; }
    public TVShow TVShow { get; set; }

    // Navigation Property for Episodes
    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
}
