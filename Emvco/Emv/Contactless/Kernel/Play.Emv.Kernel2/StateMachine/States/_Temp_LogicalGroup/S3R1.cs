using System;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup;
// TODO: Note that symbols S3R1.10, S3R1.11, S3R1.12, S3R1.13 and S3R1.18 are only implemented for the IDS/TORN Implementation Option.

public class S3R1
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;
    private readonly IHandlePcdRequests _PcdEndpoint;

    #endregion

    #region Instance Members

    public KernelState Process(Kernel2Session session)
    {
        if (!TrySendingNextCommand(session))
            HandleCardDataError(session);

        throw new NotImplementedException();
    }

    #endregion

    #endregion

    #region S3R1.1 - S3R1.9

    public bool TrySendingNextCommand(KernelSession session)
    {
        if (TryHandleGetDataToBeDone(session.GetTransactionSessionId()))
            return false;

        if (!TryHandleRemainingApplicationFilesToRead(session))
            return false;

        return true;
    }

    #region S3R1.1 - S3R1.4

    public bool TryHandleGetDataToBeDone(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekRequestType.TagsToRead, out Tag tagToRead))
            return false;

        _PcdEndpoint.Request(GetDataRequest.Create(tagToRead, sessionId));

        return true;
    }

    #endregion

    #region S3R1.5 - S3R1.9

    public bool TryHandleRemainingApplicationFilesToRead(KernelSession session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            return false;

        _PcdEndpoint.Request(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));

        return true;
    }

    #endregion

    // TODO: This should live in State 3 Section C
    private bool HandleCardDataError(KernelSession session)
    {
        _KernelDatabase.Update(Level2Error.CardDataError);
        _KernelDatabase.Update(MessageIdentifier.CardError);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.TryAgain);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _KernelDatabase.GetErrorIndication());
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    public void AttemptToHandleIntegratedDataStorage(Kernel2Session session)
    {
        if (!_KernelDatabase.IsIntegratedDataStorageSupported())
            return;

        if (!_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out TagLengthValue? idsStatus))
            return;

        if (!IntegratedDataStorageStatus.Decode(idsStatus!.EncodeValue().AsSpan()).IsReadSet())
            return;

        _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, SlotA.Tag);
    }

    private void EnqueueIntegratedDataStorageData(Kernel2Session session)
    {
        if (_KernelDatabase.TryGet(DataStorageSlotAvailability.Tag, out TagLengthValue? dataStorageSlotAvailabilityTlv))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailabilityTlv!);

        if (_KernelDatabase.TryGet(DataStorageSummary1.Tag, out TagLengthValue? dataStorageSummary1))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSummary1!);

        if (_KernelDatabase.TryGet(DataStorageUn.Tag, out TagLengthValue? dataStorageSlotAvailabilityTlv))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailabilityTlv!);
        if (_KernelDatabase.TryGet(DataStorageSlotAvailability.Tag, out TagLengthValue? dataStorageSlotAvailabilityTlv))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailabilityTlv!);
        if (_KernelDatabase.TryGet(DataStorageSlotAvailability.Tag, out TagLengthValue? dataStorageSlotAvailabilityTlv))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailabilityTlv!);
        if (_KernelDatabase.TryGet(DataStorageSlotAvailability.Tag, out TagLengthValue? dataStorageSlotAvailabilityTlv))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailabilityTlv!);

        /*  
            IF [IsNotEmpty(TagOf(DS Unpredictable Number))] 
            IF [IsNotEmpty(TagOf(DS Slot Management Control))] 
            IF [IsPresent(TagOf(DS ODS Card))] 
            AddToList(GetTLV(TagOf(Unpredictable Number)), Data To Send) 
         */
    }

    #region S3R1.14

    public void EnqueueKnownTagsToReadYet(Kernel2Session session)
    {
        _DataExchangeKernelService.Resolve(_KernelDatabase);
    }

    #endregion
}