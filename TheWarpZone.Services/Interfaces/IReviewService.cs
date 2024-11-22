using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsForMovieAsync(int movieId);
        Task<IEnumerable<ReviewDto>> GetReviewsForTVShowAsync(int tvShowId);
        Task AddReviewAsync(ReviewDto reviewDto);
        Task UpdateReviewAsync(ReviewDto reviewDto);
        Task DeleteReviewAsync(int id);
    }
}
