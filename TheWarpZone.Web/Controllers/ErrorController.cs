using Microsoft.AspNetCore.Mvc;

namespace TheWarpZone.Web.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [HttpGet("{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404");
            }

            if (statusCode == 500)
            {
                return View("500");
            }

            return View("~/Views/Shared/Error.cshtml");
        }
    }
}