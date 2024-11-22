namespace TheWarpZone.Common.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; } // Optional for display purposes
    }
}
