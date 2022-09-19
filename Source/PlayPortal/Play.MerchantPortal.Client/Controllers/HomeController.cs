using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.MerchantPortal.Client.Models;
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

            //TokenResponse response = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            //{
            //    Address = "https://localhost:7191/connect/token",

            //    ClientId = "merchantportal_client",
            //    ClientSecret = "cb17c97c-0910-41c0-aafb-2b77a5838852",
            //    RedirectUri = "https://localhost:7133/signin-oidc",
            //    Code = User.Claims.FirstOrDefault(c => c.Type == IdentityModel.JwtClaimTypes.StateHash)?.Value,
            //});

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}