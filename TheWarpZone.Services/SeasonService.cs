using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ApplicationDbContext _context;

        public SeasonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Season>> GetSeasonsByTVShowIdAsync(int tvShowId)
        {
            return await _context.Seasons
                .Where(s => s.TVShowId == tvShowId)
                .OrderBy(s => s.SeasonNumber)
                .ToListAsync();
        }

        public async Task<Season> GetSeasonDetailsAsync(int id)
        {
            var season = await _context.Seasons
                .Include(s => s.Episodes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (season == null)
            {
                throw new KeyNotFoundException($"Season with ID {id} not found.");
            }

            return season;
        }

        public async Task AddSeasonAsync(int tvShowId, Season season)
        {
            var tvShow = await _context.TVShows.FindAsync(tvShowId);
            if (tvShow == null)
            {
                throw new KeyNotFoundException($"TV show with ID {tvShowId} not found.");
            }

            season.TVShowId = tvShowId;
            await _context.Seasons.AddAsync(season);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSeasonAsync(Season season)
        {
            var existingSeason = await _context.Seasons.FindAsync(season.Id);
            if (existingSeason == null)
            {
                throw new KeyNotFoundException($"Season with ID {season.Id} not found.");
            }

            existingSeason.SeasonNumber = season.SeasonNumber;

            _context.Seasons.Update(existingSeason);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSeasonAsync(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                throw new KeyNotFoundException($"Season with ID {id} not found.");
            }

            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();
        }
    }
}
