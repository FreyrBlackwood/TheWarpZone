using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface IRatingService
    {
        Task AddOrUpdateRatingForMovieAsync(int movieId, int ratingValue, string userId);

        Task AddOrUpdateRatingForTVShowAsync(int tvShowId, int ratingValue, string userId);

        Task<double> GetAverageRatingForMovieAsync(int movieId);

        Task<double> GetAverageRatingForTVShowAsync(int tvShowId);
    }
}
