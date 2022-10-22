using Microsoft.AspNetCore.Mvc;

namespace Play.Identity.Api.Areas.Registration.Controllers
{
    [Area("Accounts")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class MerchantController : Controller
    {
        #region Instance Members

        public IActionResult Index()
        {
            return Ok();
        }

        #endregion
    }
}