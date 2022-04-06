using System;

using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private class ValidResponseHandler
        {
            private readonly KernelDatabase _Database;
            private readonly DataExchangeKernelService _DataExchangeKernelService;
            private readonly IKernelEndpoint _KernelEndpoint;

            #region Instance Members

            public ValidResponseHandler(KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint)
            {
                _Database = database;
                _DataExchangeKernelService = dataExchangeKernelService;
                _KernelEndpoint = kernelEndpoint;
            }

            public StateId HandleValidResponse(IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId)
            {
                throw new NotImplementedException();
            }

        #endregion
        }
}