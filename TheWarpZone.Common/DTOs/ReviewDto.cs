namespace TheWarpZone.Common.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime PostedDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? MovieId { get; set; }
        public int? TVShowId { get; set; }
    }
}
