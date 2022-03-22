using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;

using KernelDatabase = Play.Emv.Kernel.Databases.KernelDatabase;

namespace Play.Emv.Kernel2.StateMachine;

// TODO: Note that symbols S3R1.10, S3R1.11, S3R1.12, S3R1.13 and S3R1.18 are only implemented for the IDS/TORN Implementation Option.
/// <summary>
///     This object includes logic that is common to states 3: (<see cref="WaitingForGpoResponse" />) and State R: (
///     <see cref="WaitingForExchangeRelayResistanceDataResponse" />)
/// </summary>
public class S3R1 : CommonProcessing
{
    #region Instance Values

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForGpoResponse.StateId, WaitingForExchangeRelayResistanceDataResponse.StateId
    };

    #endregion

    #region Constructor

    public S3R1(
        KernelDatabase kernelDatabase, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint) : base(kernelDatabase, dataExchangeKernelService,
                                                                               kernelStateResolver, pcdEndpoint, kernelEndpoint)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Emv.Exceptions.RequestOutOfSyncException"></exception>
    public override StateId Process(IGetKernelStateId kernelStateId, Kernel2Session session)
    {
        HandleRequestOutOfSync(kernelStateId.GetStateId());

        if (!TrySendingNextCommand(session))
            HandleCardDataError(session);

        AttemptToHandleIntegratedDataStorage(session);
        ResolveKnownTagsToRead();

        if (!IsIntegratedStorageReadingStillRequired())
            StopReadingIntegratedStorage();

        ResolveKnownTagsToRead();

        if (IsDataExchangeNeeded())
            ExchangeData(session.GetKernelSessionId());

        if (DoesTheCardAndTerminalSupportCombinedDataAuth(session))
            SetCombinedDataAuthFlag(session);
        else
            HandleIdsFlags(session);

        if (!session.IsActiveTagEmpty())
            return WaitingForGetDataResponse.StateId;

        return WaitingForEmvModeFirstWriteFlag.StateId;
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

        if (!_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
        {
            SetOfflineAuthNotPerformed();

            return;
        }

        if (!IntegratedDataStorageStatus.Decode(((IntegratedDataStorageStatus) idsStatus!).EncodeValue().AsSpan()).IsReadSet())
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

        return true;
    }

    #region S3R1.1 - S3R1.4

    /// <remarks> EMV Book C-2 Section S3R1.1 - S3R1.4 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleGetDataToBeDone(TransactionSessionId sessionId)
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
    private bool TrySendingNextCommand(KernelSession session)
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
    private bool TryHandleRemainingApplicationFilesToRead(KernelSession session)
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
    private void AttemptToHandleIntegratedDataStorage(Kernel2Session session)
    {
        if (!_KernelDatabase.IsIntegratedDataStorageSupported())
            return;

        if (!_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
            return;

        if (!IntegratedDataStorageStatus.Decode(((IntegratedDataStorageStatus) idsStatus!).EncodeValue().AsSpan()).IsReadSet())
            return;

        if (_KernelDatabase.TryGet(DataStorageSlotAvailability.Tag, out PrimitiveValue? dataStorageSlotAvailability))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailability!);
        if (_KernelDatabase.TryGet(DataStorageSummary1.Tag, out PrimitiveValue? dataStorageSummary1))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSummary1!);
        if (_KernelDatabase.TryGet(DataStorageUnpredictableNumber.Tag, out PrimitiveValue? dataStorageUnpredictableNumber))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageUnpredictableNumber!);
        if (_KernelDatabase.TryGet(DataStorageSlotManagementControl.Tag, out PrimitiveValue? dataStorageSlotManagementControl))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotManagementControl!);
        if (_KernelDatabase.TryGet(DataStorageOperatorDataSetCard.Tag, out PrimitiveValue? dataStorageOperatorDataSetCard))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageOperatorDataSetCard!);
        if (_KernelDatabase.TryGet(UnpredictableNumber.Tag, out PrimitiveValue? unpredictableNumber))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, unpredictableNumber!);
    }

    #endregion

    #region S3R1.12

    /// <remarks> EMV Book C-2 Section S3R1.12 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsIntegratedStorageReadingStillRequired()
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
    private void StopReadingIntegratedStorage()
    {
        IntegratedDataStorageStatus idsStatus =
            IntegratedDataStorageStatus.Decode(((IntegratedDataStorageStatus) _KernelDatabase.Get(IntegratedDataStorageStatus.Tag))
                                               .EncodeValue().AsSpan());

        _KernelDatabase.Update(idsStatus.SetRead(false));
    }

    #endregion

    #region S3R1.14

    /// <remarks> EMV Book C-2 Section S3R1.14 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private void ResolveKnownTagsToRead()
    {
        _DataExchangeKernelService.Resolve(_KernelDatabase);
    }

    #endregion

    #region S3R1.15

    /// <remarks> EMV Book C-2 Section S3R1.15 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsDataExchangeNeeded()
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
    private void ExchangeData(KernelSessionId sessionId)
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
    private bool DoesTheCardAndTerminalSupportCombinedDataAuth(Kernel2Session session)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile =
            ApplicationInterchangeProfile.Decode(((ApplicationInterchangeProfile) _KernelDatabase.Get(ApplicationInterchangeProfile.Tag))
                                                 .EncodeValue().AsSpan());
        TerminalCapabilities terminalCapabilities =
            TerminalCapabilities.Decode(((TerminalCapabilities) _KernelDatabase.Get(TerminalCapabilities.Tag)).EncodeValue().AsSpan());

        if (!applicationInterchangeProfile.IsCombinedDataAuthenticationSupported())
            return false;

        if (terminalCapabilities.IsCombinedDataAuthenticationSupported())
            return false;

        return true;
    }

    #endregion

    #region S3R1.19

    /// <remarks> EMV Book C-2 Section S3R1.19 </remarks>
    private static void SetCombinedDataAuthFlag(Kernel2Session session)
    {
        session.Update(OdaStatusTypes.Cda);
    }

    #endregion

    #region S3R1.20

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <remarks> EMV Book C-2 Section S3R1.20 </remarks>
    private void SetOfflineAuthNotPerformed()
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.OfflineDataAuthenticationWasNotPerformed);
    }

    #endregion

    #endregion

    #region S3R1.18

    // HELLO!

    #endregion
}