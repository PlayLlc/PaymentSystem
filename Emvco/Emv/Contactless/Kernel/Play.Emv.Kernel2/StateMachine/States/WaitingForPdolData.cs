using System;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv;
using Play.Emv.DataExchange;
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
using Play.Emv.Sessions;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

internal class WaitingForPdolData : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForPdolData));

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

    #region Instance Members

    public override StateId GetStateId() => StateId;

    private static void HandleRequestOutOfSync(KernelSession session, IExchangeDataWithTheTerminal signal)
    {
        if (signal.GetDataExchangeTerminalId().GetTransactionSessionId() != session.GetTransactionSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    private static void HandleRequestOutOfSync(KernelSession session, IExchangeDataWithTheKernel signal)
    {
        if (signal.GetDataExchangeKernelId().GetKernelSessionId() != session.GetKernelSessionId())
        {
            throw new RequestOutOfSyncException(
                $"The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel");
        }
    }

    #region ACT

    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #region S2.1

    public bool HandleTimeout(KernelSession session)
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

    #endregion

    #endregion

    #region CLEAN

    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region RAPDU

    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region DET

    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (HandleTimeout(session))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateDataExchangeSignal(signal);

        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (IsPdolDataMissing(kernel2Session, out ProcessingOptionsDataObjectList pdol))
            return _KernelStateResolver.GetKernelState(StateId);

        // TODO: GOTO S2.8.1
        GetProcessingOptionsCommand capdu = CreateGetProcessingOptionsCapdu(session, pdol);
        StopTimer(kernel2Session);

        _PcdEndpoint.Request(capdu);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #region S2.6

    private void UpdateDataExchangeSignal(QueryTerminalResponse signal)
    {
        _KernelDatabase.UpdateRange(signal.GetDataToSend().AsTagLengthValueArray());
    }

    #endregion

    #region S2.7

    private bool IsPdolDataMissing(Kernel2Session session, out ProcessingOptionsDataObjectList pdol)
    {
        pdol = ProcessingOptionsDataObjectList.Decode(_KernelDatabase.Get(ProcessingOptionsDataObjectList.Tag).EncodeValue().AsSpan());

        if (!pdol!.IsRequestedDataAvailable(_KernelDatabase))
            return false;

        ((Kernel2Session) session).SetIsPdolDataMissing(false);

        return true;
    }

    #endregion

    #region S2.8.1 - S2.8.6

    public GetProcessingOptionsCommand CreateGetProcessingOptionsCapdu(KernelSession session, ProcessingOptionsDataObjectList pdol) =>
        !_KernelDatabase.IsPresentAndNotEmpty(ProcessingOptionsDataObjectList.Tag)
            ? GetProcessingOptionsCommand.Create(session.GetTransactionSessionId())
            : GetProcessingOptionsCommand.Create(pdol.AsDataObjectListResult(_KernelDatabase), session.GetTransactionSessionId());

    #endregion

    #region S2.9

    public void StopTimer(Kernel2Session session)
    {
        session.StopTimeout();
    }

    #endregion

    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region STOP

    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.EndApplication);
        _KernelDatabase.Update(builder);

        if (!_KernelDatabase.GetErrorIndication().IsErrorPresent())
            _KernelDatabase.Update(Level3Error.Stop);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), signal.GetKernelSessionId(), _KernelDatabase.GetOutcome()));

        Clear();

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion

    #endregion
}