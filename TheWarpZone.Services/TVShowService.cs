using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class TVShowService : ITVShowService
    {
        private readonly ApplicationDbContext _context;

        public TVShowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TVShow>> GetAllTVShowsAsync()
        {
            return await _context.TVShows
                .Include(tv => tv.Seasons)
                .ThenInclude(s => s.Episodes)
                .ToListAsync();
        }

        public async Task<TVShow> GetTVShowDetailsAsync(int id)
        {
            var tvShow = await _context.TVShows
                .Include(tv => tv.Seasons)
                    .ThenInclude(s => s.Episodes)
                .FirstOrDefaultAsync(tv => tv.Id == id);

            if (tvShow == null)
            {
                throw new KeyNotFoundException($"TV show with ID {id} not found.");
            }

            return tvShow;
        }

        public async Task AddTVShowAsync(TVShow tvShow)
        {
            await _context.TVShows.AddAsync(tvShow);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTVShowAsync(TVShow tvShow)
        {
            var existingTVShow = await _context.TVShows.FindAsync(tvShow.Id);
            if (existingTVShow == null)
            {
                throw new KeyNotFoundException($"TV show with ID {tvShow.Id} not found.");
            }

            existingTVShow.Title = tvShow.Title;
            existingTVShow.Description = tvShow.Description;
            existingTVShow.ReleaseDate = tvShow.ReleaseDate;
            existingTVShow.ImageUrl = tvShow.ImageUrl;

            _context.TVShows.Update(existingTVShow);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTVShowAsync(int id)
        {
            var tvShow = await _context.TVShows.FindAsync(id);
            if (tvShow == null)
            {
                throw new KeyNotFoundException($"TV show with ID {id} not found.");
            }

            _context.TVShows.Remove(tvShow);
            await _context.SaveChangesAsync();
        }
    }
}
