using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface IEpisodeService
    {
        Task<IEnumerable<EpisodeDto>> GetEpisodesBySeasonAsync(int seasonId);
        Task<EpisodeDto> GetEpisodeDetailsAsync(int id);
        Task AddEpisodeAsync(EpisodeDto episodeDto);
        Task UpdateEpisodeAsync(EpisodeDto episodeDto);
        Task DeleteEpisodeAsync(int id);
    }
}
