public enum MediaStatus
{
    Watched,
    ToWatch,
    CurrentlyWatching
}

public class UserMediaList
{
    public int Id { get; set; }
    public MediaStatus Status { get; set; }

    // Foreign Keys
    public int MediaId { get; set; }
    public Media Media { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
