using System.Collections.Generic;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;

namespace TheWarpZone.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<IEnumerable<string>> GetAllTagsForMoviesAsync();
        Task<IEnumerable<string>> GetAllTagsForTVShowsAsync();
        Task<TagDto> GetTagByIdAsync(int id);
        Task AddTagAsync(TagDto tagDto);
        Task DeleteTagAsync(int id);
    }
}