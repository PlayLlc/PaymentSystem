﻿using System.Net;

using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Play.Identity.Api.__Services;
using Play.Identity.Api.Attributes;
using Play.Identity.Api.Identity.Entities;
using Play.Identity.Api.Models;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Play.Identity.Api.Controllers;
// TODO: Add logging

[SecurityHeaders]
[AllowAnonymous]
[ApiController]
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    #region Instance Values

    private readonly IBuildLoginViewModel _LoginViewModelBuilder;
    private readonly IIdentityServerInteractionService _InteractionService;
    private readonly SignInManager<UserIdentity> _SignInManager;
    private readonly ILogger<AccountController> _Logger;
    private readonly IVerifyEmailAccount _EmailAccountVerifier;

    #endregion

    #region Constructor

    public AccountController(
        IBuildLoginViewModel loginViewModelBuilder, IIdentityServerInteractionService interactionService, SignInManager<UserIdentity> signInManager,
        ILogger<AccountController> logger)
    {
        _LoginViewModelBuilder = loginViewModelBuilder;
        _InteractionService = interactionService;
        _SignInManager = signInManager;
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    public async Task<IActionResult> VerifyEmail(int verificationCode)
    { }

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

    /// <exception cref="Exception"></exception>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginInputModel model, [FromForm] string button)
    {
        AuthorizationRequest? context = await _InteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

        // if we don't have a valid context then we will reload the login page
        if (context is null)
            return await Login(model.ReturnUrl);

        if (button != "login")
        {
            await _InteractionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            return Redirect(model.ReturnUrl);
        }

        // Something went wrong. Show form with error
        if (!ModelState.IsValid)
            return View(await _LoginViewModelBuilder.BuildLoginViewModelAsync(model).ConfigureAwait(false));

        UserIdentity? user = await _SignInManager.UserManager.FindByNameAsync(model.Username);

        if (!await IsUsernameAndPasswordValid(user, model.Password).ConfigureAwait(false))
        {
            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);

            return View(await _LoginViewModelBuilder.BuildLoginViewModelAsync(model).ConfigureAwait(false));
        }

        AuthenticationProperties? props = AccountOptions.AllowRememberLogin && model.RememberLogin
            ? new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
            }
            : null;

        IdentityServerUser issuer = new IdentityServerUser(user!.Id) {DisplayName = user!.UserName};

        await HttpContext.SignInAsync(issuer, props);

        return Redirect(model.ReturnUrl);
    }

    private async Task<bool> IsUsernameAndPasswordValid(UserIdentity? user, string password)
    {
        if (user is null)
            return false;

        SignInResult? result = await _SignInManager.PasswordSignInAsync(user.UserName, password, false, false /* HACK CHANGE THIS */);

        if (result.Succeeded != SignInResult.Success.Succeeded)
            return false;

        return true;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<bool> EmailVerification([FromQuery] string email, [FromQuery] int code)
    {
        bool result = await _EmailAccountVerifier.VerifyConfirmationCode(email, code).ConfigureAwait(false);

        if (result)
            return true;

        Response.StatusCode = (int) HttpStatusCode.BadRequest;

        // Hack -> But yeah ..here
        new ExternalController(null, null, null).Callback();

        return false;
    }

    #endregion
}