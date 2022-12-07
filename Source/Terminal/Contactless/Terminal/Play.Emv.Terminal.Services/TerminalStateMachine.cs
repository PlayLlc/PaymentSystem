using System;

using Play.Emv.Configuration;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;

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
        TerminalConfiguration terminalConfiguration, IGetTerminalState terminalStateResolver, IGenerateSequenceTraceAuditNumbers sequenceGenerator)
    {
        _TerminalConfiguration = terminalConfiguration;
        _SequenceGenerator = sequenceGenerator;

        _Lock = new TerminalStateLock(terminalStateResolver.GetKernelState(Idle.StateId));
    }

    #endregion

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    public void Handle(ActivateTerminalRequest request)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new RequestOutOfSyncException(
                    $"The {nameof(ActivateTerminalRequest)} can't be processed because the {nameof(TerminalChannel.Id)} already has an active session in progress");
            }

            _Lock.Session = new TerminalSession(new TransactionSessionId(request.GetTransactionType()));
            _Lock.State = _Lock.State.Handle(_Lock.Session, request);
        }
    }

    public void Handle(InitiateSettlementRequest response)
    {
        throw new NotImplementedException();
    }

    public void Handle(OutReaderResponse response)
    {
        lock (_Lock)
        {
            _Lock.State = _Lock.State.Handle(_Lock.Session!, response);
        }
    }

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

    // HACK: This should be handled on a separate process
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