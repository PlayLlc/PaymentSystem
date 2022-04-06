using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
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
                IGetKernelStateId currentStateIdRetriever, Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
            {
                if (TryHandlingForMissingMandatoryData(session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                throw new NotImplementedException();
            }

            #region S910.30 - S910.31

            /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
            private bool TryHandlingForMissingMandatoryData(KernelSessionId sessionId)
            {
                if (_Database.IsPresentAndNotEmpty(ApplicationCryptogram.Tag))
                    return false;

                _Database.Update(Level2Error.CardDataMissing);
                _ResponseHandler.ProcessInvalidDataResponse(sessionId);

                return true;
            }

            #endregion
        }
    }
}