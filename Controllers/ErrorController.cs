using Microsoft.AspNetCore.Mvc;

namespace Project_CNPM.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Forbidden()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View("403");
        }

        public IActionResult NotFoundPage()
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View("404");
        }

        public IActionResult ServerError()
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("500");
        }
    }
}
