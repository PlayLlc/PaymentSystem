using Play.Emv.DataExchange;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.State;

public abstract class KernelState
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;

    #endregion

    #region Constructor

    protected KernelState(KernelDatabase kernelDatabase, DataExchangeKernelService dataExchange)
    {
        _KernelDatabase = kernelDatabase;
        _DataExchangeKernelService = dataExchange;
    }

    #endregion

    #region Instance Members

    public abstract StateId GetStateId();
    public abstract KernelState Handle(KernelSession session, ActivateKernelRequest signal);
    public abstract KernelState Handle(CleanKernelRequest signal);
    public abstract KernelState Handle(KernelSession session, QueryKernelRequest signal);
    public abstract KernelState Handle(KernelSession session, StopKernelRequest signal);
    public abstract KernelState Handle(KernelSession session, UpdateKernelRequest signal);
    public abstract KernelState Handle(KernelSession session, QueryPcdResponse signal);
    public abstract KernelState Handle(KernelSession session, QueryTerminalResponse signal);

    public void Clear()
    {
        _KernelDatabase.Deactivate();
        _DataExchangeKernelService.Clear();
    }

    protected static void HandleRequestOutOfSync(KernelSession session, IExchangeDataWithTheKernel signal)
    {
        if (signal.GetDataExchangeKernelId().GetKernelSessionId() != session.GetKernelSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    protected static void HandleRequestOutOfSync(KernelSession session, IExchangeDataWithTheTerminal signal)
    {
        if (signal.GetDataExchangeTerminalId().GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    #endregion
}