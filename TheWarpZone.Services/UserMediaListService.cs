using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Data;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Services
{
    public class UserMediaListService : IUserMediaListService
    {
        private readonly ApplicationDbContext _context;

        public UserMediaListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMediaList>> GetUserMediaListAsync(string userId)
        {
            return await _context.UserMediaLists
                .Include(uml => uml.Movie)
                .Include(uml => uml.TVShow)
                .Where(uml => uml.UserId == userId)
                .ToListAsync();
        }

        public async Task AddToUserMediaListAsync(UserMediaList userMediaList)
        {
            var exists = await _context.UserMediaLists.AnyAsync(uml =>
                uml.UserId == userMediaList.UserId &&
                ((uml.MovieId.HasValue && uml.MovieId == userMediaList.MovieId) ||
                 (uml.TVShowId.HasValue && uml.TVShowId == userMediaList.TVShowId)));

            if (exists)
            {
                throw new InvalidOperationException("The media item is already in the user's list.");
            }

            await _context.UserMediaLists.AddAsync(userMediaList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMediaListStatusAsync(int id, MediaStatus status)
        {
            var entry = await _context.UserMediaLists.FindAsync(id);
            if (entry == null)
            {
                throw new KeyNotFoundException($"User media list entry with ID {id} not found.");
            }

            entry.Status = status;
            _context.UserMediaLists.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromUserMediaListAsync(int id)
        {
            var entry = await _context.UserMediaLists.FindAsync(id);
            if (entry == null)
            {
                throw new KeyNotFoundException($"User media list entry with ID {id} not found.");
            }

            _context.UserMediaLists.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
