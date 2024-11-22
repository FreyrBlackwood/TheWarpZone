namespace TheWarpZone.Common.DTOs
{
    public class UserMediaListDto
    {
        public int Id { get; set; }

        public string MediaTitle { get; set; } // Title of the associated Movie or TVShow

        public string Status { get; set; } // Use string to represent the MediaStatus enum

        public int? MovieId { get; set; } // Optional Movie ID

        public int? TVShowId { get; set; } // Optional TVShow ID

        public string UserId { get; set; } // ID of the user who owns this media list entry
    }
}
