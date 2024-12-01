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
    // Some of the methods will be used in the future when I expand the project
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _context.Tags.ToListAsync();
            return tags.Select(TagMapper.ToDto).ToList();
        }

        public async Task<IEnumerable<string>> GetAllTagsForMoviesAsync()
        {
            return await _context.Tags
                .Where(tag => tag.Movies.Any())
                .Select(tag => tag.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllTagsForTVShowsAsync()
        {
            return await _context.Tags
                .Where(tag => tag.TVShows.Any())
                .Select(tag => tag.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<TagDto> GetTagByIdAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }

            return TagMapper.ToDto(tag);
        }

        public async Task AddTagAsync(TagDto tagDto)
        {
            if (tagDto == null)
            {
                throw new ArgumentNullException(nameof(tagDto), "Tag cannot be null.");
            }

            var tag = TagMapper.ToEntity(tagDto);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}
