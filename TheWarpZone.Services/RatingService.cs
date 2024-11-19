using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TheWarpZone.Data;
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
            if (ratingValue < 1 || ratingValue > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(ratingValue), "Rating value must be between 1 and 5.");
            }

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Value = ratingValue;
                _context.Ratings.Update(existingRating);
            }
            else
            {
                var newRating = new Rating
                {
                    MovieId = movieId,
                    Value = ratingValue,
                    UserId = userId
                };
                await _context.Ratings.AddAsync(newRating);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateRatingForTVShowAsync(int tvShowId, int ratingValue, string userId)
        {
            if (ratingValue < 1 || ratingValue > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(ratingValue), "Rating value must be between 1 and 5.");
            }

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.TVShowId == tvShowId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Value = ratingValue;
                _context.Ratings.Update(existingRating);
            }
            else
            {
                var newRating = new Rating
                {
                    TVShowId = tvShowId,
                    Value = ratingValue,
                    UserId = userId
                };
                await _context.Ratings.AddAsync(newRating);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingForMovieAsync(int movieId)
        {
            var averageRating = await _context.Ratings
                .Where(r => r.MovieId == movieId)
                .AverageAsync(r => (double?)r.Value);

            return averageRating ?? 0.0;
        }

        public async Task<double> GetAverageRatingForTVShowAsync(int tvShowId)
        {
            var averageRating = await _context.Ratings
                .Where(r => r.TVShowId == tvShowId)
                .AverageAsync(r => (double?)r.Value);

            return averageRating ?? 0.0;
        }
    }
}
