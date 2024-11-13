public class Rating
{
    public int Id { get; set; }
    public int Value { get; set; }

    // Foreign Keys
    public int MediaId { get; set; }
    public Media Media { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
