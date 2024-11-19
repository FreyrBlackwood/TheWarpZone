using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Data;
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

        public async Task<IEnumerable<Review>> GetReviewsForMovieAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsForTVShowAsync(int tvShowId)
        {
            return await _context.Reviews
                .Where(r => r.TVShowId == tvShowId)
                .OrderByDescending(r => r.PostedDate)
                .ToListAsync();
        }

        public async Task AddReviewAsync(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Comment))
            {
                throw new ArgumentException("Review comment cannot be empty.", nameof(review.Comment));
            }

            review.PostedDate = DateTime.UtcNow;
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            var existingReview = await _context.Reviews.FindAsync(review.Id);
            if (existingReview == null)
            {
                throw new KeyNotFoundException($"Review with ID {review.Id} not found.");
            }

            existingReview.Comment = review.Comment;
            existingReview.UpdatedAt = DateTime.UtcNow;

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
