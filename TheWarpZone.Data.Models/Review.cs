﻿using System.ComponentModel.DataAnnotations;
using TheWarpZone.Common.Constraints;

public class Review
{
    public int Id { get; set; }

    [Required]
    [MaxLength(ReviewConstraints.CommentMaxLength)]
    [MinLength(ReviewConstraints.CommentMinLength)]
    public string Comment { get; set; }

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    public int? MovieId { get; set; }
    public Movie Movie { get; set; }

    public int? TVShowId { get; set; }
    public TVShow TVShow { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
