using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TheWarpZone.Services.Interfaces;
using TheWarpZone.Web.ViewModels.Shared.EpisodeList;

namespace TheWarpZone.Web.Controllers
{
    public class EpisodeListController : Controller
    {
        private readonly ISeasonService _seasonService;

        public EpisodeListController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int tvShowId, string tvShowTitle)
        {
            var seasons = await _seasonService.GetSeasonsByTVShowAsync(tvShowId);
            var viewModel = new EpisodeListViewModel
            {
                TVShowId = tvShowId,
                TVShowTitle = tvShowTitle,
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
                    }).OrderBy(e => e.EpisodeNumber).ToList()
                }).OrderBy(s => s.SeasonNumber).ToList()
            };

            return View(viewModel);
        }
    }
}