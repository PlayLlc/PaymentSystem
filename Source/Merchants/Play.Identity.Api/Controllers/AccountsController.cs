using System.Security.Claims;

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Play.Identity.Api.Models;
using Play.Identity.Api.Services;

using static IdentityModel.OidcConstants;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Play.Identity.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountsController : Controller
    {
        #region Instance Values

        private readonly IBuildLoginViewModel _LoginViewModelBuilder;
        private readonly IIdentityServerInteractionService _InteractionService;
        private readonly IAuthenticationSchemeProvider _SchemeProvider;
        private readonly IIdentityProviderStore _IdentityProviderStore;
        private readonly IClientStore _ClientStore;

        private readonly SignInManager<IdentityUser> _SignInManager;

        #endregion

        #region Constructor

        public AccountsController(IBuildLoginViewModel loginViewModelBuilder)
        {
            _LoginViewModelBuilder = loginViewModelBuilder;
        }

        #endregion

        #region Instance Members

        [HttpGet]
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

        /// <summary>
        ///     Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginInputModel model, [FromForm] string button)
        {
            // check if we are in the context of an authorization request
            AuthorizationRequest context = await _InteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            //if (button != "login")
            //{
            //    if (context != null)
            //    {
            //        // if the user cancels, send a result back into IdentityServer as if they 
            //        // denied the consent (even if this client does not require consent).
            //        // this will send back an access denied OIDC error response to the client.
            //        await _InteractionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            //        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            //        if (context.IsNativeClient())

            //            // The client is native, so this change in how to
            //            // return the response is for better UX for the end user.
            //            return this.LoadingPage("Redirect", model.ReturnUrl);

            //        return Redirect(model.ReturnUrl);
            //    }
            //    else
            //    {
            //        // since we don't have a valid context, then we just go back to the home page
            //        return Redirect("~/");
            //    }
            //}

            if (ModelState.IsValid)
            {
                // find user by username
                IdentityUser user = await _SignInManager.UserManager.FindByNameAsync(model.Username);

                // validate username/password using ASP.NET Identity
                if ((user != null) && (await _SignInManager.CheckPasswordSignInAsync(user, model.Password, true) == SignInResult.Success))
                {
                    // TODO: Log user login successful
                    //await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    // only set explicit expiration here if user chooses "remember me". 
                    // otherwise we rely upon expiration configured in cookie middleware.
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    ;

                    // issue authentication cookie with subject ID and username
                    IdentityServerUser issuer = new IdentityServerUser(user.Id) {DisplayName = user.UserName};

                    await HttpContext.SignInAsync(issuer, props);

                    if (context != null)
                    {
                        if (context.IsNativeClient())

                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect("~/");
                    else

                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                }

                // TODO: Log user login failure & throw event if needed
                //await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            LoginViewModel vm = await BuildLoginViewModelAsync(model);

            return View(vm);
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            LoginViewModel vm = await _LoginViewModelBuilder.BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;

            return vm;
        }

        #endregion

        //[HttpPost(nameof(Login))]
        //private async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    Claim[] claims = new Claim[] {new("sub", "testuser@hotmail.com")};
        //    ClaimsIdentity identity = new ClaimsIdentity(claims, "pwd");
        //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        //    IdentityServerUser user = new("testuser@hotmail.com");
        //    await HttpContext.SignInAsync(user).ConfigureAwait(false);

        //    return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
        //}
    }
}