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
    private class ReadIntegratedDataStorage
    {
        #region Instance Values

        private readonly KernelDatabase _Database;
        private readonly IEndpointClient _EndpointClient;

        #endregion

        #region Constructor

        public ReadIntegratedDataStorage(KernelDatabase database, IEndpointClient endpointClient)
        {
            _Database = database;
            _EndpointClient = endpointClient;
        }

        #endregion

        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="BerParsingException"></exception>
        public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
        {
            ReferenceControlParameter referenceControlParam = new(CryptogramTypes.AuthorizationRequestCryptogram, true);

            _Database.Update(referenceControlParam);

            _Database.Get<CardRiskManagementDataObjectList1>(CryptogramInformationData.Tag);
            CardRiskManagementDataObjectList1? cardRiskManagementDataObjectList1 =
                _Database.Get<CardRiskManagementDataObjectList1>(CardRiskManagementDataObjectList1.Tag);
            CardRiskManagementDataObjectList1RelatedData? cdol1RelatedData = new(cardRiskManagementDataObjectList1.AsDataObjectListResult(_Database));

            _EndpointClient.Send(GenerateApplicationCryptogramRequest.Create(session.GetTransactionSessionId(), referenceControlParam, cdol1RelatedData));

            return currentStateIdRetriever.GetStateId();
        }

        #endregion
    }
}