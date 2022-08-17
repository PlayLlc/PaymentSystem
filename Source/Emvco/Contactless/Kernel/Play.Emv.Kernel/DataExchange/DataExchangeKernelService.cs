using Play.Emv.Ber;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel.DataExchange;

public partial class DataExchangeKernelService : IQueryDek, IWriteToDek, IDekEndpointClient
{
    #region Instance Values

    protected readonly IReadTlvDatabase _TlvDatabase;
    private readonly ISendTerminalQueryResponse _KernelEndpoint;
    private readonly IEndpointClient _EndpointClient;
    private readonly DataExchangeKernelLock _Lock = new();

    #endregion

    #region Constructor

    public DataExchangeKernelService(IEndpointClient endpointClient, KernelDatabase kernelDatabase, ISendTerminalQueryResponse kernelEndpoint)
    {
        _EndpointClient = endpointClient;
        _KernelEndpoint = kernelEndpoint;
        _TlvDatabase = kernelDatabase;
    }

    #endregion

    #region Instnace Members

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