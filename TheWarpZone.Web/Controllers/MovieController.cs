using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TheWarpZone.Services;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.Areas.Admin.ViewModels;
using TheWarpZone.Web.ViewModels;
using TheWarpZone.Web.ViewModels.Shared;
using TheWarpZone.Web.ViewModels.Shared.Movie;

namespace TheWarpZone.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IRatingService _ratingService;

        public MovieController(IMovieService movieService, IRatingService ratingService)
        {
            _movieService = movieService;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery, string sortBy, List<string> tags, int pageNumber = 1, int pageSize = 10)
        {
            var paginatedMovies = await _movieService.GetMoviesAsync(pageNumber, pageSize, searchQuery, sortBy, tags);

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
            ViewBag.Tags = tags;
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

            var userId = User.Identity.IsAuthenticated
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var userRating = 0;
            if (!string.IsNullOrEmpty(userId))
            {
                var ratingDto = await _ratingService.GetRatingForMovieAsync(userId, id);
                userRating = ratingDto?.Value ?? 0;
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
                Tags = movieDto.Tags,
                UserRating = userRating
            };

            return View(viewModel);
        }


    }
}
