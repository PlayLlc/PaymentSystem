﻿using System;

using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Ber.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.DataExchange;
using Play.Emv.Terminal.Session;

namespace Play.Emv.Terminal.StateMachine;

/// <summary>
///     The Terminal enters this state when a transaction begins processing and stays in this state until the Terminal
///     receives an OUT signal from the Reader
/// </summary>
internal class WaitingForFinalOutcome : TerminalState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForFinalOutcome));

    #endregion

    #region Instance Values

    private readonly IGetTerminalState _TerminalStateResolver;
    private readonly IHandleAcquirerRequests _AcquirerEndpoint;

    #endregion

    #region Constructor

    public WaitingForFinalOutcome(
        DataExchangeTerminalService dataExchangeTerminalService, IGetTerminalState terminalStateResolver,
        IHandleAcquirerRequests acquirerEndpoint) : base(dataExchangeTerminalService)
    {
        _TerminalStateResolver = terminalStateResolver;
        _AcquirerEndpoint = acquirerEndpoint;
    }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;
    public override TerminalState Handle(TerminalSession? session, InitiateSettlementRequest signal) => throw new NotImplementedException();

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override TerminalState Handle(TerminalSession session, OutReaderResponse signal)
    {
        if (session.GetTransactionSessionId() != signal.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The {nameof(QueryTerminalRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{signal.GetTransactionSessionId()}] but the current {nameof(TerminalChannel)} session has a {nameof(TransactionSessionId)} of: [{session.GetTransactionSessionId()}]");
        }

        if (!signal.TryGetKernelSessionId(out KernelSessionId? kernelSessionId))
        {
            // TODO: Handle for failure at Start processing

            return _TerminalStateResolver.GetKernelState(Idle.StateId);
        }

        session.SetFinalOutcome(signal.GetOutcome());

        // HACK: Need to inject logic to determine whether or not we communicate with the acquirer

        AcquirerMessageFactory factory = _AcquirerEndpoint.GetMessageFactory(session!.MessageTypeIndicator);
        DataNeeded neededData = new(factory.GetDataNeeded(session!.MessageTypeIndicator));

        _DataExchangeTerminalService.Enqueue(neededData);
        _DataExchangeTerminalService.QueryKernel(session.GetTransactionSessionId(), kernelSessionId!.Value.GetKernelId());

        return _TerminalStateResolver.GetKernelState(WaitingForAuthorizationResponse.StateId);
    }

    public override TerminalState Handle(TerminalSession session, QueryKernelResponse signal) => throw new NotImplementedException();

    public override TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal) =>
        throw new NotImplementedException();

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession? session, AcquirerResponseSignal signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override TerminalState Handle(TerminalSession session, QueryTerminalRequest signal)
    {
        // TODO: Control flow logic

        _DataExchangeTerminalService.Enqueue(signal.GetDataNeeded());
        _DataExchangeTerminalService.Resolve(session);

        return _TerminalStateResolver.GetKernelState(StateId);
    }

    #endregion
}