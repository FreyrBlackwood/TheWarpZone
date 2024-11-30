using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Services.Mappers;

namespace TheWarpZone.Services.Tests
{
    [TestFixture]
    public class RatingServiceTests
    {
        private ApplicationDbContext _dbContext;
        private RatingService _ratingService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _ratingService = new RatingService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddOrUpdateRatingForMovieAsync_Should_AddRating_HappyPath()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";
            var ratingValue = 4;

            // Act
            await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, ratingValue, userId);

            // Assert
            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            Assert.That(rating, Is.Not.Null);
            Assert.That(ratingValue, Is.EqualTo(rating.Value));
        }

        [Test]
        public async Task AddOrUpdateRatingForMovieAsync_Should_ThrowError_When_UserIdIsNull()
        {
            // Arrange
            var movieId = 1;
            var ratingValue = 4;

            // Act
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, ratingValue, null));

            // Assert
            Assert.That(ex.Message, Does.Contain("UserId cannot be null or empty."));
        }

        [Test]
        public async Task AddOrUpdateRatingForMovieAsync_Should_ThrowError_When_RatingValueIsInvalid()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";
            var invalidRatingValue = 11;

            // Act
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, invalidRatingValue, userId));

            // Assert
            Assert.That(ex.Message, Does.Contain("Rating value must be between 1 and 5."));
        }


        [Test]
        public async Task AddOrUpdateRatingForMovieAsync_Should_UpdateExistingRating()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";
            _dbContext.Ratings.Add(new Rating { MovieId = movieId, Value = 4, UserId = userId });
            await _dbContext.SaveChangesAsync();

            // Act
            await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, 5, userId);

            // Assert
            var ratings = await _dbContext.Ratings.Where(r => r.MovieId == movieId && r.UserId == userId).ToListAsync();
            Assert.That(ratings.Count, Is.EqualTo(1));
            Assert.That(ratings.First().Value, Is.EqualTo(5));
        }

        [Test]
        public async Task AddOrUpdateRatingForMovieAsync_Should_HandleBoundaryRatingValues()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";

            // Act
            await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, 1, userId);
            await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, 5, userId);

            // Assert
            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            Assert.That(rating.Value, Is.EqualTo(5));
        }



        [Test]
        public async Task GetAverageRatingForMovieAsync_Should_ReturnCorrectAverage_HappyPath()
        {
            // Arrange
            var movieId = 1;
            _dbContext.Ratings.AddRange(
                new Rating { MovieId = movieId, Value = 4, UserId = "user1" },
                new Rating { MovieId = movieId, Value = 5, UserId = "user2" }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ratingService.GetAverageRatingForMovieAsync(movieId);

            // Assert
            Assert.That(4.5, Is.EqualTo(result));
        }

        [Test]
        public async Task GetAverageRatingForMovieAsync_Should_ReturnZero_When_NoRatingsExist()
        {
            // Arrange
            var movieId = 1;

            // Act
            var result = await _ratingService.GetAverageRatingForMovieAsync(movieId);

            // Assert
            Assert.That(0, Is.EqualTo(result));
        }

        [Test]
        public async Task GetAverageRatingForMovieAsync_Should_ReturnZero_When_MovieDoesNotExist()
        {
            // Arrange
            var nonExistentMovieId = 999;

            // Act
            var result = await _ratingService.GetAverageRatingForMovieAsync(nonExistentMovieId);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }


        [Test]
        public async Task DeleteRatingAsync_Should_RemoveRating_HappyPath()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";
            _dbContext.Ratings.Add(new Rating { MovieId = movieId, Value = 4, UserId = userId });
            await _dbContext.SaveChangesAsync();

            // Act
            await _ratingService.DeleteRatingAsync(userId, movieId, null);

            // Assert
            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            Assert.That(rating, Is.Null);
        }

        [Test]
        public async Task DeleteRatingAsync_Should_DoNothing_When_RatingDoesNotExist()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";

            // Act
            await _ratingService.DeleteRatingAsync(userId, movieId, null);

            // Assert
            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            Assert.That(rating, Is.Null.Or.Empty);
        }


        [Test]
        public async Task GetRatingForMovieAsync_Should_ReturnRating_HappyPath()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";
            _dbContext.Ratings.Add(new Rating { MovieId = movieId, Value = 4, UserId = userId });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ratingService.GetRatingForMovieAsync(userId, movieId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(4, Is.EqualTo(result.Value));
        }

        [Test]
        public async Task GetRatingForMovieAsync_Should_ReturnNull_When_RatingDoesNotExist()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";

            // Act
            var result = await _ratingService.GetRatingForMovieAsync(userId, movieId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Should_AddUpdateAndDeleteRatings()
        {
            // Arrange
            var movieId = 1;
            var userId = "user123";

            // Act
            await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, 3, userId);
            var initialAverage = await _ratingService.GetAverageRatingForMovieAsync(movieId);
            await _ratingService.AddOrUpdateRatingForMovieAsync(movieId, 5, userId);
            var updatedAverage = await _ratingService.GetAverageRatingForMovieAsync(movieId);
            await _ratingService.DeleteRatingAsync(userId, movieId, null);
            var finalAverage = await _ratingService.GetAverageRatingForMovieAsync(movieId);

            // Assert
            Assert.That(initialAverage, Is.EqualTo(3));
            Assert.That(updatedAverage, Is.EqualTo(5));
            Assert.That(finalAverage, Is.EqualTo(0));
        }
    }
}
