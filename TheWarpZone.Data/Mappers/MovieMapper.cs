using System.Linq;
using System.Collections.Generic;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Common.Mappers
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
    }
}
