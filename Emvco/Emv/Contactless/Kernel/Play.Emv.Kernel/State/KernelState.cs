using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;

namespace Play.Emv.Kernel.State;

public abstract class KernelState : IGetKernelStateId
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    protected readonly IKernelEndpoint _KernelEndpoint;

    #endregion

    #region Constructor

    protected KernelState(KernelDatabase kernelDatabase, DataExchangeKernelService dataExchange, IKernelEndpoint kernelEndpoint)
    {
        _KernelDatabase = kernelDatabase;
        _DataExchangeKernelService = dataExchange;
        _KernelEndpoint = kernelEndpoint;
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

    #endregion

    #region Exception Handling

    /// <summary>
    ///     HandleBerEncodingException
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="kernelSessionId"></param>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleBerEncodingException(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        _KernelDatabase.Update(StatusOutcome.SelectNext);
        _KernelDatabase.Update(StartOutcome.C);

        _KernelEndpoint.Send(new OutKernelResponse(correlationId, kernelSessionId, _KernelDatabase.GetOutcome()));
    }

    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, IExchangeDataWithTheKernel signal)
    {
        if (signal.GetDataExchangeKernelId().GetKernelSessionId() != session.GetKernelSessionId())
        {
            throw new
                RequestOutOfSyncException($"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, StopKernelRequest signal)
    {
        if (signal.GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new
                RequestOutOfSyncException($"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, ActivateKernelRequest signal)
    {
        if (signal.GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new
                RequestOutOfSyncException($"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new
                RequestOutOfSyncException($"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    /// <summary>
    ///     HandleRequestOutOfSync
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, IExchangeDataWithTheTerminal signal)
    {
        if (signal.GetDataExchangeTerminalId().GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new
                RequestOutOfSyncException($"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    #endregion
}