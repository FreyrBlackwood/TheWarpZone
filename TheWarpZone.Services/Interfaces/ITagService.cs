using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> GetTagByIdAsync(int id);
        Task AddTagAsync(TagDto tagDto);
        Task DeleteTagAsync(int id);
        Task<IEnumerable<MovieDto>> GetMoviesByTagAsync(int tagId);
        Task<IEnumerable<TVShowDto>> GetTVShowsByTagAsync(int tagId);
    }
}
