using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Data.Mappers
{
    public static class UserMediaListMapper
    {
        public static UserMediaListDto ToDto(UserMediaList userMediaList)
        {
            if (userMediaList == null) return null;

            return new UserMediaListDto
            {
                Id = userMediaList.Id,
                MediaTitle = userMediaList.Movie?.Title ?? userMediaList.TVShow?.Title,
                Status = userMediaList.Status.ToString(), // Convert enum to string
                MovieId = userMediaList.MovieId,
                TVShowId = userMediaList.TVShowId,
                UserId = userMediaList.UserId
            };
        }

        public static UserMediaList ToEntity(UserMediaListDto dto)
        {
            if (dto == null) return null;

            return new UserMediaList
            {
                Id = dto.Id,
                Status = Enum.TryParse(dto.Status, out MediaStatus status) ? status : MediaStatus.ToWatch, // Parse string back to enum
                MovieId = dto.MovieId,
                TVShowId = dto.TVShowId,
                UserId = dto.UserId
            };
        }
    }
}
