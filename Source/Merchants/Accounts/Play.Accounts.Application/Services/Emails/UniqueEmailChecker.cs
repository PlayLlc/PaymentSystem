using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;

namespace Play.Accounts.Application.Services;

public class UniqueEmailChecker : IEnsureUniqueEmails
{
    #region Instance Values

    private readonly IUserRegistrationRepository _UserRegistrationRepository;
    private readonly IUserRepository _UserRepository;

    #endregion

    #region Constructor

    public UniqueEmailChecker(IUserRegistrationRepository userRegistrationRepository, IUserRepository userRepository)
    {
        _UserRegistrationRepository = userRegistrationRepository;
        _UserRepository = userRepository;
    }

    #endregion

    #region Instance Members

    public async Task<bool> IsUnique(string email)
    {
        return await _UserRegistrationRepository.IsEmailUniqueAsync(email).ConfigureAwait(false)
               && await _UserRepository.IsEmailUnique(email).ConfigureAwait(false);
    }

    #endregion
}