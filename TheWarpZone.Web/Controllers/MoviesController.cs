using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheWarpZone.Services.Interfaces;

namespace TheWarpZone.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieDetailsAsync(id);
                return View(movie);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _movieService.AddMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieDetailsAsync(id);
                return View(movie);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _movieService.UpdateMovieAsync(movie);
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }

            return View(movie);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieDetailsAsync(id);
                return View(movie);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
