public class CastMember
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; } 

    // Foreign Key to Media
    public int MediaId { get; set; }
    public Media Media { get; set; }
}
