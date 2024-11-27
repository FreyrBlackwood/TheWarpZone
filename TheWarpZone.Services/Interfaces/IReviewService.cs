using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface IReviewService
    {
        Task<PaginatedResultDto<ReviewDto>> GetPaginatedReviewsForMovieAsync(int movieId, int pageNumber, int pageSize);
        Task<PaginatedResultDto<ReviewDto>> GetPaginatedReviewsForTVShowAsync(int tvShowId, int pageNumber, int pageSize);
        Task AddReviewAsync(ReviewDto reviewDto);
        Task UpdateReviewAsync(ReviewDto reviewDto, string userId);
        Task DeleteReviewAsync(int id, string userId);
        Task<ReviewDto> GetReviewByIdAsync(int reviewId, string userId);
    }
}
