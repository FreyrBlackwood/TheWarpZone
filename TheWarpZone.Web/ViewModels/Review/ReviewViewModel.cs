using System;

namespace TheWarpZone.Web.ViewModels.Review
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}