using Microsoft.AspNetCore.Mvc;

namespace Play.Accounts.Api.Areas.Registration
{
    [Area($"{nameof(Registration)}")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class MerchantsController : Controller
    {
        #region Instance Members

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}