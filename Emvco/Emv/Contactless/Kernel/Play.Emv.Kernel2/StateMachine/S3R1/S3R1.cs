using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

using KernelDatabase = Play.Emv.Kernel.Databases.KernelDatabase;

namespace Play.Emv.Kernel2.StateMachine.S3R1;

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
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint) : base(database, dataExchangeKernelService, kernelStateResolver,
        pcdEndpoint, kernelEndpoint)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
    {
        HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

        if (!TrySendingNextCommand(session))
            HandleCardDataError(session);

        AttemptToHandleIntegratedDataStorage(session);
        ResolveKnownTagsToRead();

        if (!IsIntegratedStorageReadingStillRequired())
            StopReadingIntegratedStorage();

        ResolveKnownTagsToRead();

        if (IsDataExchangeNeeded())
            ExchangeData(session.GetKernelSessionId());

        if (DoesTheCardAndTerminalSupportCombinedDataAuth())
            SetCombinedDataAuthFlag(session);
        else
            HandleIdsFlags(session);

        if (!session.IsActiveTagEmpty())
            return WaitingForGetDataResponse.StateId;

        return WaitingForEmvModeFirstWriteFlag.StateId;
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void HandleIdsFlags(Kernel2Session session)
    {
        if (!_Database.IsIdsAndTtrImplemented())
        {
            SetOfflineAuthNotPerformed();

            return;
        }

        if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
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
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool HandleCardDataError(KernelSession session)
    {
        _Database.Update(Level2Error.CardDataError);
        _Database.Update(MessageIdentifiers.CardError);
        _Database.Update(Status.NotReady);
        _Database.Update(StatusOutcome.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.TryAgain);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _Database.GetErrorIndication());
        _Database.SetUiRequestOnRestartPresent(true);

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
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void AttemptToHandleIntegratedDataStorage(Kernel2Session session)
    {
        if (!_Database.IsIntegratedDataStorageSupported())
            return;

        if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
            return;

        if (!IntegratedDataStorageStatus.Decode(((IntegratedDataStorageStatus) idsStatus!).EncodeValue().AsSpan()).IsReadSet())
            return;

        if (_Database.TryGet(DataStorageSlotAvailability.Tag, out PrimitiveValue? dataStorageSlotAvailability))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotAvailability!);
        if (_Database.TryGet(DataStorageSummary1.Tag, out PrimitiveValue? dataStorageSummary1))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSummary1!);
        if (_Database.TryGet(DataStorageUnpredictableNumber.Tag, out PrimitiveValue? dataStorageUnpredictableNumber))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageUnpredictableNumber!);
        if (_Database.TryGet(DataStorageSlotManagementControl.Tag, out PrimitiveValue? dataStorageSlotManagementControl))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageSlotManagementControl!);
        if (_Database.TryGet(DataStorageOperatorDataSetCard.Tag, out PrimitiveValue? dataStorageOperatorDataSetCard))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, dataStorageOperatorDataSetCard!);
        if (_Database.TryGet(UnpredictableNumber.Tag, out PrimitiveValue? unpredictableNumber))
            _DataExchangeKernelService.Enqueue(DekResponseType.DataToSend, unpredictableNumber!);
    }

    #endregion

    #region S3R1.12

    /// <remarks> EMV Book C-2 Section S3R1.12 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsIntegratedStorageReadingStillRequired()
    {
        if (_Database.IsPresentAndNotEmpty(DataStorageSummary1.Tag) && _Database.IsPresentAndNotEmpty(DataStorageOperatorDataSetCard.Tag))
            return true;

        if (_Database.IsPresentAndNotEmpty(DataStorageSlotAvailability.Tag)
            && _Database.IsPresentAndNotEmpty(DataStorageSummary1.Tag)
            && _Database.IsPresentAndNotEmpty(DataStorageUnpredictableNumber.Tag)
            && _Database.IsPresentAndNotEmpty(DataStorageOperatorDataSetCard.Tag))
            return true;

        return false;
    }

    #endregion

    #region S3R1.13

    /// <remarks> EMV Book C-2 Section S3R1.13 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void StopReadingIntegratedStorage()
    {
        IntegratedDataStorageStatus idsStatus =
            IntegratedDataStorageStatus.Decode(((IntegratedDataStorageStatus) _Database.Get(IntegratedDataStorageStatus.Tag)).EncodeValue()
                .AsSpan());

        _Database.Update(idsStatus.SetRead(false));
    }

    #endregion

    #region S3R1.14

    /// <remarks> EMV Book C-2 Section S3R1.14 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void ResolveKnownTagsToRead()
    {
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);
    }

    #endregion

    #region S3R1.15

    /// <remarks> EMV Book C-2 Section S3R1.15 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
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
    /// <exception cref="TerminalDataException"></exception>
    private void ExchangeData(KernelSessionId sessionId)
    {
        _DataExchangeKernelService.SendRequest(sessionId);

        // HACK: The correlationId cannot be null where. We need to revisit the pattern we're using the resolve requests and responses and implement that pattern here
        _DataExchangeKernelService.SendResponse(sessionId, null);
        _DataExchangeKernelService.Initialize(DekRequestType.DataNeeded);
        _DataExchangeKernelService.Initialize(DekResponseType.DataToSend);
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

    /// <remarks> EMV Book C-2 Section S3R1.20 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void SetOfflineAuthNotPerformed()
    {
        _Database.Set(TerminalVerificationResultCodes.OfflineDataAuthenticationWasNotPerformed);
    }

    #endregion

    #region S3R1.17

    /// <remarks> EMV Book C-2 Section S3R1.17 </remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool DoesTheCardAndTerminalSupportCombinedDataAuth() =>
        _Database.GetAuthenticationType() == AuthenticationTypes.CombinedDataAuthentication;

    #endregion

    #endregion
}