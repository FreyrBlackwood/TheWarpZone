using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface IMovieService
    {
        Task<PaginatedResultDto<MovieDto>> GetMoviesAsync(int pageNumber, int pageSize, string searchQuery = null, string sortBy = null, List<string> tags = null);
        Task<MovieDto> GetMovieDetailsAsync(int id);
        Task AddMovieAsync(MovieDto movieDto);
        Task UpdateMovieAsync(MovieDto movieDto);
        Task DeleteMovieAsync(int id);
    }
}
