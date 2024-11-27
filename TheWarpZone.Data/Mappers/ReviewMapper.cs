using System;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Data.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewDto ToDto(Review review)
        {
            if (review == null) return null;

            return new ReviewDto
            {
                Id = review.Id,
                Comment = review.Comment,
                PostedDate = review.PostedDate,
                UpdatedAt = review.UpdatedAt,
                UserId = review.UserId,
                Email = review.User?.Email,
                MovieId = review.MovieId,
                TVShowId = review.TVShowId
            };
        }

        public static Review ToEntity(ReviewDto reviewDto)
        {
            if (reviewDto == null) return null;

            return new Review
            {
                Id = reviewDto.Id,
                Comment = reviewDto.Comment,
                PostedDate = reviewDto.PostedDate,
                UpdatedAt = reviewDto.UpdatedAt,
                UserId = reviewDto.UserId,
                MovieId = reviewDto.MovieId,
                TVShowId = reviewDto.TVShowId
            };
        }
    }
}