using Microsoft.AspNetCore.Mvc;

namespace Play.Identity.Api.Areas.Registration
{
    [Area("Accounts")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class UserRegistrationController : Controller
    {
        #region Instance Members

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}