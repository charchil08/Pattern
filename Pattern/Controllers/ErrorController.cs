using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Pattern.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404 || statusCode.Value == 500)
                {
                    var viewName = statusCode.ToString();
                    ViewBag.StatusCode = viewName;
                    return View();
                }
            }
            return View();
        }

        //[Route("/Error/405")]
        public IActionResult MethodNotAllowed()
        {

            return RedirectToAction("Index", "Skill");
        }

        public IActionResult Error404()
        {
            return View();
        }


        public IActionResult Error405()
        {
            return RedirectToAction("Index", "Skill");
        }


        public IActionResult GlobalException()
        {

            if (Response.StatusCode == 404)
            {
                return RedirectToAction("Error404", "Error");
            }

            IExceptionHandlerFeature exception = HttpContext.Features.Get<IExceptionHandlerFeature>()!;


            TempData["error"] = $"{exception.Error.InnerException?.Message.Split(".")[0] ?? exception.Error.Message.Split(".")[0]}";

            if (exception.Error.GetType().Name == "DbUpdateException")
            {
                return Redirect(HttpContext.Request.Headers.Referer);
            }

            return RedirectToAction("Index", "Home");

        }
    }
}
