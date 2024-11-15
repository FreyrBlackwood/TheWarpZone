public abstract class Media
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Overview { get; set; } 
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<CastMember> CastMembers { get; set; } = new List<CastMember>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    // Calculated property for average rating
    public double AverageRating
    {
        get
        {
            return Ratings.Any() ? Ratings.Average(r => r.Value) : 0;
        }
    }
}
