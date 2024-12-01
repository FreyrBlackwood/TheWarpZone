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
    public class TagServiceTests
    {
        private ApplicationDbContext _dbContext;
        private TagService _tagService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _tagService = new TagService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllTagsAsync_Should_ReturnAllTags_HappyPath()
        {
            // Arrange
            _dbContext.Tags.AddRange(
                new Tag { Name = "Sci-Fi" },
                new Tag { Name = "Action" }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tagService.GetAllTagsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllTagsForMoviesAsync_Should_ReturnTagsForMovies_HappyPath()
        {
            // Arrange
            var movieTag = new Tag { Name = "Sci-Fi", Movies = new List<Movie> { new Movie { Title = "Movie1", Director = "Director1"} } };
            var unrelatedTag = new Tag { Name = "Cooking" };
            _dbContext.Tags.AddRange(movieTag, unrelatedTag);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tagService.GetAllTagsForMoviesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("Sci-Fi"));
        }

        [Test]
        public async Task GetTagByIdAsync_Should_ReturnTag_HappyPath()
        {
            // Arrange
            var tag = new Tag { Name = "Sci-Fi" };
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tagService.GetTagByIdAsync(tag.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Sci-Fi"));
        }

        [Test]
        public void GetTagByIdAsync_Should_ThrowError_When_IdNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _tagService.GetTagByIdAsync(999));

            // Assert
            Assert.That(ex.Message, Does.Contain("Tag with ID 999 not found."));
        }

        [Test]
        public async Task AddTagAsync_Should_AddTag_HappyPath()
        {
            // Arrange
            var tagDto = new TagDto { Name = "Horror" };

            // Act
            await _tagService.AddTagAsync(tagDto);

            // Assert
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == "Horror");
            Assert.That(tag, Is.Not.Null);
            Assert.That(tag.Name, Is.EqualTo("Horror"));
        }

        [Test]
        public void AddTagAsync_Should_ThrowError_When_TagDtoIsNull()
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _tagService.AddTagAsync(null));

            // Assert
            Assert.That(ex.Message, Does.Contain("Tag cannot be null."));
        }

        [Test]
        public async Task DeleteTagAsync_Should_RemoveTag_HappyPath()
        {
            // Arrange
            var tag = new Tag { Name = "Adventure" };
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();

            // Act
            await _tagService.DeleteTagAsync(tag.Id);

            // Assert
            var deletedTag = await _dbContext.Tags.FindAsync(tag.Id);
            Assert.That(deletedTag, Is.Null);
        }

        [Test]
        public void DeleteTagAsync_Should_ThrowError_When_IdNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _tagService.DeleteTagAsync(999));
            // Assert
            Assert.That(ex.Message, Does.Contain("Tag with ID 999 not found."));
        }
    }
}
