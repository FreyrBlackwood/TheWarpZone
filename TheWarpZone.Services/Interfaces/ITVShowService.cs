using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface ITVShowService
    {
        Task<PaginatedResultDto<TVShowDto>> GetTVShowsAsync(int pageNumber, int pageSize, string searchQuery = null, string sortBy = null, List<string> tags = null);
        Task<TVShowDto> GetTVShowDetailsAsync(int id);
        Task AddTVShowAsync(TVShowDto tvShowDto);
        Task UpdateTVShowAsync(TVShowDto tvShowDto);
        Task DeleteTVShowAsync(int id);
    }
}
