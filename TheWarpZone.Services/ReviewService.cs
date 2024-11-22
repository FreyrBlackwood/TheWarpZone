using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ReviewDto>> GetReviewsForMovieAsync(int movieId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .Include(r => r.User)
                .ToListAsync();

            return reviews.Select(ReviewMapper.ToDto).ToList();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForTVShowAsync(int tvShowId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.TVShowId == tvShowId)
                .Include(r => r.User)
                .ToListAsync();

            return reviews.Select(ReviewMapper.ToDto).ToList();
        }

        public async Task AddReviewAsync(ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                throw new ArgumentNullException(nameof(reviewDto), "Review cannot be null.");
            }

            var review = ReviewMapper.ToEntity(reviewDto);

            await _context.Reviews.AddAsync(review);
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
            existingReview.PostedDate = reviewDto.PostedDate;

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
