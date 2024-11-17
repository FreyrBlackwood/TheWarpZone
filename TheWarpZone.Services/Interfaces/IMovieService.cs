using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<Movie> GetMovieDetailsAsync(int id);

        Task AddMovieAsync(Movie movie);

        Task UpdateMovieAsync(Movie movie);

        Task DeleteMovieAsync(int id);

    }
}
