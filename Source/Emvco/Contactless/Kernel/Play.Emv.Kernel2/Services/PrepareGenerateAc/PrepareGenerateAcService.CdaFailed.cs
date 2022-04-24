using System;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.PrepareGenerateAc;

public partial class PrepareGenerateAcService
{
    private class CdaFailure
    {
        #region Instance Values

        private readonly KernelDatabase _Database;
        private readonly IHandlePcdRequests _PcdEndpoint;

        #endregion

        #region Constructor

        public CdaFailure(KernelDatabase database, IHandlePcdRequests pcdEndpoint)
        {
            _Database = database;
            _PcdEndpoint = pcdEndpoint;
        }

        #endregion

        #region Instance Members

        public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
        {
            if (IsOnDeviceCardholderVerificationSupported())
                UpdateAac(session);

            HandleCapdu(session);

            return currentStateIdRetriever.GetStateId();
        }

        #region GAC.22

        /// <remarks>Book C-2 Section GAC.22</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsOnDeviceCardholderVerificationSupported()
        {
            if (!_Database.IsOnDeviceCardholderVerificationSupported())
                return false;

            if (!_Database.Get<ApplicationInterchangeProfile>(ApplicationInterchangeProfile.Tag).IsOnDeviceCardholderVerificationSupported())
                return false;

            return true;
        }

        #endregion

        #region GAC.23

        /// <remarks>Book C-2 Section GAC.23</remarks>
        private void UpdateAac(Kernel2Session session)
        {
            session.Update(CryptogramTypes.ApplicationAuthenticationCryptogram);
        }

        #endregion

        #region GAC.26, GAC.29

        /// <remarks>Book C-2 Section GAC.26, GAC.29</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="CardDataException"></exception>
        private void HandleCapdu(Kernel2Session session)
        {
            SetReferenceControlParameterWithoutCdaSignature(session);
            SendGenerateAcCommand(session);
        }

        #endregion

        #region GAC.26

        /// <remarks>Book C-2 Section GAC.26</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="CardDataException"></exception>
        private void SetReferenceControlParameterWithoutCdaSignature(Kernel2Session session)
        {
            _Database.Update(new ReferenceControlParameter(session.GetApplicationCryptogramType(), false));
        }

        #endregion

        #region GAC.29

        /// <remarks>Book C-2 Section GAC.29</remarks>
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