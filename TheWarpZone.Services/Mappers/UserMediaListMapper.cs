using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Mappers
{
    public static class UserMediaListMapper
    {
        public static UserMediaListDto ToDto(UserMediaList userMediaList)
        {
            if (userMediaList == null) return null;

            return new UserMediaListDto
            {
                Id = userMediaList.Id,
                MediaTitle = userMediaList.Movie?.Title ?? userMediaList.TVShow?.Title, // Safely get the title
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
                Status = Enum.TryParse(dto.Status, true, out MediaStatus status) ? status : MediaStatus.ToWatch, // Safely parse enum
                MovieId = dto.MovieId,
                TVShowId = dto.TVShowId,
                UserId = dto.UserId
            };
        }
    }
}