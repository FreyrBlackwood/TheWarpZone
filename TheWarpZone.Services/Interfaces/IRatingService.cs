using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface IRatingService
    {
        Task AddOrUpdateRatingForMovieAsync(int movieId, int ratingValue, string userId);
        Task AddOrUpdateRatingForTVShowAsync(int tvShowId, int ratingValue, string userId);
        Task DeleteRatingAsync(string userId, int movieId, int? tvShowId);
        Task<RatingDto> GetRatingForMovieAsync(string userId, int movieId);
        Task<RatingDto> GetRatingForTVShowAsync(string userId, int tvShowId);
        Task<double> GetAverageRatingForMovieAsync(int movieId);
        Task<double> GetAverageRatingForTVShowAsync(int tvShowId);
    }
}