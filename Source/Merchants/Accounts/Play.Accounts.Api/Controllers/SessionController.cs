using Microsoft.AspNetCore.Mvc;

namespace Play.Accounts.Api.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
