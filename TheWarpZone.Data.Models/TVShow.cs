public class TVShow : Media
{
    public ICollection<Season> Seasons { get; set; } = new List<Season>();
}
