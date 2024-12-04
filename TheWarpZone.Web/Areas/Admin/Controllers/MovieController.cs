using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.Areas.Admin.ViewModels.Movie;
using TheWarpZone.Web.ViewModels.Shared;
using TheWarpZone.Web.ViewModels.Shared.Movie;

namespace TheWarpZone.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ITagService _tagService;
        private readonly IRatingService _ratingService;

        public MovieController(IMovieService movieService, ITagService tagService, IRatingService ratingService)
        {
            _movieService = movieService;
            _tagService = tagService;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery, string sortBy, List<string> tags, int pageNumber = 1, int pageSize = 10)
        {
            var tagList = tags ?? new List<string>();

            var paginatedMovies = await _movieService.GetMoviesAsync(pageNumber, pageSize, searchQuery, sortBy, tagList);

            var movieTags = await _tagService.GetAllTagsForMoviesAsync();
            ViewBag.AvailableTags = movieTags;

            var viewModel = new PaginatedResultViewModel<MovieGridViewModel>
            {
                Items = paginatedMovies.Items.Select(dto => new MovieGridViewModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    ImageUrl = dto.ImageUrl
                }).ToList(),
                CurrentPage = paginatedMovies.CurrentPage,
                TotalPages = paginatedMovies.TotalPages
            };

            ViewBag.SearchQuery = searchQuery;
            ViewBag.Tags = tagList;
            ViewBag.SortBy = sortBy;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, string searchQuery, string sortBy, List<string> tags, int pageNumber = 1, int pageSize = 10)
        {
            var movieDto = await _movieService.GetMovieDetailsAsync(id);
            if (movieDto == null)
            {
                return NotFound();
            }

            var userId = User.Identity.IsAuthenticated
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var userRating = userId != null
                ? await _ratingService.GetRatingForMovieAsync(userId, id)
                : null;

            var averageRating = await _ratingService.GetAverageRatingForMovieAsync(id);

            var viewModel = new MovieDetailsViewModel
            {
                Id = movieDto.Id,
                Title = movieDto.Title,
                Description = movieDto.Description,
                ReleaseDate = movieDto.ReleaseDate,
                ImageUrl = movieDto.ImageUrl,
                Tags = movieDto.Tags,
                UserRating = userRating?.Value ?? 0,
                AverageRating = averageRating
            };

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.Tags = tags;
            ViewBag.PageNumber = pageNumber;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MovieFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = model.ToDto();
                await _movieService.AddMovieAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movieDto = await _movieService.GetMovieDetailsAsync(id);
            if (movieDto == null)
            {
                return NotFound();
            }

            var viewModel = MovieFormViewModel.FromDto(movieDto);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = model.ToDto();
                await _movieService.UpdateMovieAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var movieDto = await _movieService.GetMovieDetailsAsync(id);
            if (movieDto == null)
            {
                return NotFound();
            }

            var viewModel = new MovieDeleteViewModel
            {
                Id = movieDto.Id,
                Title = movieDto.Title
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieService.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
