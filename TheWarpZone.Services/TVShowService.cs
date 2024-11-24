using Microsoft.EntityFrameworkCore;
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

        public async Task<PaginatedResultDto<TVShowDto>> GetTVShowsAsync(int pageNumber, int pageSize)
        {
            var totalTVShows = await _context.TVShows.CountAsync();

            var tvShows = await _context.TVShows
                .OrderBy(tv => tv.Title)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResultDto<TVShowDto>
            {
                Items = tvShows.Select(TVShowMapper.ToDto).ToList(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalTVShows / pageSize)
            };
        }

        public async Task<TVShowDto> GetTVShowDetailsAsync(int id)
        {
            var tvShow = await _context.TVShows
                .Include(tv => tv.Tags)
                .Include(tv => tv.Ratings)
                .Include(tv => tv.Reviews)
                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                .Include(tv => tv.CastMembers)
                .FirstOrDefaultAsync(tv => tv.Id == id);

            if (tvShow == null)
            {
                throw new KeyNotFoundException($"TV show with ID {id} not found.");
            }

            return TVShowMapper.ToDto(tvShow);
        }

        public async Task AddTVShowAsync(TVShowDto tvShowDto)
        {
            if (tvShowDto == null)
            {
                throw new ArgumentNullException(nameof(tvShowDto), "TV show data cannot be null.");
            }

            var tvShowEntity = TVShowMapper.ToEntity(tvShowDto);

            await _context.TVShows.AddAsync(tvShowEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTVShowAsync(TVShowDto tvShowDto)
        {
            if (tvShowDto == null)
            {
                throw new ArgumentNullException(nameof(tvShowDto), "TV show data cannot be null.");
            }

            var existingTVShow = await _context.TVShows.FindAsync(tvShowDto.Id);
            if (existingTVShow == null)
            {
                throw new KeyNotFoundException($"TV show with ID {tvShowDto.Id} not found.");
            }

            existingTVShow.Title = tvShowDto.Title;
            existingTVShow.Description = tvShowDto.Description;
            existingTVShow.ReleaseDate = tvShowDto.ReleaseDate;
            existingTVShow.ImageUrl = tvShowDto.ImageUrl;

            existingTVShow.Tags = tvShowDto.Tags.Select(tagName =>
                _context.Tags.FirstOrDefault(t => t.Name == tagName) ?? new Tag { Name = tagName }
            ).ToList();

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
