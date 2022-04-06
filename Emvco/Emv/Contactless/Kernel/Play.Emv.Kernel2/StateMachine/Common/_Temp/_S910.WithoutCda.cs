using System;

using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;

namespace Play.Emv.Kernel2.StateMachine._Temp
{
    public partial class _S910
    {
        private class WithoutCda
        {
            private readonly KernelDatabase _Database;
            private readonly ResponseHandler _ResponseHandler;
            private readonly IAuthenticateTransactionSession _AuthenticationService;

            public WithoutCda(
                KernelDatabase database, ResponseHandler responseHandler, IAuthenticateTransactionSession authenticationService)
            {
                _Database = database;
                _ResponseHandler = responseHandler;
                _AuthenticationService = authenticationService;
            }

            public StateId ProcessWithoutCda(
                IGetKernelStateId currentStateIdRetriever, Kernel2Session session, GenerateApplicationCryptogramResponse rapdu) =>
                throw new NotImplementedException();
        }
    }
}