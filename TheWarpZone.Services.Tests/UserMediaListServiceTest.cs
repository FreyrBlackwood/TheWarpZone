using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services.Tests
{
    [TestFixture]
    public class UserMediaListServiceTests
    {
        private ApplicationDbContext _dbContext;
        private UserMediaListService _userMediaListService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _userMediaListService = new UserMediaListService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetUserMediaListAsync_Should_ReturnMediaList_HappyPath()
        {
            // Arrange
            var userId = "user123";
            _dbContext.UserMediaLists.AddRange(
                new UserMediaList { UserId = userId, Status = MediaStatus.ToWatch, MovieId = 1 },
                new UserMediaList { UserId = userId, Status = MediaStatus.Watched, TVShowId = 2 }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userMediaListService.GetUserMediaListAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserMediaListAsync_Should_ReturnEmptyList_When_UserHasNoMedia()
        {
            // Arrange
            var userId = "user123";

            // Act
            var result = await _userMediaListService.GetUserMediaListAsync(userId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetUserMediaListAsync_Should_ReturnMixedMediaEntries()
        {
            // Arrange
            var userId = "user123";
            _dbContext.UserMediaLists.AddRange(
                new UserMediaList { UserId = userId, Status = MediaStatus.ToWatch, MovieId = 1 },
                new UserMediaList { UserId = userId, Status = MediaStatus.CurrentlyWatching, TVShowId = 2 }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userMediaListService.GetUserMediaListAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(r => r.MovieId == 1), Is.True);
            Assert.That(result.Any(r => r.TVShowId == 2), Is.True);
        }


        [Test]
        public async Task AddToUserMediaListAsync_Should_AddMediaToList_HappyPath()
        {
            // Arrange
            var userMediaListDto = new UserMediaListDto
            {
                UserId = "user123",
                Status = MediaStatus.ToWatch.ToString(),
                MovieId = 1
            };

            // Act
            await _userMediaListService.AddToUserMediaListAsync(userMediaListDto);

            // Assert
            var result = await _dbContext.UserMediaLists.FirstOrDefaultAsync(u => u.UserId == "user123" && u.MovieId == 1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(MediaStatus.ToWatch));
        }

        [Test]
        public void AddToUserMediaListAsync_Should_ThrowError_When_DtoIsNull()
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _userMediaListService.AddToUserMediaListAsync(null));

            // Assert
            Assert.That(ex.Message, Does.Contain("UserMediaList cannot be null."));
        }

        [Test]
        public void AddToUserMediaListAsync_Should_ThrowError_When_BothMovieAndTVShowIdsAreSet()
        {
            // Arrange
            var invalidMediaListDto = new UserMediaListDto
            {
                UserId = "user123",
                Status = MediaStatus.ToWatch.ToString(),
                MovieId = 1,
                TVShowId = 2
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _userMediaListService.AddToUserMediaListAsync(invalidMediaListDto));

            Assert.That(ex.Message, Does.Contain("Cannot set both MovieId and TVShowId."));
        }


        [Test]
        public async Task UpdateMediaListStatusAsync_Should_UpdateStatus_HappyPath()
        {
            // Arrange
            var userMedia = new UserMediaList
            {
                UserId = "user123",
                Status = MediaStatus.ToWatch,
                MovieId = 1
            };
            _dbContext.UserMediaLists.Add(userMedia);
            await _dbContext.SaveChangesAsync();

            // Act
            await _userMediaListService.UpdateMediaListStatusAsync(userMedia.Id, MediaStatus.Watched);

            // Assert
            var updatedMedia = await _dbContext.UserMediaLists.FindAsync(userMedia.Id);
            Assert.That(updatedMedia.Status, Is.EqualTo(MediaStatus.Watched));
        }

        [Test]
        public void UpdateMediaListStatusAsync_Should_ThrowError_When_IdNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _userMediaListService.UpdateMediaListStatusAsync(999, MediaStatus.Watched));

            // Assert
            Assert.That(ex.Message, Does.Contain("UserMediaList with ID 999 not found."));
        }

        [Test]
        public async Task UpdateMediaListStatusAsync_Should_DoNothing_When_StatusIsUnchanged()
        {
            // Arrange
            var userMedia = new UserMediaList
            {
                UserId = "user123",
                Status = MediaStatus.Watched,
                MovieId = 1
            };
            _dbContext.UserMediaLists.Add(userMedia);
            await _dbContext.SaveChangesAsync();

            // Act
            await _userMediaListService.UpdateMediaListStatusAsync(userMedia.Id, MediaStatus.Watched);

            // Assert
            var unchangedMedia = await _dbContext.UserMediaLists.FindAsync(userMedia.Id);
            Assert.That(unchangedMedia.Status, Is.EqualTo(MediaStatus.Watched));
        }

        [Test]
        public async Task RemoveFromUserMediaListAsync_Should_RemoveMedia_HappyPath()
        {
            // Arrange
            var userMedia = new UserMediaList
            {
                UserId = "user123",
                Status = MediaStatus.ToWatch,
                MovieId = 1
            };
            _dbContext.UserMediaLists.Add(userMedia);
            await _dbContext.SaveChangesAsync();

            // Act
            await _userMediaListService.RemoveFromUserMediaListAsync(userMedia.Id);

            // Assert
            var result = await _dbContext.UserMediaLists.FindAsync(userMedia.Id);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void RemoveFromUserMediaListAsync_Should_ThrowError_When_IdNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _userMediaListService.RemoveFromUserMediaListAsync(999));

            // Assert
            Assert.That(ex.Message, Does.Contain("UserMediaList with ID 999 not found."));
        }

        [Test]
        public async Task RemoveFromUserMediaListAsync_Should_HandleMultipleEntriesForSameUser()
        {
            // Arrange
            var userMedia1 = new UserMediaList
            {
                UserId = "user123",
                Status = MediaStatus.ToWatch,
                MovieId = 1
            };
            var userMedia2 = new UserMediaList
            {
                UserId = "user123",
                Status = MediaStatus.CurrentlyWatching,
                TVShowId = 2
            };

            _dbContext.UserMediaLists.AddRange(userMedia1, userMedia2);
            await _dbContext.SaveChangesAsync();

            // Act
            await _userMediaListService.RemoveFromUserMediaListAsync(userMedia1.Id);

            // Assert
            var remainingMedia = await _dbContext.UserMediaLists.FirstOrDefaultAsync(u => u.Id == userMedia1.Id);
            Assert.That(remainingMedia, Is.Null);

            var secondMedia = await _dbContext.UserMediaLists.FirstOrDefaultAsync(u => u.Id == userMedia2.Id);
            Assert.That(secondMedia, Is.Not.Null);
        }

        [Test]
        public async Task RemoveFromUserMediaListAsync_Should_NotAffectOtherUsersEntries()
        {
            // Arrange
            var userMedia = new UserMediaList
            {
                UserId = "user123",
                Status = MediaStatus.Watched,
                MovieId = 1
            };
            _dbContext.UserMediaLists.Add(userMedia);
            await _dbContext.SaveChangesAsync();

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _userMediaListService.RemoveFromUserMediaListAsync(userMedia.Id + 1));

            // Assert
            Assert.That(ex.Message, Does.Contain($"UserMediaList with ID {userMedia.Id + 1} not found."));
            var remainingMedia = await _dbContext.UserMediaLists.FindAsync(userMedia.Id);
            Assert.That(remainingMedia, Is.Not.Null);
        }

    }
}
