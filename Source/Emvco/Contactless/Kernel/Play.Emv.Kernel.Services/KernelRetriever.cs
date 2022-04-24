using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.Services;

// BUG: This object needs to be responsible for the lifetime management of the Kernel Session, resolving the current active Kernel, and handling requests that are invalid for the current State of the Kernel.
public class KernelRetriever
{
    #region Instance Values

    private readonly ConcurrentDictionary<KernelId, KernelProcess> _KernelMap;

    #endregion

    #region Constructor

    public KernelRetriever(Dictionary<KernelId, KernelProcess> kernelMap)
    {
        _KernelMap = new ConcurrentDictionary<KernelId, KernelProcess>(kernelMap);
    }

    #endregion

    #region Instance Members

    // HACK: THIS IS WRONG. This object needs to route the request to the correct Kernel instead of the first Kernel in the collection
    public virtual void Enqueue(ActivateKernelRequest message) => _KernelMap.ElementAt(0).Value.Enqueue((dynamic) message);
    public virtual void Enqueue(CleanKernelRequest message) => _KernelMap.ElementAt(0).Value.Enqueue((dynamic) message);
    public virtual void Enqueue(QueryKernelRequest message) => _KernelMap.ElementAt(0).Value.Enqueue((dynamic) message);
    public virtual void Enqueue(StopKernelRequest message) => _KernelMap.ElementAt(0).Value.Enqueue((dynamic) message);
    public virtual void Enqueue(UpdateKernelRequest message) => _KernelMap.ElementAt(0).Value.Enqueue((dynamic) message);
    public virtual void Enqueue(QueryPcdResponse message) => _KernelMap.ElementAt(0).Value.Enqueue((dynamic) message);
    public virtual void Enqueue(QueryTerminalResponse message) => _KernelMap.ElementAt(0).Value.Enqueue((dynamic) message);

    #endregion
}