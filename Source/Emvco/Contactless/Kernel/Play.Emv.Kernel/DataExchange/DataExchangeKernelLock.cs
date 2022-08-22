using System.Collections.Concurrent;

using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.DataExchange;

public class DataExchangeKernelLock
{
    #region Instance Values

    public readonly ConcurrentDictionary<Tag, DataExchangeRequest> Requests;
    public readonly ConcurrentDictionary<Tag, DataExchangeResponse> Responses;

    #endregion

    #region Constructor

    public DataExchangeKernelLock()
    {
        Requests = new ConcurrentDictionary<Tag, DataExchangeRequest>();
        Responses = new ConcurrentDictionary<Tag, DataExchangeResponse>();
    }

    #endregion
}