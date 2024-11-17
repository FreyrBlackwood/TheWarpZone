using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface IUserMediaListService
    {
        Task<IEnumerable<UserMediaList>> GetUserMediaListAsync(string userId);
        Task AddToUserMediaListAsync(UserMediaList userMediaList);
        Task UpdateMediaListStatusAsync(int id, MediaStatus status);
        Task RemoveFromUserMediaListAsync(int id);
    }
}
