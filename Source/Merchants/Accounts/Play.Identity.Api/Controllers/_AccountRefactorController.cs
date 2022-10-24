//using Duende.IdentityServer;
//using Duende.IdentityServer.Models;
//using Duende.IdentityServer.Services;

//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//using Play.Accounts.Domain.Aggregates;
//using Play.Accounts.Domain.Repositories;
//using Play.Accounts.Domain.Services;
//using Play.Accounts.Persistence.Sql.Entities;
//using Play.Identity.Api.Attributes;
//using Play.Identity.Api.Extensions;
//using Play.Identity.Api.Models;

//using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

//namespace Play.Identity.Api.Controllers;
//// TODO: Add logging

//[SecurityHeaders]
//[AllowAnonymous]
//[ApiController]
//[Route("[controller]/[action]")]
//public class _AccountRefactorController : Controller
//{
//    #region Instance Values

//    private readonly IUserRepository _UserRepository;
//    private readonly IHashPasswords _PasswordHasher;

//    private readonly IBuildLoginViewModel _LoginViewModelBuilder;
//    private readonly IIdentityServerInteractionService _InteractionService;
//    private readonly SignInManager<UserIdentity> _SignInManager;
//    private readonly ILogger<AccountController> _Logger;

//    #endregion

//    #region Constructor

//    public _AccountRefactorController(
//        IBuildLoginViewModel loginViewModelBuilder, IIdentityServerInteractionService interactionService, SignInManager<UserIdentity> signInManager,
//        ILogger<AccountController> logger, IUserRepository userRepository, IHashPasswords passwordHasher)
//    {
//        _LoginViewModelBuilder = loginViewModelBuilder;
//        _InteractionService = interactionService;
//        _SignInManager = signInManager;
//        _Logger = logger;
//        _UserRepository = userRepository;
//        _PasswordHasher = passwordHasher;
//    }

//    #endregion

//    #region Instance Members

//    [HttpGet]
//    public async Task<IActionResult> Login(string returnUrl)
//    {
//        // build a model so we know what to show on the login page
//        LoginViewModel vm = await _LoginViewModelBuilder.BuildLoginViewModelAsync(returnUrl).ConfigureAwait(false);

//        // we only have one option for logging in and it's an external provider
//        if (vm.IsExternalLoginOnly)
//            return RedirectToAction("Challenge", "External", new
//            {
//                scheme = vm.ExternalLoginScheme,
//                returnUrl
//            });

//        return View(vm);
//    }

//    /// <exception cref="Exception"></exception>
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Login([FromForm] LoginInputModel model)
//    {
//        // TODO: LoginFailed should really just be redirecting to Login with the model state errors

//        if (!ModelState.IsValid)
//            return View("LoginFailed");

//        AuthorizationRequest? context = await _InteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

//        if (context is null)
//        {
//            await _InteractionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

//            return View("Error");
//        }

//        User? user = await _UserRepository.GetByEmailAsync(model.Username).ConfigureAwait(false);

//        if (user is null)
//            return View("LoginFailed");

//        if (user.IsPasswordValid(_PasswordHasher, model.Password))
//            return View("LoginFailed");

//        AuthenticationProperties? props = AccountOptions.AllowRememberLogin && model.RememberLogin
//            ? new AuthenticationProperties
//            {
//                IsPersistent = true,
//                ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
//            }
//            : null;

//        IdentityServerUser issuer = new IdentityServerUser(user!.GetId()) {DisplayName = user!.GetEmail()};
//        UserIdentity a = null;
//        await HttpContext.SignInAsync(issuer, props);

//        return Redirect(model.ReturnUrl);
//    }

//    #endregion
//}

