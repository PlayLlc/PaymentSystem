using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private partial class AuthenticationHandler
        {
            /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
            public StateId ProcessWithoutCda(
                IGetKernelStateId currentStateIdRetriever, Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
            {
                if (TryHandlingForMissingMandatoryData(session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                if (!IsApplicationAuthenticationCryptogram())
                    return HandleIsNotAac(currentStateIdRetriever, session);
                else
                    return HandleAac(currentStateIdRetriever, session);
            }

            /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
            /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
            /// <exception cref="Play.Icc.Exceptions.IccProtocolException"></exception>
            private StateId HandleAac(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
            {
                if (IsIdsReadFlagSet())
                {
                    HandleInvalidResponse(session.GetKernelSessionId());

                    return currentStateIdRetriever.GetStateId();
                }

                if (!IsApplicationAuthenticationCryptogram())
                    return _ResponseHandler.HandleValidResponse(currentStateIdRetriever, session);

                if (!IsCdaRequested())
                    return _ResponseHandler.HandleValidResponse(currentStateIdRetriever, session);

                HandleInvalidResponse(session.GetKernelSessionId());

                return currentStateIdRetriever.GetStateId();
            }

            private StateId HandleIsNotAac(IGetKernelStateId currentGetKernelStateId, Kernel2Session session)
            {
                if (TryHandleIsNotAacDataError(session.GetKernelSessionId()))
                    return currentGetKernelStateId.GetStateId();

                HandleRelayResistanceData();

                return _ResponseHandler.HandleValidResponse(currentGetKernelStateId, session);
            }

            private bool TryHandleIsNotAacDataError(KernelSessionId sessionId)
            {
                if (!IsCdaRequested())
                    return false;

                HandleInvalidResponse(sessionId);

                return true;
            }

            private bool HandleRelayResistanceData()
            { }

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

            #region S910.32

            private bool IsApplicationAuthenticationCryptogram()
            {
                var cid = _Database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

                return cid.GetCryptogramType() == CryptogramTypes.ApplicationAuthenticationCryptogram;
            }

            #endregion

            #region S910.33

            /// <exception cref="TerminalDataException"></exception>
            private bool IsIdsReadFlagSet()
            {
                if (!_Database.IsIdsAndTtrImplemented())
                    return false;

                if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out IntegratedDataStorageStatus? integratedDataStorageStatus))
                    return false;

                if (!integratedDataStorageStatus!.IsReadSet())
                    return false;

                return true;
            }

            #endregion

            #region S910.34, S910.36

            private bool IsCdaRequested()
            {
                if (!_Database.TryGet(ReferenceControlParameter.Tag, out ReferenceControlParameter? referenceControlParameter))
                    return false;

                return referenceControlParameter!.IsCdaSignatureRequested();
            }

            #endregion

            #region S910.35

            private bool IsApplicationAuthenticationCryptogram(ReferenceControlParameter referenceControlParameter) =>
                referenceControlParameter.GetCryptogramType() == CryptogramTypes.ApplicationAuthenticationCryptogram;

            #endregion

            #region S910.37

            private void HandleInvalidResponse(KernelSessionId sessionId)
            {
                _Database.Update(Level2Error.CardDataError);
                _ResponseHandler.ProcessInvalidDataResponse(sessionId);
            }

            #endregion
        }
    }
}