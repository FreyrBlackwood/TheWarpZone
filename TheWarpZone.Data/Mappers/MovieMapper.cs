using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;

namespace TheWarpZone.Data.Mappers
{
    public static class MovieMapper
    {
        public static MovieDto ToDto(Movie movie)
        {
            if (movie == null) return null;

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                ReleaseDate = movie.ReleaseDate,
                ImageUrl = movie.ImageUrl,
                AverageRating = movie.Ratings.Any() ? movie.Ratings.Average(r => r.Value) : 0,
                Tags = movie.Tags?.Select(t => t.Name).ToList() ?? new List<string>()
            };
        }

        public static Movie ToEntity(MovieDto dto)
        {
            if (dto == null) return null;

            return new Movie
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Director = dto.Director,
                ReleaseDate = dto.ReleaseDate,
                ImageUrl = dto.ImageUrl,
                Tags = dto.Tags?.Select(tagName => new Tag { Name = tagName }).ToList()
            };
        }
    }
}
