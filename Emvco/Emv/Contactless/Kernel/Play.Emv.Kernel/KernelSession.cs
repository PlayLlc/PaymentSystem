using System;

using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Timeouts;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Kernel;

public class KernelSession
{
    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;
    private readonly TimeoutManager _TimeoutManager;
    private readonly CorrelationId _CorrelationId;

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

    #endregion

    #region Timeout Management

    public void StartTimeout(Milliseconds timeout, Action timeoutHandler) => _TimeoutManager.Start(timeout, timeoutHandler);
    public void StopTimeout() => _TimeoutManager.Stop();
    public bool TimedOut() => _TimeoutManager.TimedOut();

    #endregion
}