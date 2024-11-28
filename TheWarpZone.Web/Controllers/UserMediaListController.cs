using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.ViewModels.Shared.MediaList;

namespace TheWarpZone.Web.Controllers
{
    [Authorize]
    public class UserMediaListController : Controller
    {
        private readonly IUserMediaListService _userMediaListService;

        public UserMediaListController(IUserMediaListService userMediaListService)
        {
            _userMediaListService = userMediaListService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var mediaListDtos = await _userMediaListService.GetUserMediaListAsync(userId);

            var viewModel = new UserMediaListViewModel
            {
                MediaItems = mediaListDtos.Select(item => new MediaListItemViewModel
                {
                    Id = item.Id,
                    Title = item.MediaTitle,
                    Status = item.Status,
                    MovieId = item.MovieId,
                    TVShowId = item.TVShowId
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToList(MediaListFormViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var newMediaListDto = new UserMediaListDto
                {
                    MediaTitle = model.Title,
                    MovieId = model.MovieId,
                    TVShowId = model.TVShowId,
                    Status = model.Status,
                    UserId = userId
                };

                await _userMediaListService.AddToUserMediaListAsync(newMediaListDto);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (model.MovieId.HasValue)
                {
                    return RedirectToAction("Index", "Movie");
                }
                else if (model.TVShowId.HasValue)
                {
                    return RedirectToAction("Index", "TVShow");
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Movie");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromList(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var item = await _userMediaListService.GetUserMediaListAsync(userId);
            if (item.All(x => x.Id != id))
            {
                return Forbid();
            }

            await _userMediaListService.RemoveFromUserMediaListAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus)
        {
            if (!Enum.TryParse(newStatus, out MediaStatus status))
            {
                return BadRequest("Invalid status.");
            }

            await _userMediaListService.UpdateMediaListStatusAsync(id, status);
            return RedirectToAction(nameof(Index));
        }
    }
}
