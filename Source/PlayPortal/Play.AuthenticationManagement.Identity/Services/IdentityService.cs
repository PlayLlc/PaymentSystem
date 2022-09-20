using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Play.AuthenticationManagement.Identity.Models;
using System.Security.Claims;

namespace Play.AuthenticationManagement.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        public async Task<RegisterResult> RegisterUserAsync(CreateUserInput input)
        {
            User user = new()
            {
                UserName = input.UseEmailAsUserName ? input.Email : input.UserName,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                LastPasswordChangedDate = DateTime.UtcNow,
            };

            IdentityResult identityResult = await _UserManager.CreateAsync(user, input.Password);

            if (identityResult.Errors.Any())
                return identityResult.ToResult();

            identityResult = await _UserManager.AddClaimsAsync(user, new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, input.FirstName),
                new Claim(JwtClaimTypes.FamilyName, input.LastName),
                new Claim(JwtClaimTypes.Email, input.Email)
            });

            if (identityResult.Errors.Any())
                return identityResult.ToResult();

            IdentityResult updateRoleResult = await _UserManager.AddToRoleAsync(user, "Client");

            return updateRoleResult.ToResult();
        }

        public async Task<SignInResult> SignInUserAsync(string username, string password, bool rememberLogin)
        {
            var result = await _SignInManager.PasswordSignInAsync(username, password, isPersistent: rememberLogin, lockoutOnFailure: true);

            if (!result.Succeeded)
                return SignInResult.FailedSignIn();

            var user = await _UserManager.FindByNameAsync(username);

            if (!IsPasswordValid(user))
                return SignInResult.ChangePasswordRequired(user);

            return SignInResult.Success(user);
        }

        private static bool IsPasswordValid(User user)
        {
            const int passwordExpirationTimeStampInDays = 90;

            return user.LastPasswordChangedDate.AddDays(passwordExpirationTimeStampInDays) < DateTime.UtcNow;
        }

        public async Task SignOutAsync()
        {
            await _SignInManager.SignOutAsync();
        }
    }
}
