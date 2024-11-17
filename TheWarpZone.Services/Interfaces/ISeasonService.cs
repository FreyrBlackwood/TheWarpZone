using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface ISeasonService
    {
        Task<IEnumerable<Season>> GetSeasonsByTVShowIdAsync(int tvShowId);

        Task<Season> GetSeasonDetailsAsync(int seasonId);

        Task AddSeasonAsync(int tvShowId, Season season);

        Task UpdateSeasonAsync(Season season);

        Task DeleteSeasonAsync(int seasonId);
    }
}
