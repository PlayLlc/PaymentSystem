using Play.Registration.Domain.Repositories;
using Play.Registration.Domain.Services;

namespace Play.Registration.Application.Services.Emails;

public class UniqueEmailChecker : IEnsureUniqueEmails
{
    #region Instance Values

    private readonly IUserRegistrationRepository _UserRegistrationRepository;

    #endregion

    #region Constructor

    public UniqueEmailChecker(IUserRegistrationRepository userRegistrationRepository)
    {
        _UserRegistrationRepository = userRegistrationRepository;
    }

    #endregion

    #region Instance Members

    public async Task<bool> IsUnique(string email) => throw new NotImplementedException();

    #endregion

    // private readonly IdentityClientApi _IdentityClientApi;
    //private readonly IUserRepository _UserRepository;
    //await _UserRegistrationRepository.IsEmailUniqueAsync(email).ConfigureAwait(false) && await _UserRepository.IsEmailUnique(email).ConfigureAwait(false);
}