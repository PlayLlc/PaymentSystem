using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.PrepareGenerateAc
{
    public partial class PrepareGenerateAcService
    {
        private class WriteIntegratedDataStorage
        {
            #region Instance Values

            private readonly KernelDatabase _Database;
            private readonly IHandlePcdRequests _PcdEndpoint;

            #endregion

            #region Instance Members

            public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
            {
                DataStorageDataObjectList dataStorageDataObjectList =
                    _Database.Get<DataStorageDataObjectList>(DataStorageDataObjectList.Tag);
            }

            #region GAC.40

            private bool IsDataStorageDigestHashPresent(DataStorageDataObjectList dsdol) =>
                dsdol.IsObjectPresent(DataStorageDigestHash.Tag);

            #endregion

            #region GAC.41

            private bool IsDataStorageInputTermPresent() => _Database.IsPresent(DataStorageInputTerminal.Tag);

            #endregion

            #region GAC.42 - GAC.44

            private void UpdateDataStorageDigestHash()
            {
                DataStorageVersionNumbers number 
            }

            #endregion

            #region GAC.45 - GAC.48

            private void HandleGenerateAcCapdu(Kernel2Session session)
            {
                CryptogramType acType = session.GetApplicationCryptogramType();
                ReferenceControlParameter referenceControlParam = new(acType, true);

                _Database.Update(referenceControlParam);

                CardRiskManagementDataObjectList1RelatedData cdol1RelatedData =
                    _Database.Get<CardRiskManagementDataObjectList1RelatedData>(CardRiskManagementDataObjectList1RelatedData.Tag);

                GenerateApplicationCryptogramRequest? capdu = GenerateApplicationCryptogramRequest.Create(session.GetTransactionSessionId(), referenceControlParam,
                    cdol1RelatedData, dataStorageDataObjectList.AsDataObjectListResult(_Database));

                _PcdEndpoint.Request(capdu);
            }

            #endregion

            private void InitializeDataStorageDigestHash()
            {
                ApplicationCapabilitiesInformation? applicationCapabilitiesInformation = _Database.Get<ApplicationCapabilitiesInformation>(ApplicationCapabilitiesInformation.Tag);

                if(applicationCapabilitiesInformation.GetDataStorageVersionNumber() == DataStorageVersionNumbers.Version1)
                    Owhf2.
 
            }

            private void SetVersion1Hash()
            {
                //owhf2
            }
            private void SetVersion2Hash()
            {
                //OWHF2AES(

            }

            #endregion
        }
    }
}