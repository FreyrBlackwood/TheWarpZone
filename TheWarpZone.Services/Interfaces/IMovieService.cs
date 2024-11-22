using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
        Task<MovieDto> GetMovieDetailsAsync(int id);
        Task AddMovieAsync(MovieDto movieDto);
        Task UpdateMovieAsync(MovieDto movieDto);
        Task DeleteMovieAsync(int id);
    }
}
