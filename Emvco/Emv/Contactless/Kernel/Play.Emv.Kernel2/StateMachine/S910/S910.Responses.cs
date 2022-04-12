using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine.S910;

public partial class S910
{
    private partial class ResponseHandler
    {
        #region Instance Values

        private readonly KernelDatabase _Database;
        private readonly DataExchangeKernelService _DataExchangeKernelService;
        private readonly IKernelEndpoint _KernelEndpoint;
        private readonly IHandlePcdRequests _PcdEndpoint;

        #endregion

        #region Constructor

        public ResponseHandler(
            KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
            IHandlePcdRequests pcdEndpoint)
        {
            _Database = database;
            _DataExchangeKernelService = dataExchangeKernelService;
            _KernelEndpoint = kernelEndpoint;
            _PcdEndpoint = pcdEndpoint;
        }

        #endregion
    }
}