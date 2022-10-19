using Microsoft.AspNetCore.Mvc;

namespace Play.Identity.Api.Controllers
{
    public class Accounts : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
