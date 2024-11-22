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
    public class TVShowService : ITVShowService
    {
        private readonly ApplicationDbContext _context;

        public TVShowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TVShowDto>> GetAllTVShowsAsync()
        {
            var tvShows = await _context.TVShows
                .Include(tv => tv.Tags)
                .Include(tv => tv.Ratings)
                .ToListAsync();

            return tvShows.Select(TVShowMapper.ToDto).ToList();
        }

        public async Task<TVShowDto> GetTVShowDetailsAsync(int id)
        {
            var tvShow = await _context.TVShows
                .Include(tv => tv.Tags)
                .Include(tv => tv.Ratings)
                .Include(tv => tv.Seasons)
                    .ThenInclude(s => s.Episodes)
                .Include(tv => tv.CastMembers)
                .FirstOrDefaultAsync(tv => tv.Id == id);

            if (tvShow == null)
            {
                throw new KeyNotFoundException($"TV Show with ID {id} not found.");
            }

            return TVShowMapper.ToDto(tvShow);
        }

        public async Task AddTVShowAsync(TVShowDto tvShowDto)
        {
            if (tvShowDto == null)
            {
                throw new ArgumentNullException(nameof(tvShowDto), "TV Show cannot be null.");
            }

            var tvShow = TVShowMapper.ToEntity(tvShowDto);

            await _context.TVShows.AddAsync(tvShow);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTVShowAsync(TVShowDto tvShowDto)
        {
            if (tvShowDto == null)
            {
                throw new ArgumentNullException(nameof(tvShowDto), "TV Show cannot be null.");
            }

            var existingTVShow = await _context.TVShows.FindAsync(tvShowDto.Id);
            if (existingTVShow == null)
            {
                throw new KeyNotFoundException($"TV Show with ID {tvShowDto.Id} not found.");
            }

            existingTVShow.Title = tvShowDto.Title;
            existingTVShow.Description = tvShowDto.Description;
            existingTVShow.ReleaseDate = tvShowDto.ReleaseDate;
            existingTVShow.ImageUrl = tvShowDto.ImageUrl;

            existingTVShow.Tags = tvShowDto.Tags.Select(tagName => new Tag { Name = tagName }).ToList();

            _context.TVShows.Update(existingTVShow);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTVShowAsync(int id)
        {
            var tvShow = await _context.TVShows.FindAsync(id);
            if (tvShow == null)
            {
                throw new KeyNotFoundException($"TV Show with ID {id} not found.");
            }

            _context.TVShows.Remove(tvShow);
            await _context.SaveChangesAsync();
        }
    }
}
