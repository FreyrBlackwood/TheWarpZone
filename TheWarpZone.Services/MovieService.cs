using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;
using TheWarpZone.Services.Mappers;
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

        public async Task<PaginatedResultDto<MovieDto>> GetMoviesAsync(
            int pageNumber,
            int pageSize,
            string searchQuery = null,
            string sortBy = null,
            List<string> tags = null)
        {
            var query = _context.Movies
                .Include(m => m.Tags)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(m => m.Title.Contains(searchQuery));
            }

            if (tags != null && tags.Any())
            {
                query = query.Where(m => m.Tags.Any(tag => tags.Contains(tag.Name)));
            }

            query = sortBy switch
            {
                "Title" => query.OrderBy(m => m.Title),
                "ReleaseDate" => query.OrderByDescending(m => m.ReleaseDate),
                _ => query.OrderBy(m => m.Title)
            };

            var totalMovies = await query.CountAsync();
            var movies = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResultDto<MovieDto>
            {
                Items = movies.Select(MovieMapper.ToDto).ToList(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalMovies / pageSize)
            };
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
                throw new ArgumentNullException(nameof(movieDto), "Movie data cannot be null.");
            }

            var movieEntity = MovieMapper.ToEntity(movieDto);
            if (movieDto.Tags != null && movieDto.Tags.Any())
            {
                var resolvedTags = movieDto.Tags.Select(tagName =>
                    _context.Tags.FirstOrDefault(t => t.Name == tagName) ?? new Tag { Name = tagName }
                ).ToList();

                movieEntity.Tags = resolvedTags;
            }

            await _context.Movies.AddAsync(movieEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(MovieDto movieDto)
        {
            if (movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto), "Movie data cannot be null.");
            }

            var existingMovie = await _context.Movies
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == movieDto.Id);

            if (existingMovie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movieDto.Id} not found.");
            }

            existingMovie.Title = movieDto.Title;
            existingMovie.Description = movieDto.Description;
            existingMovie.Director = movieDto.Director;
            existingMovie.ReleaseDate = movieDto.ReleaseDate;
            existingMovie.ImageUrl = movieDto.ImageUrl;

            if (movieDto.Tags != null && movieDto.Tags.Any())
            {
                var newTags = movieDto.Tags.Select(tagName =>
                    _context.Tags.FirstOrDefault(t => t.Name == tagName) ?? new Tag { Name = tagName }
                ).ToList();

                existingMovie.Tags = existingMovie.Tags
                    .Where(tag => newTags.Any(newTag => newTag.Name == tag.Name))
                    .ToList();

                foreach (var newTag in newTags)
                {
                    if (!existingMovie.Tags.Any(existingTag => existingTag.Name == newTag.Name))
                    {
                        existingMovie.Tags.Add(newTag);
                    }
                }
            }
            else
            {
                existingMovie.Tags.Clear();
            }

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
