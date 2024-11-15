public class Review
{
    public int Id { get; set; }

    public string Comment { get; set; }

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int MediaId { get; set; }
    public Media Media { get; set; } // Navigation property to the media being reviewed

    public string UserId { get; set; }
    public ApplicationUser User { get; set; } // Navigation property to the user posting the review
}
