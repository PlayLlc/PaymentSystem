using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services;

public class Kernel2StateMachine : KernelStateMachine
{
    #region Instance Values

    private readonly Kernel2StateLock _Lock;

    #endregion

    #region Constructor

    public Kernel2StateMachine(KernelState kernelState)
    {
        _Lock = new Kernel2StateLock(kernelState);
    }

    #endregion

    #region Instance Members

    public override void Handle(ActivateKernelRequest signal)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(ActivateKernelRequest)} can't be processed because the {nameof(Kernel2StateMachine)} already has an active session");
            }

            _Lock.Session = new Kernel2Session(signal.GetCorrelationId(), signal.GetKernelSessionId());
            _Lock.KernelState = _Lock.KernelState.Handle(_Lock.Session, signal);
        }
    }

    public override void Handle(CleanKernelRequest signal)
    {
        lock (_Lock)
        {
            if (_Lock.Session != null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(CleanKernelRequest)} can't be processed because the {nameof(Kernel2StateMachine)} has an active session");
            }

            _Lock.KernelState = _Lock.KernelState.Handle(signal);
        }
    }

    public override void Handle(StopKernelRequest signal)
    {
        lock (_Lock)
        {
            if (_Lock.Session == null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(StopKernelRequest)} can't be processed because the {nameof(Kernel2StateMachine)} already has an active session");
            }

            if (_Lock.Session.GetTransactionSessionId() != signal.GetTransactionSessionId())
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(StopKernelRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{signal.GetTransactionSessionId()}] but the current {nameof(ChannelType.Kernel)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _Lock.KernelState = _Lock.KernelState.Handle(_Lock.Session, signal);
            _Lock.Session = null;
        }
    }

    public override void Handle(QueryPcdResponse signal)
    {
        lock (_Lock)
        {
            if (_Lock.Session == null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryPcdResponse)} can't be processed because the {nameof(Kernel2StateMachine)} already has an active session");
            }

            if (_Lock.Session.GetTransactionSessionId() != signal.GetTransactionSessionId())
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryPcdResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{signal.GetTransactionSessionId()}] but the current {nameof(ChannelType.Kernel)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _Lock.KernelState = _Lock.KernelState.Handle(_Lock.Session, signal);
        }
    }

    public override void Handle(QueryTerminalResponse signal)
    {
        lock (_Lock)
        {
            if (_Lock.Session == null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryTerminalResponse)} can't be processed because the {nameof(Kernel2StateMachine)} already has an active session");
            }

            if (_Lock.Session.GetTransactionSessionId() != signal.GetTransactionSessionId())
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryTerminalResponse)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{signal.GetTransactionSessionId()}] but the current {nameof(ChannelType.Kernel)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            _Lock.KernelState = _Lock.KernelState.Handle(_Lock.Session, signal);
        }
    }

    public override void Handle(UpdateKernelRequest signal)
    {
        lock (_Lock)
        {
            if (_Lock.Session == null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(UpdateKernelRequest)} can't be processed because the {nameof(Kernel2StateMachine)} already has an active session");
            }

            if (_Lock.Session.GetTransactionSessionId() != signal.GetTransactionSessionId())
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(UpdateKernelRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{signal.GetTransactionSessionId()}] but the current {nameof(ChannelType.Kernel)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            // BUG: Pass this to the DEK Manager
            throw new NotImplementedException();
        }
    }

    public override void Handle(QueryKernelRequest signal)
    {
        lock (_Lock)
        {
            if (_Lock.Session == null)
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryKernelRequest)} can't be processed because the {nameof(Kernel2StateMachine)} already has an active session");
            }

            if (_Lock.Session.GetTransactionSessionId() != signal.GetTransactionSessionId())
            {
                throw new
                    RequestOutOfSyncException($"The {nameof(QueryKernelRequest)} can't be processed because the {nameof(TransactionSessionId)} from the request is [{signal.GetTransactionSessionId()}] but the current {nameof(ChannelType.Kernel)} session has a {nameof(TransactionSessionId)} of: [{_Lock.Session.GetTransactionSessionId()}]");
            }

            // BUG: Pass this to the DEK Manager
            throw new NotImplementedException();
        }
    }

    #endregion
}