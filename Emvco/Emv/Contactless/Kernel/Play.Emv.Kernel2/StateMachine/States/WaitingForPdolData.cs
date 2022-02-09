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

    #endregion

    #region Constructor

    public WaitingForPdolData(
        IKernelEndpoint kernelEndpoint,
        IHandleTerminalRequests terminalEndpoint,
        IHandlePcdRequests pcdEndpoint,
        IGetKernelState kernelStateResolver,
        ICleanTornTransactions kernelCleaner,
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchangeKernelService) : base(kernelDatabase, dataExchangeKernelService)
    {
        _KernelEndpoint = kernelEndpoint;
        _TerminalEndpoint = terminalEndpoint;
        _PcdEndpoint = pcdEndpoint;
        _KernelStateResolver = kernelStateResolver;
        _KernelCleaner = kernelCleaner;
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
        Kernel2Session kernel2Session = (Kernel2Session) session;
        if (session.TimedOut())
            HandleTimeout(kernel2Session);

        throw new NotImplementedException();

        //OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        //builder.Set(StatusOutcome.EndApplication);
        //_KernelDatabase.GetKernelSession().Update(builder);
        //_KernelDatabase.GetKernelSession().Update(Level3Error.Stop);

        //_KernelEndpoint.Send(new OutKernelResponse(signal.GetCorrelationId(), signal.GetKernelSessionId(),
        //                                           _KernelDatabase.GetKernelSession().GetOutcome()));

        //return _KernelStateResolver.GetKernelState(KernelStateId); 
    }

    private void HandleTimeout(Kernel2Session session)
    {
        Kernel2Database kernel2Database = (Kernel2Database) _KernelDatabase;
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.EndApplication);
        kernel2Database.Update(Level3Error.TimeOut);
        kernel2Database.Update(builder);

        _DataExchangeKernelService.Initialize(DekResponseType.DiscretionaryData);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, kernel2Database.GetErrorIndication());

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), kernel2Database.GetOutcome()));
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