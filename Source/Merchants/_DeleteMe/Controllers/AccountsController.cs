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
    [Authorize(ApiScopes.IdentityServer.Name)]
    public class AccountsController : Controller
    {
        #region Instance Members

        public async Task<IActionResult> Test()
        {
            return View();
        }

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

        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel viewModel = new LoginViewModel();

            viewModel.ReturnUrl = returnUrl;

            return View(viewModel);
        }

        #endregion
    }
}