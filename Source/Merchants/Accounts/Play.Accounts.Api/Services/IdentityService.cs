using System.Security.Claims;

using IdentityModel;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Contracts.Commands;

namespace Play.Accounts.Api.Services
{
    internal class IdentityService : IIdentityService
    {
        #region Instance Values

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;

        #endregion

        #region Constructor

        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        #endregion

        #region Instance Members

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest value)
        {
            ApplicationUser applicationUser = new()
            {
                MerchantId = "",
                UserName = value.ContactInfoDto.Email,
                Email = value.ContactInfoDto.Email,
                FirstName = value.ContactInfoDto.FirstName,
                LastName = value.ContactInfoDto.LastName,
                PhoneNumber = value.ContactInfoDto.Phone,
                StreetAddress = value.AddressDto.StreetAddress,
                ApartmentNumber = value.AddressDto.ApartmentNumber,
                City = value.AddressDto.City,
                StateAbbreviation = value.AddressDto.StateAbbreviation,
                Zipcode = value.AddressDto.Zipcode,
                DateOfBirth = value.PersonalInfoDto.DateOfBirth,
                LastFourOfSocial = value.PersonalInfoDto.LastFourOfSocial,
                EmailConfirmed = false,
                IsActive = false
            };

            IdentityResult identityResult = await _UserManager.CreateAsync(applicationUser, value.Password).ConfigureAwait(false);

            if (!identityResult.Succeeded)
                return new RegisterUserResponse()
                {
                    Success = false,
                    Message = identityResult.ToString()
                };

            identityResult = await _UserManager.AddClaimsAsync(applicationUser, new List<Claim>
            {
                new(JwtClaimTypes.Name, applicationUser.FirstName),
                new(JwtClaimTypes.FamilyName, applicationUser.LastName),
                new(JwtClaimTypes.Email, applicationUser.Email)
            });

            if (!identityResult.Succeeded)
                return new RegisterUserResponse()
                {
                    Success = false,
                    Message = identityResult.ToString()
                };

            identityResult = await _UserManager.AddToRoleAsync(applicationUser, "Client");

            if (!identityResult.Succeeded)
                return new RegisterUserResponse()
                {
                    Success = false,
                    Message = identityResult.ToString()
                };

            return new RegisterUserResponse() {Success = true};
        }

        /// <exception cref="OperationCanceledException"></exception>
        public async Task<SignInResponse> SignInUserAsync(string username, string password, bool rememberLogin)
        {
            if (!await _UserManager.Users.AnyAsync(a => a.UserName == username).ConfigureAwait(false))
                return new SignInResponse()
                {
                    Success = false,
                    Message = $"Username not recognized",
                    Error = SignInError.InvalidUsername
                };

            ApplicationUser user = await _UserManager.FindByNameAsync(username);

            if (user.IsPasswordExpired())
                return new SignInResponse()
                {
                    Success = false,
                    Message = $"Password has expired",
                    Error = SignInError.ExpiredPassword
                };

            Microsoft.AspNetCore.Identity.SignInResult? result = await _SignInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return new SignInResponse()
                    {
                        Success = false,
                        Message = result.ToString(),
                        Error = SignInError.LockedOut
                    };
                if (result.IsNotAllowed)
                    return new SignInResponse()
                    {
                        Success = false,
                        Message = result.ToString(),
                        Error = SignInError.NotAuthorized
                    };
                if (result.RequiresTwoFactor)
                    return new SignInResponse()
                    {
                        Success = false,
                        Message = result.ToString(),
                        Error = SignInError.TwoFactorRequired
                    };

                return new SignInResponse()
                {
                    Success = false,
                    Message = result.ToString(),
                    Error = SignInError.InvalidPassword
                };
            }

            return new SignInResponse()
            {
                Success = true,
                User = user.AsDto()
            };
        }

        public async Task SignOutAsync()
        {
            await _SignInManager.SignOutAsync();
        }

        public async Task<bool> ValidateUsername(ValidateEmailRequest request)
        {
            if (!await _UserManager.Users.AnyAsync(a => a.UserName == request.Email).ConfigureAwait(false))
                return false;

            return true;
        }

        #endregion
    }
}