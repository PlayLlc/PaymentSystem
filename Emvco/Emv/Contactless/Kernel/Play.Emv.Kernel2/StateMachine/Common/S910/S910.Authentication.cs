using Play.Emv.Kernel.Databases;
using Play.Emv.Security;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private partial class AuthenticationHandler
        {
            private readonly KernelDatabase _Database;
            private readonly ResponseHandler _ResponseHandler;
            private readonly IAuthenticateTransactionSession _AuthenticationService;

            public AuthenticationHandler(
                KernelDatabase database, ResponseHandler responseHandler, IAuthenticateTransactionSession authenticationService)
            {
                _Database = database;
                _ResponseHandler = responseHandler;
                _AuthenticationService = authenticationService;
            }
        }
    }
}