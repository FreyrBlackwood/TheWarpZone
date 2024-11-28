using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TheWarpZone.Services;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.ViewModels.Shared;
using TheWarpZone.Web.ViewModels.Shared.TVShow;

namespace TheWarpZone.Web.Controllers
{
    public class TVShowController : Controller
    {
        private readonly ITVShowService _tvShowService;
        private readonly ITagService _tagService;
        private readonly IRatingService _ratingService;

        public TVShowController(ITVShowService tvShowService, ITagService tagService, IRatingService ratingService)
        {
            _tvShowService = tvShowService;
            _tagService = tagService;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery, string sortBy, string tags, int pageNumber = 1, int pageSize = 10)
        {
            var tagList = string.IsNullOrEmpty(tags)
                ? new List<string>()
                : tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var paginatedTVShows = await _tvShowService.GetTVShowsAsync(pageNumber, pageSize, searchQuery, sortBy, tagList);

            var tvShowTags = await _tagService.GetAllTagsForTVShowsAsync();
            ViewBag.AvailableTags = tvShowTags;

            var viewModel = new PaginatedResultViewModel<TVShowGridViewModel>
            {
                Items = paginatedTVShows.Items.Select(dto => new TVShowGridViewModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    ImageUrl = dto.ImageUrl
                }).ToList(),
                CurrentPage = paginatedTVShows.CurrentPage,
                TotalPages = paginatedTVShows.TotalPages
            };

            ViewBag.SearchQuery = searchQuery;
            ViewBag.Tags = tagList;
            ViewBag.SortBy = sortBy;

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var tvShowDto = await _tvShowService.GetTVShowDetailsAsync(id);
            if (tvShowDto == null)
            {
                return NotFound();
            }

            var userId = User.Identity.IsAuthenticated
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var userRating = userId != null
                ? await _ratingService.GetRatingForTVShowAsync(userId, id)
            : null;

            var averageRating = await _ratingService.GetAverageRatingForTVShowAsync(id);

            var viewModel = new TVShowDetailsViewModel
            {
                Id = tvShowDto.Id,
                Title = tvShowDto.Title,
                Description = tvShowDto.Description,
                ReleaseDate = tvShowDto.ReleaseDate,
                ImageUrl = tvShowDto.ImageUrl,
                Tags = tvShowDto.Tags,
                UserRating = userRating?.Value ?? 0,
                AverageRating = averageRating 
            };

            return View(viewModel);
        }

    }
}
