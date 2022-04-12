﻿using System;
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
            private readonly ReadIntegratedDataStorage _ReadIntegratedDataStorage;

            #endregion

            #region Constructor

            public NoIntegratedDataStorage(
                KernelDatabase database, IHandlePcdRequests pcdEndpoint, ReadIntegratedDataStorage readIntegratedDataStorage)
            {
                _Database = database;
                _PcdEndpoint = pcdEndpoint;
                _ReadIntegratedDataStorage = readIntegratedDataStorage;
            }

            #endregion

            #region Instance Members

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="OverflowException"></exception>
            /// <exception cref="BerParsingException"></exception>
            /// <exception cref="CardDataException"></exception>
            public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
            {
                if (session.GetOdaStatus() != OdaStatusTypes.Cda)
                {
                    SetReferenceControlParameterWithoutCdaSignature(session);
                    SendGenerateAcCommand(session);

                    return currentStateIdRetriever.GetStateId();
                }

                // GAC.21
                if (_Database.IsSet(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed))
                    return HandleCdaFailed(currentStateIdRetriever, session); // GAC.22 - GAC.23, GAC.26, GAC.29

                // GAC.24
                if (session.GetApplicationCryptogramType() != CryptogramTypes.ApplicationAuthenticationCryptogram)
                    return _ReadIntegratedDataStorage.Process(currentStateIdRetriever, session, message); // GAC.C

                // GAC.25
                if (IsCdaSupportedByApplicationForAcTypes())
                    return _ReadIntegratedDataStorage.Process(currentStateIdRetriever, session, message); // GAC.C

                return HandleCdaFailed(currentStateIdRetriever, session);
            }

            #region GAC.22 - GAC.23, GAC.26, GAC.29

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="OverflowException"></exception>
            /// <exception cref="BerParsingException"></exception>
            /// <exception cref="CardDataException"></exception>
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

            #region GAC.26

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="CardDataException"></exception>
            private void SetReferenceControlParameterWithoutCdaSignature(Kernel2Session session)
            {
                _Database.Update(new ReferenceControlParameter(session.GetApplicationCryptogramType(), false));
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
        }
    }
}