using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class S910
{
    private partial class ResponseHandler
    {
        #region Instance Values

        private readonly KernelDatabase _Database;
        private readonly DataExchangeKernelService _DataExchangeKernelService;
        private readonly IEndpointClient _EndpointClient;

        #endregion

        #region Constructor

        public ResponseHandler(KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IEndpointClient endpointClient)
        {
            _Database = database;
            _DataExchangeKernelService = dataExchangeKernelService;
            _EndpointClient = endpointClient;
        }

        #endregion
    }
}