using Play.Identity.Domain.Repositories;
using Play.Identity.Domain.Services;

namespace Play.Identity.Application.Services;

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

    public async Task<bool> IsUnique(string email) =>
        await _UserRegistrationRepository.IsEmailUniqueAsync(email).ConfigureAwait(false) && await _UserRepository.IsEmailUnique(email).ConfigureAwait(false);

    #endregion
}