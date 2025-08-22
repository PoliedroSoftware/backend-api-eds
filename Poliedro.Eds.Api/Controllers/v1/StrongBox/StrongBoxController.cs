using Microsoft.AspNetCore.Mvc;

namespace Poliedro.Eds.Api.Controllers.v1.StrongBox
{
    public class StrongBox : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
