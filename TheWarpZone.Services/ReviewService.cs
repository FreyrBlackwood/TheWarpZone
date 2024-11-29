using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;
using TheWarpZone.Services.Mappers;
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
                .Include(r => r.User)
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
                .Include(r => r.User)
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
            var user = await _context.Users.FindAsync(reviewDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {reviewDto.UserId} not found.");
            }

            reviewDto.Email = user.Email;

            var entity = ReviewMapper.ToEntity(reviewDto);
            entity.PostedDate = DateTime.UtcNow;
            await _context.Reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int reviewId, string userId)
        {
            var review = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewId} not found.");
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only access your own reviews.");
            }

            return ReviewMapper.ToDto(review);
        }


        public async Task UpdateReviewAsync(ReviewDto reviewDto, string userId)
        {
            if (reviewDto == null)
            {
                throw new ArgumentNullException(nameof(reviewDto), "Review cannot be null.");
            }

            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewDto.Id);

            if (existingReview == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewDto.Id} not found.");
            }

            if (existingReview.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own reviews.");
            }

            existingReview.Comment = reviewDto.Comment;
            existingReview.UpdatedAt = DateTime.UtcNow;

            _context.Reviews.Update(existingReview);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteReviewAsync(int id, string userId)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {id} not found.");
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own reviews.");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

    }
}
