using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsForMovieAsync(int movieId);
        Task<IEnumerable<Review>> GetReviewsForTVShowAsync(int tvShowId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
    }
}
