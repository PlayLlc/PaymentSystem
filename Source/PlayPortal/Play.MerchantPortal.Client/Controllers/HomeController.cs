using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.MerchantPortal.Client.Models.Home;

namespace Play.MerchantPortal.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var httpClient = httpClientFactory.CreateClient("MerchantPortalClient");
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            IndexViewModel vm = new IndexViewModel
            {
                GivenName = User.Claims.SingleOrDefault(c => c.Type == "given_name")!.Value,
                AccessToken = accessToken!
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult Logout()
        {
            return SignOut("cookie", "oidc");
        }
    }
}