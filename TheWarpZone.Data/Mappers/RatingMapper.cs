using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Data.Mappers
{
    public static class RatingMapper
    {
        public static RatingDto ToDto(Rating rating)
        {
            if (rating == null) return null;

            return new RatingDto
            {
                Id = rating.Id,
                Value = rating.Value,
                UserId = rating.UserId,
                UserName = rating.User?.UserName,
                MovieId = rating.MovieId,
                TVShowId = rating.TVShowId
            };
        }

        public static Rating ToEntity(RatingDto dto)
        {
            if (dto == null) return null;

            return new Rating
            {
                Id = dto.Id,
                Value = dto.Value,
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                TVShowId = dto.TVShowId
            };
        }
    }
}
