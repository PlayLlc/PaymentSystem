using AutoMapper;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.AuthenticationManagement.Identity.Services;
using Play.AuthenticationManagement.IdentityServer.Filters;
using Play.AuthenticationManagement.IdentityServer.Models.Account;

namespace Play.AuthenticationManagement.IdentityServer.Controllers;

[SecurityHeaders]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IIdentityService _IdentityService;
    private readonly IIdentityServerInteractionService _InteractionService;
    private readonly IClientStore _ClientStore;
    private readonly IEventService _EventService;
    private readonly IMapper _Mapper;

    public AccountController(
        IEventService eventService,
        IClientStore clientStore,
        IIdentityServerInteractionService interactionService,
        IIdentityService identityService,
        IMapper mapper)
    {
        _EventService = eventService;
        _ClientStore = clientStore;
        _InteractionService = interactionService;
        _IdentityService = identityService;
        _Mapper = mapper;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
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
                var signInResult = await _IdentityService.SignInUserAsync(model.Username, model.Password, model.RememberLogin);

                if (signInResult.ChangePassword)
                    RedirectToAction("ChangePassword", new { UserId = signInResult.User.Id });

                if (signInResult.Succeeded)
                {
                    var user = signInResult.User;

                    await _EventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

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

            await _EventService.RaiseAsync(new UserLoginFailureEvent(model.Username, "Invalid credentials", clientId: context?.Client.ClientId));
            ModelState.AddModelError(string.Empty, "Invalid credentials");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
            return BadRequest("Invalid authorization context. We don`t know where you are coming from");

        RegisterViewModel vm = new RegisterViewModel()
        {
            ReturnUrl = returnUrl
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (ModelState.IsValid)
        {
            CreateUserInput createUser = _Mapper.Map<CreateUserInput>(vm);

            var result = await _IdentityService.RegisterUserAsync(createUser);

            if (result.Succeeded)
                return RedirectToAction("Login", "Account", new { returnUrl = vm.ReturnUrl });

            for(int i = 0; i < result.Errors.Length; i++)
            {
                ModelState.AddModelError(i.ToString(), result.Errors[i]);
            }
        }

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        if (User?.Identity?.IsAuthenticated != true)
        {
            //if the user is not authenticated then just show the logout page as we do not have a context for this request.
            return View();
        }

        var context = await _InteractionService.GetLogoutContextAsync(logoutId);

        if (context?.ShowSignoutPrompt == false)
        {
            //the request for logout was properly authenticated from Identity server
            // we don't need to show the prompt and can just log the user out directly.
            return await Logout(context, logoutId);
        }

        return View();
    }

    private async Task<IActionResult> Logout(LogoutRequest logout, string logoutId)
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            // delete local authentication cookie
            await _IdentityService.SignOutAsync();

            // raise the logout event
            await _EventService.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        LoggedOutViewModel vm = new LoggedOutViewModel
        {
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            LogoutId = logoutId
        };

        return View("LoggedOut", vm);
    }
}
