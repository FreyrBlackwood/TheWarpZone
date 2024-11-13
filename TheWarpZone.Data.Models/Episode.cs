public class Episode
{
    public int Id { get; set; }
    public int EpisodeNumber { get; set; }
    public string Title { get; set; }
    public string EpisodeDescription { get; set; }

    // Foreign Key to Season
    public int SeasonId { get; set; }
    public Season Season { get; set; }
}
