using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.Areas.Admin.ViewModels;
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

        public MovieController(IMovieService movieService, ITagService tagService)
        {
            _movieService = movieService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery, string sortBy, string tags, int pageNumber = 1, int pageSize = 10)
        {
            List<string> tagList = string.IsNullOrEmpty(tags)
                ? new List<string>()
                : tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var paginatedMovies = await _movieService.GetMoviesAsync(pageNumber, pageSize, searchQuery, sortBy, tagList);

            var allTags = await _tagService.GetAllTagsAsync();
            ViewBag.AvailableTags = allTags.Select(t => t.Name).ToList();

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
        public async Task<IActionResult> Details(int id)
        {
            var movieDto = await _movieService.GetMovieDetailsAsync(id);
            if (movieDto == null)
            {
                return NotFound();
            }

            var viewModel = new MovieDetailsViewModel
            {
                Id = movieDto.Id,
                Title = movieDto.Title,
                Description = movieDto.Description,
                Director = movieDto.Director,
                ReleaseDate = movieDto.ReleaseDate,
                ImageUrl = movieDto.ImageUrl,
                AverageRating = movieDto.AverageRating,
                Tags = movieDto.Tags
            };

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
