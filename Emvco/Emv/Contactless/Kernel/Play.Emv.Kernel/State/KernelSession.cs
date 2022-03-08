using System;

using Play.Ber.Identifiers;
using Play.Emv.Kernel.State;
using Play.Emv.Sessions;
using Play.Globalization.Time;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Kernel;

public class KernelSession
{
    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;
    private readonly TimeoutManager _TimeoutManager;
    private readonly CorrelationId _CorrelationId;
    private readonly ActiveApplicationFileLocator _ActiveApplicationFileLocator = new();

    #endregion

    #region Constructor

    public KernelSession(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        _CorrelationId = correlationId;
        _KernelSessionId = kernelSessionId;
        _TimeoutManager = new TimeoutManager();
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _CorrelationId;
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();
    public void EnqueueActiveApplicationFileLocator(RecordRange[] values) => _ActiveApplicationFileLocator.Enqueue(values);
    public bool TryDequeueActiveApplicationFileLocator(out RecordRange? result) => _ActiveApplicationFileLocator.TryDequeue(out result);

    #endregion

    #region Timeout Management

    public void StartTimeout(Milliseconds timeout, Action timeoutHandler) => _TimeoutManager.Start(timeout, timeoutHandler);
    public void StopTimeout() => _TimeoutManager.Stop();
    public bool TimedOut() => _TimeoutManager.TimedOut();

    #endregion
}