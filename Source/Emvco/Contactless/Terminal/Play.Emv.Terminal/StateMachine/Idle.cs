using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Ber.DataElements;
using Play.Emv.Configuration;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.DataExchange;
using Play.Emv.Terminal.Session;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Terminal.StateMachine;

/// <summary>
///     This state handles Terminal requests when a transaction session is not currently being processed
/// </summary>
internal class Idle : TerminalState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(Idle));

    #endregion

    #region Instance Values

    private readonly ISettleTransactions _SettleTransactions;

    #endregion

    #region Constructor

    public Idle(
        TerminalConfiguration terminalConfiguration, DataExchangeTerminalService dataExchangeTerminalService, IEndpointClient endpointClient,
        IGetTerminalState terminalStateResolver, ISettleTransactions settleTransactions) : base(dataExchangeTerminalService, terminalConfiguration,
        endpointClient, terminalStateResolver)
    {
        _SettleTransactions = settleTransactions;
    }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    public override TerminalState Handle(TerminalSession session, ActivateTerminalRequest signal)
    {
        Transaction transaction = new(new TransactionSessionId(signal.GetTransactionType()), signal.GetAccountType(), signal.GetAmountAuthorizedNumeric(),
            signal.GetAmountOtherNumeric(), signal.GetTransactionType(), _TerminalConfiguration.GetLanguagePreference(),
            _TerminalConfiguration.GetTerminalCountryCode(), new TransactionDate(DateTimeUtc.Now), new TransactionTime(DateTimeUtc.Now),
            _TerminalConfiguration.GetTransactionCurrencyExponent(), _TerminalConfiguration.GetTransactionCurrencyCode());

        _EndpointClient.Send(new ActivateReaderRequest(transaction));

        return _TerminalStateResolver.GetKernelState(WaitingForFinalOutcome.StateId);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, OutReaderResponse signal) => throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, QueryKernelResponse signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, StopReaderAcknowledgedResponse signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <summary>
    ///     There shouldn't be any incoming responses from the Acquirer while we're in the <see cref="Idle" /> state. There are
    ///     some instances where the Acquirer can send requests directly to the terminal for operations such as file
    ///     management, but we should not expect a reply from the terminal in this state
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession? session, ReconciliationResponse signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override TerminalState Handle(TerminalSession session, QueryTerminalRequest signal) =>
        throw new RequestOutOfSyncException(signal, TerminalChannel.Id);

    #endregion
}