using AutoMapper;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.AuthenticationManagement.IdentityServer.Filters;
using Play.AuthenticationManagement.IdentityServer.Models.Account;

namespace Play.AuthenticationManagement.IdentityServer.Controllers;

[SecurityHeaders]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly TestUserStore _UserStore;
    private readonly IIdentityServerInteractionService _InteractionService;
    private readonly IClientStore _ClientStore;
    private readonly IEventService _EventService;
    private readonly IMapper _Mapper;

    public AccountController(
        IEventService eventService,
        IClientStore clientStore,
        IIdentityServerInteractionService interactionService,
        TestUserStore userStore,
        IMapper mapper)
    {
        _EventService = eventService;
        _ClientStore = clientStore;
        _InteractionService = interactionService;
        _UserStore = userStore;
        _Mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        LoginViewModel viewModel = new LoginViewModel();

        viewModel.ReturnUrl = returnUrl;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            //check if we are in the context of an authorization request
            var context = await _InteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

            if (context != null)
            {
                if (_UserStore.ValidateCredentials(model.Username, model.Password))
                {
                    TestUser user = _UserStore.FindByUsername(model.Username);

                    await _EventService.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username));

                    //this section applies only for persistent sessions( remember me set to true). we don`t have this for now.
                    //AuthenticationProperties props = null;
                    //if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    //{
                    //    props = new AuthenticationProperties
                    //    {
                    //        IsPersistent = true,
                    //        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                    //    };
                    //};

                    //we issue an authentication cookie with subject ID and username

                    IdentityServerUser serverUser = _Mapper.Map<IdentityServerUser>(user);

                    serverUser.IdentityProvider = "identity";
                    serverUser.AuthenticationTime = DateTime.UtcNow;
                    serverUser.DisplayName = user.Username;
                    serverUser.AdditionalClaims = user.Claims;

                    await HttpContext.SignInAsync(serverUser);

                    var client = await _ClientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

                    if (client.RequirePkce)
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null (non-pkce clients, usually hybrid/implicit flow)
                    return Redirect(model.ReturnUrl);
                }
            }
        }

        await _EventService.RaiseAsync(new UserLoginFailureEvent(model.Username, "Invalid credentials"));
        return View(model);
    }
}
