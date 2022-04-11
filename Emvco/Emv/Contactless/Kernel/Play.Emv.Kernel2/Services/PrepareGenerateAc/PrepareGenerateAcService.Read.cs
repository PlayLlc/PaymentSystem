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
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.PrepareGenerateAc
{
    public partial class PrepareGenerateAcService
    {
        private class ReadIntegratedDataStorage
        {
            #region Instance Values

            private readonly KernelDatabase _Database;

            #endregion

            #region Instance Members

            public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
            {
                CryptogramType acType = session.GetApplicationCryptogramType();

                var referenceControlParam = new ReferenceControlParameter(CryptogramTypes.AuthorizationRequestCryptogram, true);

                _Database.Update(referenceControlParam); 

                _Database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

                GenerateApplicationCryptogramRequest.Create(session.GetTransactionSessionId(),)


            }

            #region GAC.27 - GAC.29



            private void SetAcTypeInReferenceControl(Kernel2Session session)
            {



            }

            #endregion

            #region GAC.29

            private void HandleGenerateAcRequest(ReferenceControlParameter referenceControlParameter, Kernel2Session session)
            {


            }

            #endregion

            #endregion
        }
    }
}