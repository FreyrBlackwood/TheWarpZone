using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface ISeasonService
    {
        Task<IEnumerable<SeasonDto>> GetSeasonsByTVShowAsync(int tvShowId);
        Task AddSeasonAsync(SeasonDto seasonDto);
        Task UpdateSeasonAsync(SeasonDto seasonDto);
        Task DeleteSeasonAsync(int id);
    }
}
