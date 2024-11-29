using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;
using TheWarpZone.Services.Mappers;
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

        public async Task<IEnumerable<EpisodeDto>> GetEpisodesBySeasonAsync(int seasonId)
        {
            var episodes = await _context.Episodes
                .Where(e => e.SeasonId == seasonId)
                .ToListAsync();

            return episodes.Select(EpisodeMapper.ToDto).ToList();
        }

        public async Task<EpisodeDto> GetEpisodeDetailsAsync(int id)
        {
            var episode = await _context.Episodes
                .FirstOrDefaultAsync(e => e.Id == id);

            if (episode == null)
            {
                throw new KeyNotFoundException($"Episode with ID {id} not found.");
            }

            return EpisodeMapper.ToDto(episode);
        }

        public async Task AddEpisodeAsync(EpisodeDto episodeDto)
        {
            if (episodeDto == null)
            {
                throw new ArgumentNullException(nameof(episodeDto), "Episode cannot be null.");
            }

            if (!await IsEpisodeNumberUniqueAsync(episodeDto.SeasonId, episodeDto.EpisodeNumber))
            {
                throw new InvalidOperationException($"Episode number {episodeDto.EpisodeNumber} already exists in the season.");
            }

            var episode = EpisodeMapper.ToEntity(episodeDto);

            await _context.Episodes.AddAsync(episode);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEpisodeAsync(EpisodeDto episodeDto)
        {
            if (episodeDto == null)
            {
                throw new ArgumentNullException(nameof(episodeDto), "Episode cannot be null.");
            }

            var existingEpisode = await _context.Episodes.FindAsync(episodeDto.Id);
            if (existingEpisode == null)
            {
                throw new KeyNotFoundException($"Episode with ID {episodeDto.Id} not found.");
            }

            if (!await IsEpisodeNumberUniqueAsync(episodeDto.SeasonId, episodeDto.EpisodeNumber, episodeDto.Id))
            {
                throw new InvalidOperationException($"Episode number {episodeDto.EpisodeNumber} already exists in the season.");
            }

            existingEpisode.Title = episodeDto.Title;
            existingEpisode.EpisodeDescription = episodeDto.Description;
            existingEpisode.EpisodeNumber = episodeDto.EpisodeNumber;

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

        public async Task<bool> IsEpisodeNumberUniqueAsync(int seasonId, int episodeNumber, int? episodeId = null)
        {
            return !await _context.Episodes
                .AnyAsync(e => e.SeasonId == seasonId && e.EpisodeNumber == episodeNumber && e.Id != episodeId);
        }
    }
}
