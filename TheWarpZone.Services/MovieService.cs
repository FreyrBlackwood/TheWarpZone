using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
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

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies
                .Include(m => m.Tags)
                .Include(m => m.Ratings)
                .Include(m => m.Reviews)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieDetailsAsync(int id)
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

            return movie;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "Movie cannot be null.");
            }

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "Movie cannot be null.");
            }

            var existingMovie = await _context.Movies.FindAsync(movie.Id);
            if (existingMovie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movie.Id} not found.");
            }

            existingMovie.Title = movie.Title;
            existingMovie.Description = movie.Description;
            existingMovie.Director = movie.Director;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.ImageUrl = movie.ImageUrl;

            existingMovie.Tags = movie.Tags;

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
