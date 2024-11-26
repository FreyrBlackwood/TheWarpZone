using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Web.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateRating(int mediaId, string mediaType, int ratingValue)
        {
            if (ratingValue < 1 || ratingValue > 5)
            {
                return BadRequest("Rating value must be between 1 and 5.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                if (mediaType == "Movie")
                {
                    await _ratingService.AddOrUpdateRatingForMovieAsync(mediaId, ratingValue, userId);
                }
                else if (mediaType == "TVShow")
                {
                    await _ratingService.AddOrUpdateRatingForTVShowAsync(mediaId, ratingValue, userId);
                }
                else
                {
                    return BadRequest("Invalid media type.");
                }

                double averageRating = mediaType == "Movie"
                    ? await _ratingService.GetAverageRatingForMovieAsync(mediaId)
                    : await _ratingService.GetAverageRatingForTVShowAsync(mediaId);

                return Json(new
                {
                    success = true,
                    message = "Rating submitted successfully.",
                    newAverageRating = averageRating
                });
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRating(int mediaId, string mediaType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                if (mediaType == "Movie")
                {
                    await _ratingService.DeleteRatingAsync(userId, mediaId, null);
                }
                else if (mediaType == "TVShow")
                {
                    await _ratingService.DeleteRatingAsync(userId, 0, mediaId);
                }
                else
                {
                    return BadRequest("Invalid media type.");
                }

                double averageRating = mediaType == "Movie"
                    ? await _ratingService.GetAverageRatingForMovieAsync(mediaId)
                    : await _ratingService.GetAverageRatingForTVShowAsync(mediaId);

                return Json(new
                {
                    success = true,
                    message = "Rating deleted successfully.",
                    newAverageRating = averageRating
                });
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAverageRating(int mediaId, string mediaType)
        {
            try
            {
                double averageRating;

                if (mediaType == "Movie")
                {
                    averageRating = await _ratingService.GetAverageRatingForMovieAsync(mediaId);
                }
                else if (mediaType == "TVShow")
                {
                    averageRating = await _ratingService.GetAverageRatingForTVShowAsync(mediaId);
                }
                else
                {
                    return BadRequest("Invalid media type.");
                }

                return Ok(new { averageRating });
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
