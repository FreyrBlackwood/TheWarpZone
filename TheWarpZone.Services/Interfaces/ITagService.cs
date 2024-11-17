using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Data;

namespace TheWarpZone.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task AddTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        Task<IEnumerable<Movie>> GetMoviesByTagAsync(int tagId);
        Task<IEnumerable<TVShow>> GetTVShowsByTagAsync(int tagId);
    }
}
