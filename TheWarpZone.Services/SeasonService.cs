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

            existingSeason.SeasonNumber = seasonDto.SeasonNumber;

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
