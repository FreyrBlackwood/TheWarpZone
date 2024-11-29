using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Data;
using TheWarpZone.Services.Mappers;
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

        public async Task<IEnumerable<SeasonDto>> GetSeasonsByTVShowAsync(int tvShowId)
        {
            var seasons = await _context.Seasons
                .Where(s => s.TVShowId == tvShowId)
                .Include(s => s.Episodes)
                .ToListAsync();

            return seasons.Select(SeasonMapper.ToDto).ToList();
        }

        public async Task AddSeasonAsync(SeasonDto seasonDto)
        {
            if (seasonDto == null)
            {
                throw new ArgumentNullException(nameof(seasonDto), "Season cannot be null.");
            }

            if (!await IsSeasonNumberUniqueAsync(seasonDto.TVShowId, seasonDto.SeasonNumber))
            {
                throw new InvalidOperationException($"Season number {seasonDto.SeasonNumber} already exists for the TV show.");
            }

            var season = SeasonMapper.ToEntity(seasonDto);

            await _context.Seasons.AddAsync(season);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSeasonAsync(SeasonDto seasonDto)
        {
            if (seasonDto == null)
            {
                throw new ArgumentNullException(nameof(seasonDto), "Season cannot be null.");
            }

            var existingSeason = await _context.Seasons.FindAsync(seasonDto.Id);
            if (existingSeason == null)
            {
                throw new KeyNotFoundException($"Season with ID {seasonDto.Id} not found.");
            }

            if (!await IsSeasonNumberUniqueAsync(seasonDto.TVShowId, seasonDto.SeasonNumber, seasonDto.Id))
            {
                throw new InvalidOperationException($"Season number {seasonDto.SeasonNumber} already exists for the TV show.");
            }

            existingSeason.SeasonNumber = seasonDto.SeasonNumber;
            existingSeason.Title = seasonDto.Title;

            _context.Seasons.Update(existingSeason);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteSeasonAsync(int id)
        {
            var season = await _context.Seasons
                .Include(s => s.Episodes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (season == null)
            {
                throw new KeyNotFoundException($"Season with ID {id} not found.");
            }

            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsSeasonNumberUniqueAsync(int tvShowId, int seasonNumber, int? seasonId = null)
        {
            return !await _context.Seasons
                .AnyAsync(s => s.TVShowId == tvShowId && s.SeasonNumber == seasonNumber && s.Id != seasonId);
        }

        public async Task<SeasonDto> GetSeasonByIdAsync(int seasonId)
        {
            var season = await _context.Seasons
                .Include(s => s.Episodes)
                .FirstOrDefaultAsync(s => s.Id == seasonId);

            if (season == null)
            {
                throw new KeyNotFoundException($"Season with ID {seasonId} not found.");
            }

            return SeasonMapper.ToDto(season);
        }


    }
}
