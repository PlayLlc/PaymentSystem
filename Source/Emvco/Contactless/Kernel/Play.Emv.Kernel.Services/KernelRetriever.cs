using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.Services;

public class KernelRetriever
{
    #region Instance Values

    private readonly Dictionary<KernelId, KernelProcess> _KernelMap;
    private KernelProcess? _ActiveKernel;
    private KernelSessionId? _ActiveKernelSessionId;

    #endregion

    #region Constructor

    public KernelRetriever(params KernelProcess[] kernelMap)
    {
        _KernelMap = kernelMap.ToDictionary(a => a.GetKernelId(), b => b);
        _ActiveKernel = null;
        _ActiveKernelSessionId = null;
    }

    #endregion

    private bool IsActive() => _ActiveKernel is not null;

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    public virtual void Enqueue(ActivateKernelRequest message)
    {
        if (IsActive())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not activate Kernel{message.GetKernelId()} because Kernel{_ActiveKernel!.GetKernelId()} is already active");
        }

        _ActiveKernelSessionId = message.GetKernelSessionId();
        _ActiveKernel = _KernelMap[message.GetKernelId()];
        _ActiveKernel.Enqueue(message);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public virtual void Enqueue(CleanKernelRequest message)
    {
        if (!IsActive())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(CleanKernelRequest)} because no kernels are currently active");
        }

        if (message.GetKernelId() != _ActiveKernel!.GetKernelId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(CleanKernelRequest)} for Kernel{message.GetKernelId()} because Kernel{_ActiveKernel.GetKernelId()} is currently active");
        }

        if (message.GetKernelSessionId() != _ActiveKernelSessionId)
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(CleanKernelRequest)} for {nameof(KernelSessionId)}: [{message.GetKernelSessionId()}] because the {nameof(KernelSessionId)}: [{_ActiveKernelSessionId}] is currently active");
        }

        _ActiveKernel.Enqueue(message);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public virtual void Enqueue(QueryKernelRequest message)
    {
        if (!IsActive())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryKernelRequest)} because no kernels are currently active");
        }

        if (message.GetKernelId() != _ActiveKernel!.GetKernelId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryKernelRequest)} for Kernel{message.GetKernelId()} because Kernel{_ActiveKernel.GetKernelId()} is currently active");
        }

        if (message.GetTransactionSessionId() != _ActiveKernelSessionId!.Value.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryKernelRequest)} for {nameof(TransactionSessionId)}: [{message.GetTransactionSessionId()}] because the {nameof(TransactionSessionId)}: [{_ActiveKernelSessionId!.Value.GetTransactionSessionId()}] is currently active");
        }

        _ActiveKernel.Enqueue(message);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public virtual void Enqueue(StopKernelRequest message)
    {
        if (!IsActive())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(StopKernelRequest)} because no kernels are currently active");
        }

        if (message.GetKernelId() != _ActiveKernel!.GetKernelId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(StopKernelRequest)} for Kernel{message.GetKernelId()} because Kernel{_ActiveKernel.GetKernelId()} is currently active");
        }

        if (message.GetKernelSessionId() != _ActiveKernelSessionId!)
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(StopKernelRequest)} for {nameof(KernelSessionId)}: [{message.GetKernelSessionId()}] because the {nameof(KernelSessionId)}: [{_ActiveKernelSessionId!}] is currently active");
        }

        _ActiveKernel.Enqueue(message);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public virtual void Enqueue(UpdateKernelRequest message)
    {
        if (!IsActive())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(UpdateKernelRequest)} because no kernels are currently active");
        }

        if (message.GetKernelId() != _ActiveKernel!.GetKernelId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(UpdateKernelRequest)} for Kernel{message.GetKernelId()} because Kernel{_ActiveKernel.GetKernelId()} is currently active");
        }

        if (message.GetTransactionSessionId() != _ActiveKernelSessionId!.Value.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(UpdateKernelRequest)} for {nameof(TransactionSessionId)}: [{message.GetTransactionSessionId()}] because the {nameof(TransactionSessionId)}: [{_ActiveKernelSessionId!.Value.GetTransactionSessionId()}] is currently active");
        }

        _ActiveKernel.Enqueue(message);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public virtual void Enqueue(QueryPcdResponse message)
    {
        if (!IsActive())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryPcdResponse)} because no kernels are currently active");
        }

        if (message.GetTransactionSessionId() != _ActiveKernelSessionId!.Value.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryPcdResponse)} for {nameof(TransactionSessionId)}: [{message.GetTransactionSessionId()}] because the {nameof(TransactionSessionId)}: [{_ActiveKernelSessionId!.Value.GetTransactionSessionId()}] is currently active");
        }

        _ActiveKernel!.Enqueue(message);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public virtual void Enqueue(QueryTerminalResponse message)
    {
        if (!IsActive())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryTerminalResponse)} because no kernels are currently active");
        }

        if (message.GetKernelId() != _ActiveKernel!.GetKernelId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryTerminalResponse)} for Kernel{message.GetKernelId()} because Kernel{_ActiveKernel.GetKernelId()} is currently active");
        }

        if (message.GetTransactionSessionId() != _ActiveKernelSessionId!.Value.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(KernelRetriever)} could not process the {nameof(QueryTerminalResponse)} for {nameof(TransactionSessionId)}: [{message.GetTransactionSessionId()}] because the {nameof(TransactionSessionId)}: [{_ActiveKernelSessionId!.Value.GetTransactionSessionId()}] is currently active");
        }

        _ActiveKernel!.Enqueue(message);
    }

    #endregion
}