using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<UserMediaList> UserMediaLists { get; set; } = new List<UserMediaList>();
}
