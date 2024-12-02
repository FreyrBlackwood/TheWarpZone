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
    public class SeasonServiceTests
    {
        private ApplicationDbContext _dbContext;
        private SeasonService _seasonService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _seasonService = new SeasonService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetSeasonsByTVShowAsync_Should_ReturnSeasons()
        {
            // Arrange
            var tvShowId = 1;
            _dbContext.Seasons.AddRange(
                new Season { Title = "Season 1", TVShowId = tvShowId, SeasonNumber = 1 },
                new Season { Title = "Season 2", TVShowId = tvShowId, SeasonNumber = 2 }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _seasonService.GetSeasonsByTVShowAsync(tvShowId);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(s => s.Title == "Season 1"), Is.True);
        }

        [Test]
        public async Task GetSeasonsByTVShowAsync_Should_ReturnEmptyList_When_NoSeasonsExist()
        {
            // Arrange
            var tvShowId = 1;

            // Act
            var result = await _seasonService.GetSeasonsByTVShowAsync(tvShowId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task AddSeasonAsync_Should_AddSeason()
        {
            // Arrange
            var seasonDto = new SeasonDto
            {
                Title = "Season 1",
                TVShowId = 1,
                SeasonNumber = 1
            };

            // Act
            await _seasonService.AddSeasonAsync(seasonDto);

            // Assert
            var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.Title == "Season 1");
            Assert.That(season, Is.Not.Null);
            Assert.That(season.TVShowId, Is.EqualTo(1));
        }

        [Test]
        public void AddSeasonAsync_Should_ThrowError_When_SeasonDtoIsNull()
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _seasonService.AddSeasonAsync(null));

            // Assert
            Assert.That(ex.Message, Does.Contain("Season cannot be null."));
        }

        [Test]
        public async Task AddSeasonAsync_Should_ThrowError_When_SeasonNumberIsNotUnique()
        {
            // Arrange
            _dbContext.Seasons.Add(new Season
            {
                Title = "Season 1",
                TVShowId = 1,
                SeasonNumber = 1
            });
            await _dbContext.SaveChangesAsync();

            var duplicateSeasonDto = new SeasonDto
            {
                Title = "Season 2",
                TVShowId = 1,
                SeasonNumber = 1
            };

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _seasonService.AddSeasonAsync(duplicateSeasonDto));
            // Assert
            Assert.That(ex.Message, Does.Contain("Season number 1 already exists for the TV show."));
        }

        [Test]
        public async Task UpdateSeasonAsync_Should_UpdateSeason()
        {
            // Arrange
            var season = new Season { Title = "Old Title", TVShowId = 1, SeasonNumber = 1 };
            _dbContext.Seasons.Add(season);
            await _dbContext.SaveChangesAsync();

            var updatedSeasonDto = new SeasonDto
            {
                Id = season.Id,
                Title = "New Title",
                TVShowId = 1,
                SeasonNumber = 1
            };

            // Act
            await _seasonService.UpdateSeasonAsync(updatedSeasonDto);

            // Assert
            var updatedSeason = await _dbContext.Seasons.FindAsync(season.Id);
            Assert.That(updatedSeason.Title, Is.EqualTo("New Title"));
        }

        [Test]
        public void UpdateSeasonAsync_Should_ThrowError_When_SeasonNotFound()
        {
            // Arrange
            var seasonDto = new SeasonDto
            {
                Id = 999,
                Title = "Nonexistent Season",
                TVShowId = 1,
                SeasonNumber = 1
            };

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _seasonService.UpdateSeasonAsync(seasonDto));

            // Assert
            Assert.That(ex.Message, Does.Contain("Season with ID 999 not found."));
        }

        [Test]
        public async Task UpdateSeasonAsync_Should_ThrowError_When_SeasonNumberIsNotUnique()
        {
            // Arrange
            _dbContext.Seasons.AddRange(
                new Season { Title = "Season 1", TVShowId = 1, SeasonNumber = 1 },
                new Season { Title = "Season 2", TVShowId = 1, SeasonNumber = 2 }
            );
            await _dbContext.SaveChangesAsync();

            var seasonDto = new SeasonDto
            {
                Id = 1,
                Title = "Updated Season",
                TVShowId = 1,
                SeasonNumber = 2
            };

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _seasonService.UpdateSeasonAsync(seasonDto));

            // Assert
            Assert.That(ex.Message, Does.Contain("Season number 2 already exists for the TV show."));
        }

        [Test]
        public async Task DeleteSeasonAsync_Should_RemoveSeason()
        {
            // Arrange
            var season = new Season { Title = "Season 1", TVShowId = 1, SeasonNumber = 1 };
            _dbContext.Seasons.Add(season);
            await _dbContext.SaveChangesAsync();

            // Act
            await _seasonService.DeleteSeasonAsync(season.Id);

            // Assert
            var deletedSeason = await _dbContext.Seasons.FindAsync(season.Id);
            Assert.That(deletedSeason, Is.Null);
        }

        [Test]
        public void DeleteSeasonAsync_Should_ThrowError_When_SeasonNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _seasonService.DeleteSeasonAsync(999));
            // Assert
            Assert.That(ex.Message, Does.Contain("Season with ID 999 not found."));
        }

        [Test]
        public async Task GetSeasonByIdAsync_Should_ReturnSeason()
        {
            // Arrange
            var season = new Season
            {
                Title = "Season 1",
                TVShowId = 1,
                SeasonNumber = 1
            };
            _dbContext.Seasons.Add(season);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _seasonService.GetSeasonByIdAsync(season.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Season 1"));
        }

        [Test]
        public void GetSeasonByIdAsync_Should_ThrowError_When_SeasonNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _seasonService.GetSeasonByIdAsync(999));

            // Assert

            Assert.That(ex.Message, Does.Contain("Season with ID 999 not found."));
        }
    }
}
