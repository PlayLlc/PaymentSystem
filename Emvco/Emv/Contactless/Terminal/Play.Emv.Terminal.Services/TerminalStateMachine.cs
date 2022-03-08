using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Configuration;
using Play.Emv.DataElements.Emv;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Messaging;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Services;

internal class TerminalStateMachine
{
    #region Instance Values

    private readonly TerminalStateLock _Lock;
    private readonly TerminalConfiguration _TerminalConfiguration;
    private readonly IGenerateSequenceTraceAuditNumbers _SequenceGenerator;

    #endregion

    #region Constructor

    public TerminalStateMachine(
        TerminalConfiguration terminalConfiguration,
        IGetTerminalState terminalStateResolver,
        IGenerateSequenceTraceAuditNumbers sequenceGenerator)
    {
        _TerminalConfiguration = terminalConfiguration;
        _SequenceGenerator = sequenceGenerator;

        _Lock = new TerminalStateLock(terminalStateResolver.GetKernelState(Idle.StateId));
    }

    #endregion

    #region Instance Members

    // TODO: I'm not sure if this is the correct way to implement this. I'm not sure what form of communication the point of sale will have with the Terminal. Keeping this here for now
    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(ActivateTerminalRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateTerminalRequest)} can't be processed because the {nameof(ChannelType.Terminal)} already has an active session in progress");
            }

            Transaction transaction = new(new TransactionSessionId(request.GetTransactionType()), request.GetAccountType(),
                request.GetAmountAuthorizedNumeric(), request.GetAmountOtherNumeric(), request.GetTransactionType(),
                _TerminalConfiguration.GetLanguagePreference(), _TerminalConfiguration.GetTerminalCountryCode(),
                new TransactionDate(DateTimeUtc.Now()), new TransactionTime(DateTimeUtc.Now()));

            _Lock.Session = new TerminalSession(_SequenceGenerator.Generate(), request.GetMessageTypeIndicator(), transaction);
            _Lock.State = _Lock.State.Handle(_Lock.Session, request);
        }
    }

    public void Handle(AcquirerResponseSignal request)
    {
        lock (_Lock)
        {
            _Lock.State = _Lock.State.Handle(_Lock.Session, request);
        }
    }

    public void Handle(OutReaderResponse response)
    {
        lock (_Lock)
        {
            _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(QueryKernelResponse response)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryKernelResponse)} can't be processed because the {nameof(TerminalStateMachine)} has an active session");
            }

            _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(StopReaderAcknowledgedResponse response)
    {
        lock (_Lock)
        {
            if (_Lock.Session == null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryKernelResponse)} can't be processed because the {nameof(TerminalStateMachine)} does not have an active session");
            }

            // Clear state inside the handler
            _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(InitiateSettlementRequest response)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryKernelResponse)} can't be processed because the {nameof(TerminalStateMachine)} has an active session");
            }

            _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
        }
    }

    // HACK: This should be handled on a separate process
    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(QueryTerminalRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(QueryKernelResponse)} can't be processed because the {nameof(TerminalStateMachine)} has an active session");
            }

            _Lock.State = _Lock.State.Handle(_Lock.Session!, request);
        }
    }

    #endregion

    internal class TerminalStateLock
    {
        #region Instance Values

        public TerminalSession? Session;
        public TerminalState State;

        #endregion

        #region Constructor

        public TerminalStateLock(TerminalState state)
        {
            State = state;
        }

        #endregion
    }
}