using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.ViewModels.Review;

namespace TheWarpZone.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int movieId, int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var reviews = await _reviewService.GetPaginatedReviewsForMovieAsync(movieId, pageNumber, pageSize);

            var viewModel = new ReviewListViewModel
            {
                MovieId = movieId,
                Reviews = reviews.Items.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    PostedDate = r.PostedDate,
                    UpdatedAt = r.UpdatedAt,
                    UserId = r.UserId,
                    Email = r.Email
                }),
                CurrentPage = reviews.CurrentPage,
                TotalPages = reviews.TotalPages,
                UserHasReview = reviews.Items.Any(r => r.UserId == userId)
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add(int movieId)
        {
            return View(new ReviewFormViewModel { MovieId = movieId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ReviewFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reviewDto = new ReviewDto
            {
                Comment = model.Comment,
                MovieId = model.MovieId,
                UserId = userId
            };

            await _reviewService.AddReviewAsync(reviewDto);
            return RedirectToAction(nameof(Index), new { movieId = model.MovieId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id, userId);

                var model = new ReviewFormViewModel
                {
                    Id = review.Id,
                    MovieId = review.MovieId ?? 0,
                    Comment = review.Comment
                };

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You do not have permission to edit this review.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Review not found.");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReviewFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reviewDto = new ReviewDto
            {
                Id = model.Id.Value,
                Comment = model.Comment,
                MovieId = model.MovieId,
                UserId = userId
            };

            await _reviewService.UpdateReviewAsync(reviewDto, userId);
            return RedirectToAction("Index", new { movieId = model.MovieId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _reviewService.DeleteReviewAsync(id, userId);
            return RedirectToAction("Index", new { movieId });
        }

        // --- TV Show Methods ---

        [HttpGet]
        public async Task<IActionResult> IndexForTVShow(int tvShowId, int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var reviews = await _reviewService.GetPaginatedReviewsForTVShowAsync(tvShowId, pageNumber, pageSize);

            var viewModel = new ReviewListViewModel
            {
                TVShowId = tvShowId,
                Reviews = reviews.Items.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    PostedDate = r.PostedDate,
                    UpdatedAt = r.UpdatedAt,
                    UserId = r.UserId,
                    Email = r.Email
                }),
                CurrentPage = reviews.CurrentPage,
                TotalPages = reviews.TotalPages,
                UserHasReview = reviews.Items.Any(r => r.UserId == userId)
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddForTVShow(int tvShowId)
        {
            return View(new ReviewFormViewModel { TVShowId = tvShowId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddForTVShow(ReviewFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reviewDto = new ReviewDto
            {
                Comment = model.Comment,
                TVShowId = model.TVShowId,
                UserId = userId
            };

            await _reviewService.AddReviewAsync(reviewDto);
            return RedirectToAction(nameof(IndexForTVShow), new { tvShowId = model.TVShowId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditForTVShow(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id, userId);

                var model = new ReviewFormViewModel
                {
                    Id = review.Id,
                    TVShowId = review.TVShowId ?? 0,
                    Comment = review.Comment
                };

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You do not have permission to edit this review.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Review not found.");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditForTVShow(ReviewFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reviewDto = new ReviewDto
            {
                Id = model.Id.Value,
                Comment = model.Comment,
                TVShowId = model.TVShowId,
                UserId = userId
            };

            await _reviewService.UpdateReviewAsync(reviewDto, userId);
            return RedirectToAction(nameof(IndexForTVShow), new { tvShowId = model.TVShowId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteForTVShow(int id, int tvShowId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _reviewService.DeleteReviewAsync(id, userId);
            return RedirectToAction(nameof(IndexForTVShow), new { tvShowId });
        }
    }
}
