using Play.Emv.Ber;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel.DataExchange;

public partial class DataExchangeKernelService : IQueryDek, IWriteToDek, IDekEndpointClient
{
    #region Instance Values

    protected readonly IReadTlvDatabase _TlvDatabase;
    private readonly ISendTerminalQueryResponse _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly DataExchangeKernelLock _Lock = new();

    #endregion

    #region Constructor

    public DataExchangeKernelService(IHandleTerminalRequests terminalEndpoint, KernelDatabase kernelDatabase, ISendTerminalQueryResponse kernelEndpoint)
    {
        _TerminalEndpoint = terminalEndpoint;
        _KernelEndpoint = kernelEndpoint;
        _TlvDatabase = kernelDatabase;
    }

    #endregion

    #region Instance Members

    public void Clear()
    {
        lock (_Lock)
        {
            _Lock.Responses.Clear();
            _Lock.Requests.Clear();
        }
    }

    #endregion
}