using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface IEpisodeService
    {
        Task<IEnumerable<Episode>> GetEpisodesBySeasonIdAsync(int seasonId);

        Task<Episode> GetEpisodeDetailsAsync(int episodeId);

        Task AddEpisodeAsync(int seasonId, Episode episode);

        Task UpdateEpisodeAsync(Episode episode);

        Task DeleteEpisodeAsync(int episodeId);
    }
}
