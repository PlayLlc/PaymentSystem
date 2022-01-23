using System.Threading.Tasks;

namespace Play.Emv.Security.Authentications;

public interface IAuthenticateOfflineData
{
    #region Instance Members

    public Task<AuthenticationResponse> Process(AuthenticationCommand command);

    #endregion
}