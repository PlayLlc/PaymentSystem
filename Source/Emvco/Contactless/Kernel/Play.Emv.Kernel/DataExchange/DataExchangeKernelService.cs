using Play.Emv.Ber;
using Play.Emv.Kernel.Databases;
using Play.Messaging;

namespace Play.Emv.Kernel.DataExchange;

public partial class DataExchangeKernelService : IQueryDek, IWriteToDek, IDekEndpointClient
{
    #region Instance Values

    protected readonly IReadTlvDatabase _TlvDatabase;
    private readonly IEndpointClient _EndpointClient;
    private readonly DataExchangeKernelLock _Lock = new();

    #endregion

    #region Constructor

    public DataExchangeKernelService(IEndpointClient endpointClient, KernelDatabase kernelDatabase)
    {
        _EndpointClient = endpointClient;
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