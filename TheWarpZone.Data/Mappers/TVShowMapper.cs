using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Data.Mappers
{
    public static class TVShowMapper
    {
        public static TVShowDto ToDto(TVShow tvShow)
        {
            if (tvShow == null) return null;

            return new TVShowDto
            {
                Id = tvShow.Id,
                Title = tvShow.Title,
                Description = tvShow.Description,
                ReleaseDate = tvShow.ReleaseDate,
                ImageUrl = tvShow.ImageUrl,
                AverageRating = tvShow.Ratings.Any() ? tvShow.Ratings.Average(r => r.Value) : 0,
                Tags = tvShow.Tags?.Select(t => t.Name).ToList() ?? new List<string>(),
                Seasons = tvShow.Seasons?.Select(SeasonMapper.ToDto).ToList() ?? new List<SeasonDto>()
            };
        }

        public static TVShow ToEntity(TVShowDto dto)
        {
            if (dto == null) return null;

            return new TVShow
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                ImageUrl = dto.ImageUrl,
                Tags = dto.Tags?.Select(tagName => new Tag { Name = tagName }).ToList()
            };
        }
    }
}
