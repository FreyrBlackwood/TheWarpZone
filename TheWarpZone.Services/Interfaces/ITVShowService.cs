using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface ITVShowService
    {
        Task<IEnumerable<TVShowDto>> GetAllTVShowsAsync();
        Task<TVShowDto> GetTVShowDetailsAsync(int id);
        Task AddTVShowAsync(TVShowDto tvShowDto);
        Task UpdateTVShowAsync(TVShowDto tvShowDto);
        Task DeleteTVShowAsync(int id);
    }
}
