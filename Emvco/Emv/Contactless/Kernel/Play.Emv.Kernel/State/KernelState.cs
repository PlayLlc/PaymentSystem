using Play.Emv.DataElements.Emv.Primitives.Outcome;
using Play.Emv.DataElements.Emv.ValueTypes;
using Play.Emv.DataExchange;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;

namespace Play.Emv.Kernel.State;

public abstract class KernelState
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    private readonly IKernelEndpoint _KernelEndpoint;

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
    /// <exception cref="System.InvalidOperationException"></exception>
    private void HandleBerEncodingException(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.SelectNext);
        builder.Set(StartOutcome.C);
        _KernelDatabase.Update(builder);

        _KernelEndpoint.Send(new OutKernelResponse(correlationId, kernelSessionId, _KernelDatabase.GetOutcome()));
    }

    /// <summary>
    ///     TryHandleTimeout
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public bool TryHandleTimeout(KernelSession session)
    {
        if (!session.TimedOut())
            return false;

        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.EndApplication);
        _KernelDatabase.Update(builder);
        _KernelDatabase.Update(Level3Error.TimeOut);
        _KernelDatabase.Initialize(DiscretionaryData.Tag);
        _DataExchangeKernelService.Initialize(DekResponseType.DiscretionaryData);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _KernelDatabase.GetErrorIndication());

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    /// <summary>
    ///     HandleRequestOutOfSync
    /// </summary>
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