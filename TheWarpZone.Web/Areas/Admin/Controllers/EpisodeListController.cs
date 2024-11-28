using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheWarpZone.Common.DTOs;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.Areas.Admin.ViewModels.EpisodeList;
using TheWarpZone.Web.ViewModels.Shared.EpisodeList;

namespace TheWarpZone.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class EpisodeListController : Controller
    {
        private readonly ISeasonService _seasonService;
        private readonly IEpisodeService _episodeService;

        public EpisodeListController(ISeasonService seasonService, IEpisodeService episodeService)
        {
            _seasonService = seasonService;
            _episodeService = episodeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int tvShowId, int? expandedSeasonId = null)
        {
            var seasons = await _seasonService.GetSeasonsByTVShowAsync(tvShowId);

            var viewModel = new EpisodeListViewModel
            {
                TVShowId = tvShowId,
                Seasons = seasons.Select(s => new SeasonViewModel
                {
                    Id = s.Id,
                    SeasonNumber = s.SeasonNumber,
                    Title = s.Title,
                    Episodes = s.Episodes.Select(e => new EpisodeViewModel
                    {
                        Id = e.Id,
                        EpisodeNumber = e.EpisodeNumber,
                        Title = e.Title,
                        Description = e.Description
                    }).ToList()
                }).ToList(),
                ExpandedSeasonId = expandedSeasonId
            };

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> AddSeason(int tvShowId)
        {
            var seasons = await _seasonService.GetSeasonsByTVShowAsync(tvShowId);
            var nextSeasonNumber = seasons.Any() ? seasons.Max(s => s.SeasonNumber) + 1 : 1;

            var model = new SeasonFormViewModel
            {
                TVShowId = tvShowId,
                SeasonNumber = nextSeasonNumber
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSeason(SeasonFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _seasonService.IsSeasonNumberUniqueAsync(model.TVShowId, model.SeasonNumber))
            {
                ModelState.AddModelError(nameof(model.SeasonNumber), $"Season number {model.SeasonNumber} already exists for this TV show.");
                return View(model);
            }

            var seasonDto = new SeasonDto
            {
                SeasonNumber = model.SeasonNumber,
                Title = model.Title,
                TVShowId = model.TVShowId
            };

            await _seasonService.AddSeasonAsync(seasonDto);
            return RedirectToAction(nameof(Index), new { tvShowId = model.TVShowId });
        }


        [HttpGet]
        public async Task<IActionResult> EditSeason(int id)
        {
            var season = await _seasonService.GetSeasonByIdAsync(id);

            var viewModel = new SeasonFormViewModel
            {
                Id = season.Id,
                SeasonNumber = season.SeasonNumber,
                Title = season.Title,
                TVShowId = season.TVShowId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSeason(SeasonFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _seasonService.IsSeasonNumberUniqueAsync(model.TVShowId, model.SeasonNumber, model.Id))
            {
                ModelState.AddModelError(string.Empty, $"Season number {model.SeasonNumber} already exists for the TV show.");
                return View(model);
            }

            var seasonDto = new SeasonDto
            {
                Id = model.Id,
                SeasonNumber = model.SeasonNumber,
                Title = model.Title,
                TVShowId = model.TVShowId
            };

            await _seasonService.UpdateSeasonAsync(seasonDto);
            return RedirectToAction(nameof(Index), new { tvShowId = model.TVShowId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSeason(int id, int tvShowId)
        {
            try
            {
                await _seasonService.DeleteSeasonAsync(id);
                return RedirectToAction(nameof(Index), new { tvShowId });
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index), new { tvShowId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying to delete the season.";
                return RedirectToAction(nameof(Index), new { tvShowId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddEpisode(int seasonId, int tvShowId)
        {
            var episodes = await _episodeService.GetEpisodesBySeasonAsync(seasonId);
            var nextEpisodeNumber = episodes.Any() ? episodes.Max(e => e.EpisodeNumber) + 1 : 1;

            var model = new EpisodeFormViewModel
            {
                SeasonId = seasonId,
                TVShowId = tvShowId,
                EpisodeNumber = nextEpisodeNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEpisode(EpisodeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _episodeService.IsEpisodeNumberUniqueAsync(model.SeasonId, model.EpisodeNumber))
            {
                ModelState.AddModelError(nameof(model.EpisodeNumber), $"Episode number {model.EpisodeNumber} already exists in the season.");
                return View(model);
            }

            var episodeDto = new EpisodeDto
            {
                EpisodeNumber = model.EpisodeNumber,
                Title = model.Title,
                Description = model.Description,
                SeasonId = model.SeasonId
            };

            await _episodeService.AddEpisodeAsync(episodeDto);
            return RedirectToAction(nameof(Index), new { tvShowId = model.TVShowId, expandedSeasonId = model.SeasonId });
        }


        [HttpGet]
        public async Task<IActionResult> EditEpisode(int id)
        {
            var episode = await _episodeService.GetEpisodeDetailsAsync(id);
            if (episode == null)
            {
                return NotFound();
            }

            var season = await _seasonService.GetSeasonByIdAsync(episode.SeasonId);
            if (season == null)
            {
                return NotFound();
            }

            var viewModel = new EpisodeFormViewModel
            {
                Id = episode.Id,
                EpisodeNumber = episode.EpisodeNumber,
                Title = episode.Title,
                Description = episode.Description,
                SeasonId = episode.SeasonId,
                TVShowId = season.TVShowId
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEpisode(EpisodeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _episodeService.IsEpisodeNumberUniqueAsync(model.SeasonId, model.EpisodeNumber, model.Id))
            {
                ModelState.AddModelError(nameof(model.EpisodeNumber), $"Episode number {model.EpisodeNumber} already exists in the season.");
                return View(model);
            }

            var episodeDto = new EpisodeDto
            {
                Id = model.Id,
                EpisodeNumber = model.EpisodeNumber,
                Title = model.Title,
                Description = model.Description,
                SeasonId = model.SeasonId
            };

            await _episodeService.UpdateEpisodeAsync(episodeDto);

            return RedirectToAction(nameof(Index), new { tvShowId = model.TVShowId, expandedSeasonId = model.SeasonId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEpisode(int id, int tvShowId, int seasonId)
        {
            await _episodeService.DeleteEpisodeAsync(id);
            return RedirectToAction(nameof(Index), new { tvShowId, expandedSeasonId = seasonId });
        }
    }
}
