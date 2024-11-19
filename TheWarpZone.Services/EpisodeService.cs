using Microsoft.EntityFrameworkCore;
using TheWarpZone.Data;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly ApplicationDbContext _context;

        public EpisodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Episode>> GetEpisodesBySeasonIdAsync(int seasonId)
        {
            return await _context.Episodes
                .Where(e => e.SeasonId == seasonId)
                .OrderBy(e => e.EpisodeNumber)
                .ToListAsync();
        }

        public async Task<Episode> GetEpisodeDetailsAsync(int id)
        {
            var episode = await _context.Episodes
                .Include(e => e.Season)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (episode == null)
            {
                throw new KeyNotFoundException($"Episode with ID {id} not found.");
            }

            return episode;
        }

        public async Task AddEpisodeAsync(int seasonId, Episode episode)
        {
            var season = await _context.Seasons.FindAsync(seasonId);
            if (season == null)
            {
                throw new KeyNotFoundException($"Season with ID {seasonId} not found.");
            }

            episode.SeasonId = seasonId;
            await _context.Episodes.AddAsync(episode);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEpisodeAsync(Episode episode)
        {
            var existingEpisode = await _context.Episodes.FindAsync(episode.Id);
            if (existingEpisode == null)
            {
                throw new KeyNotFoundException($"Episode with ID {episode.Id} not found.");
            }

            existingEpisode.Title = episode.Title;
            existingEpisode.EpisodeDescription = episode.EpisodeDescription;
            existingEpisode.EpisodeNumber = episode.EpisodeNumber;

            _context.Episodes.Update(existingEpisode);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEpisodeAsync(int id)
        {
            var episode = await _context.Episodes.FindAsync(id);
            if (episode == null)
            {
                throw new KeyNotFoundException($"Episode with ID {id} not found.");
            }

            _context.Episodes.Remove(episode);
            await _context.SaveChangesAsync();
        }
    }
}
