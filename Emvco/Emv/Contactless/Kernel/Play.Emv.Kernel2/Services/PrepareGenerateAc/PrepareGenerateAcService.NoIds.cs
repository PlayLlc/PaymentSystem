using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.PrepareGenerateAc
{
    public partial class PrepareGenerateAcService
    {
        private class NoIntegratedDataStorage
        {
            #region Instance Values

            private readonly KernelDatabase _Database;
            private readonly IHandlePcdRequests _PcdEndpoint;
            private readonly CdaFailure _CdaFailure;
            private readonly ReadIntegratedDataStorage _ReadIntegratedDataStorage;

            #endregion

            #region Constructor

            public NoIntegratedDataStorage(
                KernelDatabase database, IHandlePcdRequests pcdEndpoint, CdaFailure cdaFailure,
                ReadIntegratedDataStorage readIntegratedDataStorage)
            {
                _Database = database;
                _PcdEndpoint = pcdEndpoint;
                _CdaFailure = cdaFailure;
                _ReadIntegratedDataStorage = readIntegratedDataStorage;
            }

            #endregion

            #region Instance Members

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="OverflowException"></exception>
            /// <exception cref="BerParsingException"></exception>
            public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
            {
                if (session.GetOdaStatus() != OdaStatusTypes.Cda)
                {
                    HandleCapdu(session);

                    return currentStateIdRetriever.GetStateId();
                }

                // GAC.21
                if (_Database.IsSet(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed))

                    // GAC.21 - GAC.23, GAC.26, GAC.29
                    return HandleCdaFailed(currentStateIdRetriever, session);

                if (session.GetApplicationCryptogramType() != CryptogramTypes.ApplicationAuthenticationCryptogram)
                    return _ReadIntegratedDataStorage.Process(currentStateIdRetriever, session, message);

                if (IsCdaSupportedByApplicationForAcTypes())
                    return _ReadIntegratedDataStorage.Process(currentStateIdRetriever, session, message);

                HandleCapdu(session);

                return currentStateIdRetriever.GetStateId();
            }

            #region GAC.21 - GAC.23, GAC.26, GAC.29

            /// <exception cref="TerminalDataException"></exception>
            private StateId HandleCdaFailed(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
            {
                if (IsOnDeviceCardholderVerificationSupported())
                    UpdateAac(session);

                HandleCapdu(session);

                return currentStateIdRetriever.GetStateId();
            }

            #endregion

            #region GAC.25

            /// <exception cref="TerminalDataException"></exception>
            private bool IsCdaSupportedByApplicationForAcTypes()
            {
                ApplicationCapabilitiesInformation aci =
                    _Database.Get<ApplicationCapabilitiesInformation>(ApplicationCapabilitiesInformation.Tag);

                return aci.CombinedDataAuthenticationIndicator();
            }

            #endregion

            #region GAC.29

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="OverflowException"></exception>
            /// <exception cref="BerParsingException"></exception>
            private void SendGenerateAcCommand(Kernel2Session session)
            {
                CardRiskManagementDataObjectList1RelatedData cdol1RelatedData =
                    _Database.Get<CardRiskManagementDataObjectList1RelatedData>(CardRiskManagementDataObjectList1RelatedData.Tag);

                _PcdEndpoint.Request(GenerateApplicationCryptogramRequest.Create(session.GetTransactionSessionId(),
                    _Database.Get<ReferenceControlParameter>(ReferenceControlParameter.Tag), cdol1RelatedData));
            }

            #endregion

            #endregion

            #region CDA FAILURE

            #region GAC.22

            /// <exception cref="TerminalDataException"></exception>
            private bool IsOnDeviceCardholderVerificationSupported()
            {
                if (!_Database.IsOnDeviceCardholderVerificationSupported())
                    return false;

                if (!_Database.Get<ApplicationInterchangeProfile>(ApplicationInterchangeProfile.Tag)
                    .IsOnDeviceCardholderVerificationSupported())
                    return false;

                return true;
            }

            #endregion

            #region GAC.23

            private void UpdateAac(Kernel2Session session)
            {
                session.Update(CryptogramTypes.ApplicationAuthenticationCryptogram);
            }

            #endregion

            #endregion

            #region GAC.26, GAC.29

            /// <exception cref="TerminalDataException"></exception>
            private void HandleCapdu(Kernel2Session session)
            {
                SetReferenceControlParameterWithoutCdaSignature(session);
                SendGenerateAcCommand(session);
            }

            #region GAC.26

            /// <exception cref="TerminalDataException"></exception>
            private void SetReferenceControlParameterWithoutCdaSignature(Kernel2Session session)
            {
                _Database.Update(new ReferenceControlParameter(session.GetApplicationCryptogramType(), false));
            }

            #endregion

            #endregion
        }
    }
}