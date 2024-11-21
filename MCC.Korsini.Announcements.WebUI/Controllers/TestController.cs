using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MCC.Korsini.Announcements.WebUI.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace MCC.Korsini.Announcements.WebUI.Controllers
{
    public class TestController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                var errorDetails = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = exceptionFeature.Error.Message,
                    Path = exceptionFeature.Path
                };

                return View(errorDetails);
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
