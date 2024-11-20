using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Data;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }
            return tag;
        }

        public async Task AddTagAsync(Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                throw new ArgumentException("Tag name cannot be empty.", nameof(tag.Name));
            }

            var exists = await _context.Tags.AnyAsync(t => t.Name == tag.Name);
            if (exists)
            {
                throw new InvalidOperationException($"A tag with the name '{tag.Name}' already exists.");
            }

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

        public async Task<IEnumerable<Movie>> GetMoviesByTagAsync(int tagId)
        {
            var tag = await _context.Tags
                .Include(t => t.Movies)
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
            }

            return tag.Movies;
        }

        public async Task<IEnumerable<TVShow>> GetTVShowsByTagAsync(int tagId)
        {
            var tag = await _context.Tags
                .Include(t => t.TVShows)
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
            }

            return tag.TVShows;
        }
    }
}
