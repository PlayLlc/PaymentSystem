using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Repositories;
using Play.Domain.Exceptions;
using Play.Identity.Api.Models;
using Play.Identity.Api.Services;
using Play.Mvc.Attributes;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Play.Identity.Api.Controllers;
// TODO: Add logging

//[ApiController]
[SecurityHeaders]
[AllowAnonymous]
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    #region Instance Values

    private readonly IBuildLoginViewModel _LoginViewModelBuilder;
    private readonly IIdentityServerInteractionService _InteractionService;
    private readonly UserManager<UserIdentity> _UserManager;
    private readonly IUserRepository _UserRepository;
    private readonly IHashPasswords _PasswordHasher;
    private readonly ILoginUsers _UserLoginService;
    private readonly ILogger<AccountController> _Logger;

    #endregion

    #region Constructor

    public AccountController(
        IBuildLoginViewModel loginViewModelBuilder, IIdentityServerInteractionService interactionService, UserManager<UserIdentity> userManager,
        IUserRepository userRepository, IHashPasswords passwordHasher, ILoginUsers userLoginService, ILogger<AccountController> logger)
    {
        _LoginViewModelBuilder = loginViewModelBuilder;
        _InteractionService = interactionService;
        _UserManager = userManager;
        _UserRepository = userRepository;
        _PasswordHasher = passwordHasher;
        _UserLoginService = userLoginService;
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        User user = await _UserRepository.GetByEmailAsync("test@aol.com").ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        var test = await _UserManager.FindByEmailAsync("test@aol.com").ConfigureAwait(false);

        LoginViewModel vm = await _LoginViewModelBuilder.BuildLoginViewModelAsync(returnUrl).ConfigureAwait(false);

        return View(vm);
    }

    /// <exception cref="Exception"></exception>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        AuthorizationRequest? context = await _InteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

        // if we don't have a valid context then we will reload the login page
        if (context is null)
            return await Login(model.ReturnUrl);

        if (!ModelState.IsValid)
            return View(await _LoginViewModelBuilder.BuildLoginViewModelAsync(model).ConfigureAwait(false));

        User user = await _UserRepository.GetByEmailAsync(model.Username).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        var loginResult = await _UserLoginService.LoginAsync(HttpContext, user, model.Password).ConfigureAwait(false);

        if (!loginResult.Succeeded)
        {
            foreach (var error in loginResult.Errors)
                ModelState.AddModelError(string.Empty, error);

            return View(await _LoginViewModelBuilder.BuildLoginViewModelAsync(model).ConfigureAwait(false));
        }

        return Redirect(model.ReturnUrl);
    }

    #endregion

    // TODO: Logout 
}