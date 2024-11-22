using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface IUserMediaListService
    {
        Task<IEnumerable<UserMediaListDto>> GetUserMediaListAsync(string userId);
        Task AddToUserMediaListAsync(UserMediaListDto userMediaListDto);
        Task UpdateMediaListStatusAsync(int id, MediaStatus status);
        Task RemoveFromUserMediaListAsync(int id);
    }
}
