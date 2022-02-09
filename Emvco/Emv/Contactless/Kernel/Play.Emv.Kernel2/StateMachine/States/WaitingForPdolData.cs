using System;

using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

internal class WaitingForPdolData : KernelState
{
    #region Static Metadata

    public static readonly KernelStateId KernelStateId = new(nameof(WaitingForPdolData));

    #endregion

    #region Instance Values

    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;
    private readonly KernelDatabase _KernelDatabase;
    private readonly Kernel2Session _KernelSession;

    #endregion

    #region Constructor

    public WaitingForPdolData(
        IKernelEndpoint kernelEndpoint,
        IHandleTerminalRequests terminalEndpoint,
        IHandlePcdRequests pcdEndpoint,
        IGetKernelState kernelStateResolver,
        ICleanTornTransactions kernelCleaner,
        KernelDatabase kernelDatabase,
        Kernel2Session kernelSession)
    {
        _KernelEndpoint = kernelEndpoint;
        _TerminalEndpoint = terminalEndpoint;
        _PcdEndpoint = pcdEndpoint;
        _KernelStateResolver = kernelStateResolver;
        _KernelCleaner = kernelCleaner;
        _KernelDatabase = kernelDatabase;
        _KernelSession = kernelSession;
    }

    #endregion

    public override KernelStateId GetKernelStateId() => KernelStateId;

    #region ACT

    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region CLEAN

    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region STOP

    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        if (_KernelSession.TimedOut())
            HandleTimeout();

        //OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        //builder.Set(StatusOutcome.EndApplication);
        //_KernelDatabase.GetKernelSession().Update(builder);
        //_KernelDatabase.GetKernelSession().Update(Level3Error.Stop);

        //_KernelEndpoint.Send(new OutKernelResponse(signal.GetCorrelationId(), signal.GetKernelSessionId(),
        //                                           _KernelDatabase.GetKernelSession().GetOutcome()));

        //return _KernelStateResolver.GetKernelState(KernelStateId); 
    }

    private void HandleTimeout()
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.EndApplication);
        _KernelSession.Update(Level3Error.TimeOut);
        _KernelSession.Update(builder);

        DataExchangeKernelService dataExchangeService = _KernelDatabase.GetDataExchanger();
        dataExchangeService.Initialize(DekResponseType.DiscretionaryData);
        dataExchangeService.Enqueue(DekResponseType.DiscretionaryData, _KernelSession.GetErrorIndication());

        _KernelEndpoint.Send(new OutKernelResponse(_KernelSession.GetCorrelationId(), _KernelSession.GetKernelSessionId(),
                                                   _KernelDatabase.GetKernelSession().GetOutcome()));
    }

    #endregion

    #region RAPDU

    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region DET

    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) => throw new NotImplementedException();

    #endregion

    // BUG: The messages below should be handled by the DEK
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) => throw new NotImplementedException();
}