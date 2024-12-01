using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Tests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private ApplicationDbContext _dbContext;
        private ReviewService _reviewService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _reviewService = new ReviewService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }


        [Test]
        public async Task GetPaginatedReviewsForMovieAsync_Should_ReturnCorrectPage_HappyPath()
        {
            // Arrange
            int movieId = 1;
            var movie = new Movie { Id = movieId, Title = "Test Movie", Director = "director1"};
            var user = new ApplicationUser { Id = "user123", Email = "user123@example.com" };

            _dbContext.Movies.Add(movie);
            _dbContext.Users.Add(user);

            for (int i = 0; i < 10; i++)
            {
                _dbContext.Reviews.Add(new Review
                {
                    MovieId = movieId,
                    Comment = $"Review {i + 1}",
                    PostedDate = DateTime.UtcNow.AddMinutes(-i),
                    UserId = "user123",
                    User = user
                });
            }

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _reviewService.GetPaginatedReviewsForMovieAsync(movieId, 1, 5);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(5));
            Assert.That(result.TotalPages, Is.EqualTo(2));
        }


        [Test]
        public async Task GetPaginatedReviewsForMovieAsync_Should_ReturnEmptyList_When_NoReviews()
        {
            // Arrange
            int movieId = 1;

            // Act
            var result = await _reviewService.GetPaginatedReviewsForMovieAsync(movieId, 1, 5);

            // Assert
            Assert.That(result.Items, Is.Empty);
            Assert.That(result.TotalPages, Is.EqualTo(0));
        }

        [Test]
        public async Task GetPaginatedReviewsForMovieAsync_Should_HandleInvalidPageNumber()
        {
            // Arrange
            int movieId = 1;
            _dbContext.Reviews.Add(new Review { MovieId = movieId, Comment = "Review 1", PostedDate = DateTime.UtcNow , UserId = "user123"});
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _reviewService.GetPaginatedReviewsForMovieAsync(movieId, 999, 5);

            // Assert
            Assert.That(result.Items, Is.Empty);
        }


        [Test]
        public async Task AddReviewAsync_Should_AddReview_HappyPath()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", Email = "user1@example.com" };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var reviewDto = new ReviewDto { MovieId = 1, UserId = "user1", Comment = "Great movie!" };

            // Act
            await _reviewService.AddReviewAsync(reviewDto);

            // Assert
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == "user1");
            Assert.That(review, Is.Not.Null);
            Assert.That(review.Comment, Is.EqualTo("Great movie!"));
        }

        [Test]
        public void AddReviewAsync_Should_ThrowError_When_ReviewDtoIsNull()
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _reviewService.AddReviewAsync(null));

            // Assert
            Assert.That(ex.Message, Does.Contain("Review cannot be null."));
        }

        [Test]
        public async Task AddReviewAsync_Should_ThrowError_When_UserNotFound()
        {
            // Arrange
            var reviewDto = new ReviewDto { MovieId = 1, UserId = "nonexistent", Comment = "Great movie!" };

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _reviewService.AddReviewAsync(reviewDto));

            // Assert
            Assert.That(ex.Message, Does.Contain("User with ID nonexistent not found."));
        }


        [Test]
        public async Task GetReviewByIdAsync_Should_ReturnReview_HappyPath()
        {
            // Arrange
            var movie = new Movie { Id = 1, Title = "Test Movie", Director = "direktor"};
            var user = new ApplicationUser { Id = "user1", Email = "user1@example.com" };

            _dbContext.Movies.Add(movie);
            _dbContext.Users.Add(user);

            var review = new Review
            {
                UserId = "user1",
                Comment = "Great movie!",
                MovieId = 1,
                PostedDate = DateTime.UtcNow,
                User = user
            };

            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _reviewService.GetReviewByIdAsync(review.Id, "user1");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comment, Is.EqualTo("Great movie!"));
        }


        [Test]
        public void GetReviewByIdAsync_Should_ThrowError_When_ReviewNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _reviewService.GetReviewByIdAsync(999, "user1"));

            // Assert
            Assert.That(ex.Message, Does.Contain("Review with ID 999 not found."));
        }

        [Test]
        public async Task GetReviewByIdAsync_Should_ThrowError_When_UserIsUnauthorized()
        {
            // Arrange
            var user1 = new ApplicationUser { Id = "user1", Email = "user1@example.com" };
            var user2 = new ApplicationUser { Id = "user2", Email = "user2@example.com" };
            _dbContext.Users.AddRange(user1, user2);

            var review = new Review
            {
                UserId = "user1",
                Comment = "Great movie!",
                MovieId = 1,
                PostedDate = DateTime.UtcNow,
                User = user1
            };
            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();

            // Act
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await _reviewService.GetReviewByIdAsync(review.Id, "user2"));

            // Assert
            Assert.That(ex.Message, Does.Contain("You can only access your own reviews."));
        }


    }
}
