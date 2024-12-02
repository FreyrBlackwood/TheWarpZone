using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services;
using TheWarpZone.Common.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheWarpZone.Services.Tests
{
    [TestFixture]
    public class TVShowServiceTests
    {
        private ApplicationDbContext _dbContext;
        private TVShowService _tvShowService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _tvShowService = new TVShowService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetTVShowsAsync_Should_ReturnPaginatedTVShows()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                _dbContext.TVShows.Add(new TVShow
                {
                    Title = $"TV Show {i + 1}",
                    ReleaseDate = DateTime.UtcNow.AddYears(-i)
                });
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tvShowService.GetTVShowsAsync(1, 5);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(5));
            Assert.That(result.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public async Task GetTVShowsAsync_Should_FilterBySearchQuery()
        {
            // Arrange
            _dbContext.TVShows.AddRange(
                new TVShow { Title = "Star Trek", ReleaseDate = DateTime.UtcNow },
                new TVShow { Title = "Star Wars", ReleaseDate = DateTime.UtcNow },
                new TVShow { Title = "Matrix", ReleaseDate = DateTime.UtcNow }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tvShowService.GetTVShowsAsync(1, 5, searchQuery: "Star");

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetTVShowsAsync_Should_FilterByTags()
        {
            // Arrange
            var sciFiTag = new Tag { Name = "Sci-Fi" };
            _dbContext.Tags.Add(sciFiTag);

            _dbContext.TVShows.Add(new TVShow
            {
                Title = "Star Trek",
                Tags = new List<Tag> { sciFiTag }
            });
            _dbContext.TVShows.Add(new TVShow
            {
                Title = "Matrix"
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tvShowService.GetTVShowsAsync(1, 5, tags: new List<string> { "Sci-Fi" });

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.Items.First().Title, Is.EqualTo("Star Trek"));
        }

        [Test]
        public void GetTVShowDetailsAsync_Should_ThrowError_When_TVShowNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _tvShowService.GetTVShowDetailsAsync(999));


            Assert.That(ex.Message, Does.Contain("TV Show with ID 999 not found."));
        }

        [Test]
        public async Task AddTVShowAsync_Should_AddTVShow()
        {
            // Arrange
            var tvShowDto = new TVShowDto
            {
                Title = "Game of Thrones",
                ReleaseDate = new DateTime(2011, 4, 17)
            };

            // Act
            await _tvShowService.AddTVShowAsync(tvShowDto);

            // Assert
            var tvShow = await _dbContext.TVShows.FirstOrDefaultAsync(tv => tv.Title == "Game of Thrones");
            Assert.That(tvShow, Is.Not.Null);
            Assert.That(tvShow.ReleaseDate, Is.EqualTo(new DateTime(2011, 4, 17)));
        }

        [Test]
        public void AddTVShowAsync_Should_ThrowError_When_TVShowDtoIsNull()
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _tvShowService.AddTVShowAsync(null));

            // Assert
            Assert.That(ex.Message, Does.Contain("TV Show data cannot be null."));
        }

        [Test]
        public async Task UpdateTVShowAsync_Should_UpdateTVShow()
        {
            // Arrange
            var tvShow = new TVShow
            {
                Id = 1,
                Title = "Old Title",
                ReleaseDate = new DateTime(2000, 1, 1)
            };
            _dbContext.TVShows.Add(tvShow);
            await _dbContext.SaveChangesAsync();

            var updatedDto = new TVShowDto
            {
                Id = tvShow.Id,
                Title = "New Title",
                ReleaseDate = new DateTime(2022, 1, 1)
            };

            // Act
            await _tvShowService.UpdateTVShowAsync(updatedDto);

            // Assert
            var updatedTVShow = await _dbContext.TVShows.FindAsync(tvShow.Id);
            Assert.That(updatedTVShow.Title, Is.EqualTo("New Title"));
            Assert.That(updatedTVShow.ReleaseDate, Is.EqualTo(new DateTime(2022, 1, 1)));
        }

        [Test]
        public void UpdateTVShowAsync_Should_ThrowError_When_TVShowNotFound()
        {
            // Arrange
            var tvShowDto = new TVShowDto
            {
                Id = 999,
                Title = "Nonexistent Show",
                ReleaseDate = new DateTime(2022, 1, 1)
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _tvShowService.UpdateTVShowAsync(tvShowDto));

            Assert.That(ex.Message, Does.Contain("TV Show with ID 999 not found."));
        }

        [Test]
        public async Task DeleteTVShowAsync_Should_RemoveTVShow()
        {
            // Arrange
            var tvShow = new TVShow
            {
                Title = "Breaking Bad",
                ReleaseDate = new DateTime(2008, 1, 20)
            };
            _dbContext.TVShows.Add(tvShow);
            await _dbContext.SaveChangesAsync();

            // Act
            await _tvShowService.DeleteTVShowAsync(tvShow.Id);

            // Assert
            var deletedTVShow = await _dbContext.TVShows.FindAsync(tvShow.Id);
            Assert.That(deletedTVShow, Is.Null);
        }

        [Test]
        public void DeleteTVShowAsync_Should_ThrowError_When_TVShowNotFound()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _tvShowService.DeleteTVShowAsync(999));

            Assert.That(ex.Message, Does.Contain("TV Show with ID 999 not found."));
        }
    }
}
