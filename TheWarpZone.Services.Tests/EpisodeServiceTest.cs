using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Tests
{
    [TestFixture]
    public class EpisodeServiceTests
    {
        private ApplicationDbContext _dbContext;
        private EpisodeService _episodeService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _episodeService = new EpisodeService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetEpisodesBySeasonAsync_Should_ReturnEpisodes()
        {
            // Arrange
            var seasonId = 1;
            _dbContext.Episodes.AddRange(
                new Episode { Title = "Episode 1", EpisodeDescription = "Description 1", SeasonId = seasonId, EpisodeNumber = 1 },
                new Episode { Title = "Episode 2", EpisodeDescription = "Description 2", SeasonId = seasonId, EpisodeNumber = 2 }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _episodeService.GetEpisodesBySeasonAsync(seasonId);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(e => e.Title == "Episode 1"), Is.True);
        }

        [Test]
        public async Task GetEpisodesBySeasonAsync_Should_ReturnEmpty_When_NoEpisodesExist()
        {
            // Arrange
            var seasonId = 1;

            // Act
            var result = await _episodeService.GetEpisodesBySeasonAsync(seasonId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetEpisodeDetailsAsync_Should_ReturnEpisode()
        {
            // Arrange
            var episode = new Episode
            {
                Title = "Episode 1",
                EpisodeDescription = "Description 1",
                SeasonId = 1,
                EpisodeNumber = 1
            };
            _dbContext.Episodes.Add(episode);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _episodeService.GetEpisodeDetailsAsync(episode.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Episode 1"));
        }

        [Test]
        public void GetEpisodeDetailsAsync_Should_ThrowError_When_EpisodeNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _episodeService.GetEpisodeDetailsAsync(999));

            // Assert
            Assert.That(ex.Message, Does.Contain("Episode with ID 999 not found."));
        }

        [Test]
        public async Task AddEpisodeAsync_Should_AddEpisode()
        {
            // Arrange
            var episodeDto = new EpisodeDto
            {
                Title = "Episode 1",
                Description = "Description 1",
                SeasonId = 1,
                EpisodeNumber = 1
            };

            // Act
            await _episodeService.AddEpisodeAsync(episodeDto);

            // Assert
            var episode = await _dbContext.Episodes.FirstOrDefaultAsync(e => e.Title == "Episode 1");
            Assert.That(episode, Is.Not.Null);
            Assert.That(episode.EpisodeDescription, Is.EqualTo("Description 1"));
        }

        [Test]
        public void AddEpisodeAsync_Should_ThrowError_When_EpisodeDtoIsNull()
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _episodeService.AddEpisodeAsync(null));

            // Assert
            Assert.That(ex.Message, Does.Contain("Episode cannot be null."));
        }

        [Test]
        public async Task AddEpisodeAsync_Should_ThrowError_When_EpisodeNumberIsNotUnique()
        {
            // Arrange
            _dbContext.Episodes.Add(new Episode
            {
                Title = "Episode 1",
                EpisodeDescription = "Description 1",
                SeasonId = 1,
                EpisodeNumber = 1
            });
            await _dbContext.SaveChangesAsync();

            var duplicateEpisode = new EpisodeDto
            {
                Title = "Episode 2",
                Description = "Description 2",
                SeasonId = 1,
                EpisodeNumber = 1
            };

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _episodeService.AddEpisodeAsync(duplicateEpisode));

            // Assert
            Assert.That(ex.Message, Does.Contain("Episode number 1 already exists in the season."));
        }

        [Test]
        public async Task UpdateEpisodeAsync_Should_UpdateEpisode()
        {
            // Arrange
            var episode = new Episode
            {
                Title = "Old Title",
                EpisodeDescription = "Old Description",
                SeasonId = 1,
                EpisodeNumber = 1
            };
            _dbContext.Episodes.Add(episode);
            await _dbContext.SaveChangesAsync();

            var updatedDto = new EpisodeDto
            {
                Id = episode.Id,
                Title = "New Title",
                Description = "New Description",
                SeasonId = 1,
                EpisodeNumber = 1
            };

            // Act
            await _episodeService.UpdateEpisodeAsync(updatedDto);

            // Assert
            var updatedEpisode = await _dbContext.Episodes.FindAsync(episode.Id);
            Assert.That(updatedEpisode.Title, Is.EqualTo("New Title"));
            Assert.That(updatedEpisode.EpisodeDescription, Is.EqualTo("New Description"));
        }

        [Test]
        public void UpdateEpisodeAsync_Should_ThrowError_When_EpisodeNotFound()
        {
            // Arrange
            var episodeDto = new EpisodeDto
            {
                Id = 999,
                Title = "Nonexistent Episode",
                Description = "Nonexistent Description",
                SeasonId = 1,
                EpisodeNumber = 1
            };

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _episodeService.UpdateEpisodeAsync(episodeDto));

            // Assert
            Assert.That(ex.Message, Does.Contain("Episode with ID 999 not found."));
        }

        [Test]
        public async Task DeleteEpisodeAsync_Should_RemoveEpisode()
        {
            // Arrange
            var episode = new Episode
            {
                Title = "Episode 1",
                EpisodeDescription = "Description 1",
                SeasonId = 1,
                EpisodeNumber = 1
            };
            _dbContext.Episodes.Add(episode);
            await _dbContext.SaveChangesAsync();

            // Act
            await _episodeService.DeleteEpisodeAsync(episode.Id);

            // Assert
            var deletedEpisode = await _dbContext.Episodes.FindAsync(episode.Id);
            Assert.That(deletedEpisode, Is.Null);
        }

        [Test]
        public void DeleteEpisodeAsync_Should_ThrowError_When_EpisodeNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _episodeService.DeleteEpisodeAsync(999));

            // Assert
            Assert.That(ex.Message, Does.Contain("Episode with ID 999 not found."));
        }
    }
}
