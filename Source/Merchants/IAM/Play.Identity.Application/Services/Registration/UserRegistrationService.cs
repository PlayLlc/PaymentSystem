using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Core;
using Play.Identity.Api.Identity.Entities;
using Play.Identity.Api.Identity.Services._Email_Sms_Clientz;
using Play.Identity.Application.Services.Registration.Merchants;
using Play.Identity.Domain;

namespace Play.Identity.Api.Identity.Services.Registration
{
    public class UserRegistrationService : IRegisterUsers
    {
        #region Instance Values

        private readonly IUserRepository _UserRepository;
        private readonly IUnderwriteMerchants _Underwriter;
        private readonly IVerifyEmailAccount _EmailAccountVerifier;
        private readonly IVerifyMobilePhone _MobilePhoneVerifier;
        private readonly UserManager<UserIdentity> _UserManager;

        #endregion

        #region Constructor

        public UserRegistrationService(
            IUnderwriteMerchants underwriter, UserManager<UserIdentity> userManager, IUserRepository userRepository, IVerifyEmailAccount emailAccountVerifier,
            IVerifyMobilePhone mobilePhoneVerifier)
        {
            _UserManager = userManager;
            _Underwriter = underwriter;
            _UserRepository = userRepository;
            _EmailAccountVerifier = emailAccountVerifier;
            _MobilePhoneVerifier = mobilePhoneVerifier;
        }

        #endregion

        #region Instance Members

        public async Task SendEmailVerificationCode(string email)
        { }

        /// <exception cref="OperationCanceledException"></exception>
        public async Task<bool> IsUsernameUnique(string username)
        {
            return await _UserManager.Users.AnyAsync(a => a.UserName == username).ConfigureAwait(false);
        }

        public async Task<Result> ValidatePasswordPolicies(string password)
        {
            return await _UserManager.ValidatePasswordPolicies(password).ConfigureAwait(false);
        }

        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        public async Task<Result> RegisterUser(UserIdentity user, string password)
        {
            var a = await ValidatePasswordPolicies(password).ConfigureAwait(false);

            if (!a.Succeeded)
                return a;

            bool userNameValidationResult = await IsUsernameUnique(user.UserName).ConfigureAwait(false);

            if (!userNameValidationResult)
                return new Result(new List<string> {"Email is already in use"});

            if (_Underwriter.IsUserProhibited(user))
                return new Result(new List<string> {"This use is prohibited from conducted commerce in the United States"});

            var identityResult = await _UserManager.CreateAsync(user, password).ConfigureAwait(false);

            if (!identityResult.Succeeded)
                return new Result(identityResult.Errors.Select(a => a.Description));

            await _UserRepository.SaveChangesAsync().ConfigureAwait(false);

            return new Result();
        }

        #endregion
    }
}