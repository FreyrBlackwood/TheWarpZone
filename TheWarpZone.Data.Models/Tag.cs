public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation Properties
    public ICollection<Media> Media { get; set; } = new List<Media>();
}
