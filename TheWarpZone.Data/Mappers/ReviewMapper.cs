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
                UserId = review.UserId,
                UserName = review.User?.UserName,
                MovieId = review.MovieId,
                TVShowId = review.TVShowId
            };
        }

        public static Review ToEntity(ReviewDto dto)
        {
            if (dto == null) return null;

            return new Review
            {
                Id = dto.Id,
                Comment = dto.Comment,
                PostedDate = dto.PostedDate,
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                TVShowId = dto.TVShowId
            };
        }
    }
}
