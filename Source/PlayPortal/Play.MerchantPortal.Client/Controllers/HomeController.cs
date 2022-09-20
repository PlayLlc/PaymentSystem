using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.MerchantPortal.Client.Models;
using Play.MerchantPortal.Client.Models.Home;
using System.Diagnostics;

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
                AccessToken = accessToken
            };

            //TokenResponse response = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            //{
            //    Address = "https://localhost:7191/connect/token",

            //    ClientId = "merchantportal_client",
            //    ClientSecret = "cb17c97c-0910-41c0-aafb-2b77a5838852",
            //    RedirectUri = "https://localhost:7133/signin-oidc",
            //    Code = User.Claims.FirstOrDefault(c => c.Type == IdentityModel.JwtClaimTypes.StateHash)?.Value,
            //});

            return View(vm);
        }

        [Authorize]
        public IActionResult Logout()
        {
            return SignOut("cookie", "oidc");
        }
    }
}