using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface ITVShowService
    {
        Task<IEnumerable<TVShow>> GetAllTVShowsAsync();

        Task<TVShow> GetTVShowDetailsAsync(int id);

        Task AddTVShowAsync(TVShow tvShow);

        Task UpdateTVShowAsync(TVShow tvShow);

        Task DeleteTVShowAsync(int id);

    }
}
