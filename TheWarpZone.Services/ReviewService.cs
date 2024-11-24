using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;
using TheWarpZone.Data.Mappers;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResultDto<ReviewDto>> GetPaginatedReviewsForMovieAsync(int movieId, int pageNumber, int pageSize)
        {
            var totalReviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .CountAsync();

            var reviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.PostedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResultDto<ReviewDto>
            {
                Items = reviews.Select(ReviewMapper.ToDto).ToList(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalReviews / pageSize)
            };
        }

        public async Task<PaginatedResultDto<ReviewDto>> GetPaginatedReviewsForTVShowAsync(int tvShowId, int pageNumber, int pageSize)
        {
            var totalReviews = await _context.Reviews
                .Where(r => r.TVShowId == tvShowId)
                .CountAsync();

            var reviews = await _context.Reviews
                .Where(r => r.TVShowId == tvShowId)
                .OrderByDescending(r => r.PostedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResultDto<ReviewDto>
            {
                Items = reviews.Select(ReviewMapper.ToDto).ToList(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalReviews / pageSize)
            };
        }

        public async Task AddReviewAsync(ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                throw new ArgumentNullException(nameof(reviewDto), "Review cannot be null.");
            }

            var entity = ReviewMapper.ToEntity(reviewDto);
            await _context.Reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                throw new ArgumentNullException(nameof(reviewDto), "Review cannot be null.");
            }

            var existingReview = await _context.Reviews.FindAsync(reviewDto.Id);
            if (existingReview == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewDto.Id} not found.");
            }

            existingReview.Comment = reviewDto.Comment;
            existingReview.PostedDate = DateTime.UtcNow;

            _context.Reviews.Update(existingReview);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {id} not found.");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
