using System;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv.Primitives.DataStorage.IntegratedDataStoraged;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
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

    /// <exception cref="BerParsingException"></exception>
    public KernelState Process(Kernel2Session session)
    {
        if (!TrySendingNextCommand(session))
            HandleCardDataError(session);

        AttemptToHandleIntegratedDataStorage(session);
        ResolveKnownTagsToRead();

        if (!IsIntegratedStorageReadingStillRequired())
            StopReadingIntegratedStorage(session);

        ResolveKnownTagsToRead();

        if (IsDataExchangeNeeded())
            ExchangeData(session.GetKernelSessionId());

        if (DoesTheCardAndTerminalSupportCombinedDataAuth(session))
            SetCombinedDataAuthFlag(session);
        else if (_KernelDatabase.IsTornTransactionRecoverySupported())

            throw new NotImplementedException();
    }

    #region S3R1.18

    public void ing

    #endregion

    #region S3R1.1 - S3R1.4

    /// <remarks> EMV Book C-2 Section S3R1.1 - S3R1.4 </remarks>
    public bool TryHandleGetDataToBeDone(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekRequestType.TagsToRead, out Tag tagToRead))
            return false;

        _PcdEndpoint.Request(GetDataRequest.Create(tagToRead, sessionId));

        return true;
    }

    #endregion

    // HACK: This should live in State 3 Section C
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

    #region S3R1.1 - S3R1.9

    /// <remarks> EMV Book C-2 Section S3R1.1 - S3R1.9 </remarks>
    public bool TrySendingNextCommand(KernelSession session)
    {
        if (TryHandleGetDataToBeDone(session.GetTransactionSessionId()))
            return false;

        if (!TryHandleRemainingApplicationFilesToRead(session))
            return false;

        return true;
    }

    #region S3R1.5 - S3R1.9

    /// <remarks> EMV Book C-2 Section S3R1.5 - S3R1.9 </remarks>
    public bool TryHandleRemainingApplicationFilesToRead(KernelSession session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            return false;

        _PcdEndpoint.Request(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));

        return true;
    }

    #region S3R1.10 - S3R1.11

    /// <remarks> EMV Book C-2 Section S3R1.10 - S3R1.11 </remarks>
    public void AttemptToHandleIntegratedDataStorage(Kernel2Session session)
    {
        if (!_KernelDatabase.IsIntegratedDataStorageSupported())
            return;

        if (!_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out TagLengthValue? idsStatus))
            return;

        if (!IntegratedDataStorageStatus.Decode(idsStatus!.EncodeValue().AsSpan()).IsReadSet())
            return;

        if (_KernelDatabase.TryGet(DataStorageSlotAvailability.Tag, out TagLengthValue? dataStorageSlotAvailability))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailability!);
        if (_KernelDatabase.TryGet(DataStorageSummary1.Tag, out TagLengthValue? dataStorageSummary1))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSummary1!);
        if (_KernelDatabase.TryGet(DataStorageUnpredictableNumber.Tag, out TagLengthValue? dataStorageUnpredictableNumber))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageUnpredictableNumber!);
        if (_KernelDatabase.TryGet(DataStorageSlotManagementControl.Tag, out TagLengthValue? dataStorageSlotManagementControl))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotManagementControl!);
        if (_KernelDatabase.TryGet(DataStorageOperatorDataSetCard.Tag, out TagLengthValue? dataStorageOperatorDataSetCard))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageOperatorDataSetCard!);
        if (_KernelDatabase.TryGet(UnpredictableNumber.Tag, out TagLengthValue? unpredictableNumber))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, unpredictableNumber!);
    }

    #endregion

    #region S3R1.12

    /// <remarks> EMV Book C-2 Section S3R1.12 </remarks>
    public bool IsIntegratedStorageReadingStillRequired()
    {
        if (_KernelDatabase.IsPresentAndNotEmpty(DataStorageSummary1.Tag)
            && _KernelDatabase.IsPresentAndNotEmpty(DataStorageOperatorDataSetCard.Tag))
            return true;

        if (_KernelDatabase.IsPresentAndNotEmpty(DataStorageSlotAvailability.Tag)
            && _KernelDatabase.IsPresentAndNotEmpty(DataStorageSummary1.Tag)
            && _KernelDatabase.IsPresentAndNotEmpty(DataStorageUnpredictableNumber.Tag)
            && _KernelDatabase.IsPresentAndNotEmpty(DataStorageOperatorDataSetCard.Tag))
            return true;

        return false;
    }

    #endregion

    #region S3R1.13

    /// <remarks> EMV Book C-2 Section S3R1.13 </remarks>
    public void StopReadingIntegratedStorage(Kernel2Session session)
    {
        IntegratedDataStorageStatus idsStatus =
            IntegratedDataStorageStatus.Decode(_KernelDatabase.Get(IntegratedDataStorageStatus.Tag).EncodeValue().AsSpan());

        _KernelDatabase.Update(idsStatus.SetRead(false));
    }

    #endregion

    #region S3R1.14

    /// <remarks> EMV Book C-2 Section S3R1.14 </remarks>
    public void ResolveKnownTagsToRead()
    {
        _DataExchangeKernelService.Resolve(_KernelDatabase);
    }

    #endregion

    #region S3R1.15

    /// <remarks> EMV Book C-2 Section S3R1.15 </remarks>
    public bool IsDataExchangeNeeded()
    {
        /*
         * [IsNotEmptyList(Data Needed) OR
(IsNotEmptyList(Data To Send) AND IsEmptyList(Tags To Read Yet))] 
         */
        if (_DataExchangeKernelService.IsEmpty(DekRequestType.DataNeeded))
            return true;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend) && _DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead))
            return true;

        return false;
    }

    #endregion

    #region S3R1.16

    /// <remarks> EMV Book C-2 Section S3R1.16 </remarks>
    public void ExchangeData(KernelSessionId sessionId)
    {
        _DataExchangeKernelService.SendRequest(sessionId);
        _DataExchangeKernelService.SendResponse(sessionId);
        _DataExchangeKernelService.Initialize(DekRequestType.DataNeeded);
        _DataExchangeKernelService.Initialize(DekResponseType.DataToSend);
    }

    #endregion

    #region S3R1.17

    /// <remarks> EMV Book C-2 Section S3R1.17 </remarks>
    /// <exception cref="BerParsingException"></exception>
    public bool DoesTheCardAndTerminalSupportCombinedDataAuth(Kernel2Session session)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile =
            ApplicationInterchangeProfile.Decode(_KernelDatabase.Get(ApplicationInterchangeProfile.Tag).EncodeValue().AsSpan());
        TerminalCapabilities terminalCapabilities =
            TerminalCapabilities.Decode(_KernelDatabase.Get(TerminalCapabilities.Tag).EncodeValue().AsSpan());

        if (!applicationInterchangeProfile.IsCombinedDataAuthenticationSupported())
            return false;

        if (terminalCapabilities.IsCombinedDataAuthenticationSupported())
            return false;

        return true;
    }

    #endregion

    #region S3R1.19

    public void SetCombinedDataAuthFlag(Kernel2Session session)
    {
        session.Update(OdaStatusTypes.Cda);
    }

    #endregion

    #endregion

    #endregion
}