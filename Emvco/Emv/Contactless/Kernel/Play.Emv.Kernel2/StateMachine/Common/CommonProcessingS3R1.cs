using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Kernel2.StateMachine;
// TODO: Note that symbols S3R1.10, S3R1.11, S3R1.12, S3R1.13 and S3R1.18 are only implemented for the IDS/TORN Implementation Option.

public class CommonProcessingS3R1 : CommonProcessing
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly IHandlePcdRequests _PcdEndpoint;

    protected override StateId[] _ValidStateIds { get; } = new StateId[]
    {
        WaitingForGpoResponse.StateId, WaitingForExchangeRelayResistanceDataResponse.StateId
    };

    #endregion

    #region Constructor

    public CommonProcessingS3R1(
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchangeKernelService,
        IKernelEndpoint kernelEndpoint,
        IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint)
    {
        _KernelDatabase = kernelDatabase;
        _DataExchangeKernelService = dataExchangeKernelService;
        _KernelEndpoint = kernelEndpoint;
        _KernelStateResolver = kernelStateResolver;
        _PcdEndpoint = pcdEndpoint;
    }

    #endregion

    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Process(IGetKernelStateId kernelStateId, Kernel2Session session)
    {
        HandleRequestOutOfSync(kernelStateId.GetStateId());

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
        else
            HandleIdsFlags(session);

        if (!session.IsActiveTagEmpty())
            return _KernelStateResolver.GetKernelState(WaitingForGetDataResponse.StateId);

        return _KernelStateResolver.GetKernelState(WaitingForEmvModeFirstWriteFlag.StateId);
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private void HandleIdsFlags(Kernel2Session session)
    {
        if (!_KernelDatabase.IsIdsAndTtrSupported())
        {
            SetOfflineAuthNotPerformed();

            return;
        }

        if (!_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out TagLengthValue? idsStatus))
        {
            SetOfflineAuthNotPerformed();

            return;
        }

        if (!IntegratedDataStorageStatus.Decode(idsStatus!.EncodeValue().AsSpan()).IsReadSet())
        {
            SetOfflineAuthNotPerformed();

            return;
        }

        // BUG: This looks to be a discrepancy in the EMV Specification. Check out the Jira story 'S3R1 CDA & IDS Read Set'
        SetCombinedDataAuthFlag(session);
    }

    // HACK: This should live in State 3 Section C
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    #region S3R1.1 - S3R1.4

    /// <remarks> EMV Book C-2 Section S3R1.1 - S3R1.4 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryHandleGetDataToBeDone(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekRequestType.TagsToRead, out Tag tagToRead))
            return false;

        _PcdEndpoint.Request(GetDataRequest.Create(tagToRead, sessionId));

        return true;
    }

    #endregion

    #region S3R1.1 - S3R1.9

    /// <remarks> EMV Book C-2 Section S3R1.1 - S3R1.9 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TrySendingNextCommand(KernelSession session)
    {
        if (TryHandleGetDataToBeDone(session.GetTransactionSessionId()))
            return false;

        if (!TryHandleRemainingApplicationFilesToRead(session))
            return false;

        return true;
    }

    #endregion

    #region S3R1.5 - S3R1.9

    /// <remarks> EMV Book C-2 Section S3R1.5 - S3R1.9 </remarks>
    public bool TryHandleRemainingApplicationFilesToRead(KernelSession session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            return false;

        _PcdEndpoint.Request(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));

        return true;
    }

    #endregion

    #region S3R1.10 - S3R1.11

    /// <remarks> EMV Book C-2 Section S3R1.10 - S3R1.11 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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
    /// <exception cref="TerminalDataException"></exception>
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
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void StopReadingIntegratedStorage(Kernel2Session session)
    {
        IntegratedDataStorageStatus idsStatus =
            IntegratedDataStorageStatus.Decode(_KernelDatabase.Get(IntegratedDataStorageStatus.Tag).EncodeValue().AsSpan());

        _KernelDatabase.Update(idsStatus.SetRead(false));
    }

    #endregion

    #region S3R1.14

    /// <remarks> EMV Book C-2 Section S3R1.14 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public void ResolveKnownTagsToRead()
    {
        _DataExchangeKernelService.Resolve(_KernelDatabase);
    }

    #endregion

    #region S3R1.15

    /// <remarks> EMV Book C-2 Section S3R1.15 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsDataExchangeNeeded()
    {
        if (_DataExchangeKernelService.IsEmpty(DekRequestType.DataNeeded))
            return true;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend) && _DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead))
            return true;

        return false;
    }

    #endregion

    #region S3R1.16

    /// <remarks> EMV Book C-2 Section S3R1.16 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
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
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <remarks> EMV Book C-2 Section S3R1.19 </remarks>
    public void SetCombinedDataAuthFlag(Kernel2Session session)
    {
        session.Update(OdaStatusTypes.Cda);
    }

    #endregion

    #region S3R1.20

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <remarks> EMV Book C-2 Section S3R1.20 </remarks>
    public void SetOfflineAuthNotPerformed()
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.OfflineDataAuthenticationWasNotPerformed);
    }

    #endregion

    #endregion

    #region S3R1.18

    #endregion
}