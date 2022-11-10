using Microsoft.AspNetCore.Mvc;

namespace Play.Accounts.Api.Controllers
{
    public class AuthorizeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
