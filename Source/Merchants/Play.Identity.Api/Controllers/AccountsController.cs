using System.Security.Claims;

using Duende.IdentityServer;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Identity.Api.Models;
using Play.Identity.Api.Services;

namespace Play.Identity.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        #region Instance Values

        private readonly IBuildLoginViewModel _LoginViewModelBuilder;

        #endregion

        #region Constructor

        public AccountsController(IBuildLoginViewModel loginViewModelBuilder)
        {
            _LoginViewModelBuilder = loginViewModelBuilder;
        }

        #endregion

        #region Instance Members

        [HttpGet(nameof(Login))]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            LoginViewModel vm = await _LoginViewModelBuilder.BuildLoginViewModelAsync(returnUrl).ConfigureAwait(false);

            if (vm.IsExternalLoginOnly)

                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new
                {
                    scheme = vm.ExternalLoginScheme,
                    returnUrl
                });

            return View(vm);
        }

        [HttpPost(nameof(Login))]
        private async Task<IActionResult> Login(LoginViewModel model)
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

        #endregion
    }
}