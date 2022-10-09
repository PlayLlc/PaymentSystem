using System.Security.Claims;

using _DeleteMe.Configuration;
using _DeleteMe.Models;

using Duende.IdentityServer;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _DeleteMe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        #region Instance Members

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return View();
        }

        [HttpPost("{action}")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Claim[] claims = new Claim[] {new("sub", "testuser@hotmail.com")};
            ClaimsIdentity identity = new ClaimsIdentity(claims, "pwd");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            IdentityServerUser user = new("testuser@hotmail.com");
            await HttpContext.SignInAsync(user).ConfigureAwait(false);

            return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
        }

        [HttpGet(nameof(Login))]
        public async Task<IActionResult> Login()
        {
            LoginViewModel viewModel = new LoginViewModel();

            viewModel.ReturnUrl = "";

            return View(viewModel);
        }

        #endregion
    }
}