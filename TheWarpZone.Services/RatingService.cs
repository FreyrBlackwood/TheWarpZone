using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;
using TheWarpZone.Data.Mappers;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateRatingForMovieAsync(int movieId, int ratingValue, string userId)
        {
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Value = ratingValue;
                _context.Ratings.Update(existingRating);
            }
            else
            {
                var newRating = new RatingDto
                {
                    Value = ratingValue,
                    MovieId = movieId,
                    UserId = userId
                };

                var entity = RatingMapper.ToEntity(newRating);
                await _context.Ratings.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateRatingForTVShowAsync(int tvShowId, int ratingValue, string userId)
        {
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.TVShowId == tvShowId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Value = ratingValue;
                _context.Ratings.Update(existingRating);
            }
            else
            {
                var newRating = new RatingDto
                {
                    Value = ratingValue,
                    TVShowId = tvShowId,
                    UserId = userId
                };

                var entity = RatingMapper.ToEntity(newRating);
                await _context.Ratings.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRatingAsync(string userId, int movieId, int? tvShowId)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r =>
                    r.UserId == userId &&
                    (r.MovieId == movieId || (tvShowId.HasValue && r.TVShowId == tvShowId.Value)));

            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RatingDto> GetRatingForMovieAsync(string userId, int movieId)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);

            return RatingMapper.ToDto(rating);
        }

        public async Task<RatingDto> GetRatingForTVShowAsync(string userId, int tvShowId)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.TVShowId == tvShowId);

            return RatingMapper.ToDto(rating);
        }

        public async Task<double> GetAverageRatingForMovieAsync(int movieId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            return ratings.Any() ? ratings.Average(r => r.Value) : 0;
        }

        public async Task<double> GetAverageRatingForTVShowAsync(int tvShowId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.TVShowId == tvShowId)
                .ToListAsync();

            return ratings.Any() ? ratings.Average(r => r.Value) : 0;
        }
    }
}
