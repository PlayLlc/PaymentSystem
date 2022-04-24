using Play.Emv.Kernel.Databases;
using Play.Emv.Security;

namespace Play.Emv.Kernel2.StateMachine;

internal partial class S910
{
    private partial class AuthHandler
    {
        #region Instance Values

        private readonly KernelDatabase _Database;
        private readonly ResponseHandler _ResponseHandler;
        private readonly IAuthenticateTransactionSession _AuthenticationService;

        #endregion

        #region Constructor

        public AuthHandler(KernelDatabase database, ResponseHandler responseHandler, IAuthenticateTransactionSession authenticationService)
        {
            _Database = database;
            _ResponseHandler = responseHandler;
            _AuthenticationService = authenticationService;
        }

        #endregion
    }
}