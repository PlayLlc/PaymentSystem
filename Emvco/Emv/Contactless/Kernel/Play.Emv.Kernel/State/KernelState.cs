using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataExchange;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;

namespace Play.Emv.Kernel.State;

public abstract class KernelState : IGetKernelStateId
{
    #region Instance Values

    protected readonly KernelDatabase _Database;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    protected readonly IKernelEndpoint _KernelEndpoint;
    protected readonly IManageTornTransactions _TornTransactionManager;
    protected readonly IGetKernelState _KernelStateResolver;
    protected readonly IHandlePcdRequests _PcdEndpoint;
    protected readonly IHandleDisplayRequests _DisplayEndpoint;

    #endregion

    #region Constructor

    protected KernelState(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
        IManageTornTransactions tornTransactionManager, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IHandleDisplayRequests displayEndpoint)
    {
        _Database = database;
        _DataExchangeKernelService = dataExchangeKernelService;
        _KernelEndpoint = kernelEndpoint;
        _TornTransactionManager = tornTransactionManager;
        _KernelStateResolver = kernelStateResolver;
        _PcdEndpoint = pcdEndpoint;
        _DisplayEndpoint = displayEndpoint;
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
        _Database.Deactivate();
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
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    private void HandleBerEncodingException(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        _Database.Update(StatusOutcome.SelectNext);
        _Database.Update(StartOutcome.C);

        _KernelEndpoint.Send(new OutKernelResponse(correlationId, kernelSessionId, _Database.GetOutcome()));
    }

    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, IExchangeDataWithTheKernel signal)
    {
        if (signal.GetDataExchangeKernelId().GetKernelSessionId() != session.GetKernelSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, StopKernelRequest signal)
    {
        if (signal.GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, ActivateKernelRequest signal)
    {
        if (signal.GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected static void HandleRequestOutOfSync(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
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
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    #endregion
}