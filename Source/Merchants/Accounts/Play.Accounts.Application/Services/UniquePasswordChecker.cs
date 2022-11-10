using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;

namespace Play.Accounts.Application.Services;

/// <summary>
///     When a user updates their password it must be different than their previous 4 passwords
/// </summary>
internal class UniquePasswordChecker : IEnsureUniquePasswordHistory
{
    #region Instance Values

    private readonly IPasswordRepository _PasswordRepository;

    #endregion

    #region Constructor

    public UniquePasswordChecker(IPasswordRepository passwordRepository)
    {
        _PasswordRepository = passwordRepository;
    }

    #endregion

    #region Instance Members

    public async Task<bool> AreLastFourPasswordsUnique(string userId, string hashedPassword)
    {
        IEnumerable<Password> result = (await _PasswordRepository.GetByIdAsync(userId).ConfigureAwait(false)).ToList();

        for (int i = 0; i < 4; i++)
            if (result.ElementAt(i).HashedPassword == hashedPassword)
                return false;

        return true;
    }

    #endregion
}