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
    public class MovieServiceTests
    {
        private ApplicationDbContext _dbContext;
        private MovieService _movieService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _movieService = new MovieService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }


        [Test]
        public async Task GetMoviesAsync_Should_ReturnPaginatedMovies_HappyPath()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                _dbContext.Movies.Add(new Movie
                {
                    Title = $"Movie {i + 1}",
                    Director = "Director",
                    ReleaseDate = DateTime.UtcNow.AddYears(-i)
                });
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _movieService.GetMoviesAsync(1, 5);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(5));
            Assert.That(result.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public async Task GetMoviesAsync_Should_FilterBySearchQuery()
        {
            // Arrange
            _dbContext.Movies.AddRange(
                new Movie { Title = "Star Wars", Director = "Director" },
                new Movie { Title = "Star Trek", Director = "Director" },
                new Movie { Title = "Matrix", Director = "Director" }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _movieService.GetMoviesAsync(1, 5, searchQuery: "Star");

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetMoviesAsync_Should_FilterByTags()
        {
            // Arrange
            var sciFiTag = new Tag { Name = "Sci-Fi" };
            _dbContext.Tags.Add(sciFiTag);

            _dbContext.Movies.Add(new Movie { Title = "Star Wars", Director = "Director", Tags = new List<Tag> { sciFiTag } });
            _dbContext.Movies.Add(new Movie { Title = "Matrix", Director = "Director" });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _movieService.GetMoviesAsync(1, 5, tags: new List<string> { "Sci-Fi" });

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.Items.First().Title, Is.EqualTo("Star Wars"));
        }



        [Test]
        public async Task GetMovieDetailsAsync_Should_ReturnMovie_HappyPath()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Inception",
                Director = "Christopher Nolan",
                ReleaseDate = new DateTime(2010, 7, 16)
            };
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _movieService.GetMovieDetailsAsync(movie.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Inception"));
        }

        [Test]
        public void GetMovieDetailsAsync_Should_ThrowError_When_MovieNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _movieService.GetMovieDetailsAsync(999));

            // Assert

            Assert.That(ex.Message, Does.Contain("Movie with ID 999 not found."));
        }



        [Test]
        public async Task AddMovieAsync_Should_AddMovie_HappyPath()
        {
            // Arrange
            var movieDto = new MovieDto
            {
                Title = "Interstellar",
                Director = "Christopher Nolan",
                ReleaseDate = new DateTime(2014, 11, 7)
            };

            // Act
            await _movieService.AddMovieAsync(movieDto);

            // Assert
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == "Interstellar");
            Assert.That(movie, Is.Not.Null);
            Assert.That(movie.Director, Is.EqualTo("Christopher Nolan"));
        }

        [Test]
        public void AddMovieAsync_Should_ThrowError_When_MovieDtoIsNull()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _movieService.AddMovieAsync(null));

            Assert.That(ex.Message, Does.Contain("Movie data cannot be null."));
        }


        [Test]
        public async Task DeleteMovieAsync_Should_RemoveMovie_HappyPath()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Dunkirk",
                Director = "Christopher Nolan",
                ReleaseDate = new DateTime(2017, 7, 21)
            };
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            // Act
            await _movieService.DeleteMovieAsync(movie.Id);

            // Assert
            var deletedMovie = await _dbContext.Movies.FindAsync(movie.Id);
            Assert.That(deletedMovie, Is.Null);
        }

        [Test]
        public void DeleteMovieAsync_Should_ThrowError_When_MovieNotFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _movieService.DeleteMovieAsync(999));

            // Assert
            Assert.That(ex.Message, Does.Contain("Movie with ID 999 not found."));
        }

    }
}
