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
    public class UserMediaListService : IUserMediaListService
    {
        private readonly ApplicationDbContext _context;

        public UserMediaListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMediaListDto>> GetUserMediaListAsync(string userId)
        {
            var mediaList = await _context.UserMediaLists
                .Where(uml => uml.UserId == userId)
                .Include(uml => uml.Movie)
                .Include(uml => uml.TVShow)
                .ToListAsync();

            return mediaList.Select(UserMediaListMapper.ToDto).ToList();
        }

        public async Task AddToUserMediaListAsync(UserMediaListDto userMediaListDto)
        {
            if (userMediaListDto == null)
            {
                throw new ArgumentNullException(nameof(userMediaListDto), "UserMediaList cannot be null.");
            }

            var entity = UserMediaListMapper.ToEntity(userMediaListDto);

            await _context.UserMediaLists.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMediaListStatusAsync(int id, MediaStatus status)
        {
            var existingMediaList = await _context.UserMediaLists.FindAsync(id);
            if (existingMediaList == null)
            {
                throw new KeyNotFoundException($"UserMediaList with ID {id} not found.");
            }

            existingMediaList.Status = status;

            _context.UserMediaLists.Update(existingMediaList);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromUserMediaListAsync(int id)
        {
            var mediaList = await _context.UserMediaLists.FindAsync(id);
            if (mediaList == null)
            {
                throw new KeyNotFoundException($"UserMediaList with ID {id} not found.");
            }

            _context.UserMediaLists.Remove(mediaList);
            await _context.SaveChangesAsync();
        }
    }
}
