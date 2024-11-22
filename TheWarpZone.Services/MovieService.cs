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
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Tags)
                .Include(m => m.Ratings)
                .ToListAsync();

            return movies.Select(MovieMapper.ToDto).ToList();
        }

        public async Task<MovieDto> GetMovieDetailsAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Tags)
                .Include(m => m.Ratings)
                .Include(m => m.Reviews)
                .Include(m => m.CastMembers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {id} not found.");
            }

            return MovieMapper.ToDto(movie);
        }

        public async Task AddMovieAsync(MovieDto movieDto)
        {
            if (movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto), "Movie cannot be null.");
            }

            var movie = MovieMapper.ToEntity(movieDto);

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(MovieDto movieDto)
        {
            if (movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto), "Movie cannot be null.");
            }

            var existingMovie = await _context.Movies.FindAsync(movieDto.Id);
            if (existingMovie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movieDto.Id} not found.");
            }

            existingMovie.Title = movieDto.Title;
            existingMovie.Description = movieDto.Description;
            existingMovie.Director = movieDto.Director;
            existingMovie.ReleaseDate = movieDto.ReleaseDate;
            existingMovie.ImageUrl = movieDto.ImageUrl;

            existingMovie.Tags = movieDto.Tags.Select(tagName => new Tag { Name = tagName }).ToList();

            _context.Movies.Update(existingMovie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {id} not found.");
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}
