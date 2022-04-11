using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Identifiers;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.PrepareGenerateAc
{
    public partial class PrepareGenerateAcService
    {
        private class CdaFailure
        {
            #region Instance Members

            public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
            { }

            #endregion
        }
    }
}