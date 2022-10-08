using Microsoft.AspNetCore.Mvc;

namespace Play.Accounts.Api.Controllers
{
    public class DeviceAuthorizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
