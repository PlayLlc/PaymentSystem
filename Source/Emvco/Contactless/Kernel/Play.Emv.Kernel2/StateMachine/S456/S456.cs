using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Ber.ValueTypes.DataStorage;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Kernel2.Services.PrepareGenerateAc;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Security.Exceptions;
using Play.Globalization.Time.Seconds;
using Play.Icc.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

/// <summary>
///     This object includes logic that is common to states 4, 5, and 6
/// </summary>
public class S456 : CommonProcessing
{
    #region Instance Values

    private readonly IReadOfflineBalance _OfflineBalanceReader;
    private readonly PrepareGenerateAcService _PrepareGenAcServiceService;
    private readonly IValidateCombinationCapability _CombinationCapabilityValidator;
    private readonly IValidateCombinationCompatibility _CombinationCompatibilityValidator;
    private readonly ISelectCardholderVerificationMethod _CardholderVerificationMethodSelector;
    private readonly IPerformTerminalActionAnalysis _TerminalActionAnalyzer;
    private readonly IManageTornTransactions _TornTransactionManager;

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId
    };

    #endregion

    #region Constructor

    public S456(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IKernelEndpoint kernelEndpoint, IReadOfflineBalance offlineBalanceReader, IValidateCombinationCapability combinationCapabilityValidator,
        IValidateCombinationCompatibility combinationCompatibilityValidator, ISelectCardholderVerificationMethod cardholderVerificationMethodSelector,
        IPerformTerminalActionAnalysis terminalActionAnalyzer, IManageTornTransactions tornTransactionManager) : base(database, dataExchangeKernelService,
        kernelStateResolver, pcdEndpoint, kernelEndpoint)
    {
        _OfflineBalanceReader = offlineBalanceReader;
        _CombinationCapabilityValidator = combinationCapabilityValidator;
        _CombinationCompatibilityValidator = combinationCompatibilityValidator;
        _CardholderVerificationMethodSelector = cardholderVerificationMethodSelector;
        _TerminalActionAnalyzer = terminalActionAnalyzer;
        _TornTransactionManager = tornTransactionManager;
    }

    #endregion

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
    {
        HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

        if (TryToGetNeededData())
            return WaitingForGetDataResponse.StateId;

        if (TryToReadApplicationData(session))
            return WaitingForEmvReadRecordResponse.StateId;

        if (TryWaitingForFirstWriteFlag(session))
            return WaitingForEmvModeFirstWriteFlag.StateId;

        // S456.12
        if (IsAmountAuthorizedEmpty())
        {
            // S456.13
            HandleLevel3Error(session.GetKernelSessionId());

            return currentStateIdRetriever.GetStateId();
        }

        // S456.14 - S456.15
        if (TryHandlingMaxTransactionAmountExceeded(session.GetKernelSessionId()))
            return currentStateIdRetriever.GetStateId();

        // S456.16 - S456.17.1
        if (TryHandleMandatoryDataObjectsAreMissing(session.GetKernelSessionId()))
            return currentStateIdRetriever.GetStateId();

        // S456.18 - S456.20.2
        if (TryHandleIntegratedDataStorageError(session.GetKernelSessionId()))
            return currentStateIdRetriever.GetStateId();

        // S456.21
        EnqueueKnownTagsToRead();

        // S456.22 - S456.23
        AttemptToExchangeDataToSend(session.GetKernelSessionId());

        // S456.24 - S456.27.2
        if (TryToHandleCdaError(session))
            return currentStateIdRetriever.GetStateId();

        // S456.30 - S456.3
        InitializeCvmFlags();

        // S456.34
        if (IsStateTransitionNeededAfterReadingOfflineBalance(currentStateIdRetriever, session, message, out StateId? stateIdTransition))
            return stateIdTransition!.Value;

        // S456.35
        ValidateProcessingRestrictions();

        // S456.36
        SelectCardholderVerificationMethod();

        // S456.37 - S456.38
        AttemptToSetTransactionExceedsFloorLimitFlag(_Database);

        // S456.39
        PerformTerminalActionAnalysis(session, _Database);

        // S456.42, S456.50 - S456.51
        if (TryToWriteDataBeforeGeneratingApplicationCryptogram(session.GetTransactionSessionId()))
            return WaitingForPutDataResponseBeforeGenerateAc.StateId;

        // S456.44, S456.47 - S456.49
        if (TryRecoveringTornTransaction(session))
            return WaitingForRecoverAcResponse.StateId;

        // S456.45 - S456.46
        SendGenerateAcCapdu(currentStateIdRetriever, session, message);

        return WaitingForGenerateAcResponse1.StateId;
    }

    #region S456.1

    /// <remarks>EMV Book C-2 Section S456.1</remarks>
    private bool TryToGetNeededData() => _DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead);

    #endregion

    #region S456.2 - S456.4

    /// <remarks>EMV Book C-2 Section S456.2 - S456.4</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryToReadApplicationData(Kernel2Session session)
    {
        if (!session.IsActiveTagEmpty())
            return false;

        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);

        // HACK: The correlationId cannot be null where. We need to revisit the pattern we're using the resolve requests and responses and implement that pattern here
        _DataExchangeKernelService.SendResponse(session.GetKernelSessionId(), null);

        return true;
    }

    #endregion

    #region S456.5

    /// <remarks>EMV Book C-2 Section S456.5</remarks>
    private bool IsProceedToFirstWriteFlagEmpty() => !_Database.IsPresentAndNotEmpty(ProceedToFirstWriteFlag.Tag);

    #endregion

    #region S456.8

    /// <remarks>EMV Book C-2 Section S456.8</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void AttemptToExchangeData(KernelSessionId sessionId)
    {
        if (_DataExchangeKernelService.IsEmpty(DekRequestType.DataNeeded))
            _DataExchangeKernelService.SendRequest(sessionId);

        if (!_DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead))
            return;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        // HACK: The correlationId cannot be null where. We need to revisit the pattern we're using the resolve requests and responses and implement that pattern here
        _DataExchangeKernelService.SendResponse(sessionId, null);
    }

    #endregion

    #region S456.6 - S456.10

    /// <remarks>EMV Book C-2 Section S456.6 - S456.10</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryWaitingForFirstWriteFlag(KernelSession session)
    {
        if (!IsProceedToFirstWriteFlagNonZero())
            return false;

        if (IsProceedToFirstWriteFlagEmpty())
            _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, ProceedToFirstWriteFlag.Tag);

        HandleWaitingForFirstWriteFlag(session);

        return true;
    }

    #endregion

    #region S456.7 - S456.10

    /// <remarks>EMV Book C-2 Section S456.7 - S456.10</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleWaitingForFirstWriteFlag(KernelSession session)
    {
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);
        AttemptToExchangeData(session.GetKernelSessionId());
        session.Timer.Start((Milliseconds) (TimeoutValue) _Database.Get(TimeoutValue.Tag));
    }

    #endregion

    #region S456.11

    /// <remarks>EMV Book C-2 Section S456.11</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsProceedToFirstWriteFlagNonZero()
    {
        if (_Database.TryGet(ProceedToFirstWriteFlag.Tag, out PrimitiveValue? result))
            return false;

        return (ProceedToFirstWriteFlag) result! != 0;
    }

    #endregion

    #region S456.12

    /// <remarks>EMV Book C-2 Section S456.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsAmountAuthorizedEmpty() => !_Database.IsPresentAndNotEmpty(AmountAuthorizedNumeric.Tag);

    #endregion

    #region S456.13

    /// <remarks>EMV Book C-2 Section S456.13</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleLevel3Error(KernelSessionId sessionId)
    {
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(Level3Error.AmountNotPresent);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.14 - S456.15

    /// <remarks>EMV Book C-2 Section S456.14 - S456.15</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandlingMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        if (!IsMaxTransactionAmountExceeded())
            return false;

        HandleMaxTransactionAmountExceeded(sessionId);

        return true;
    }

    #endregion

    #region S456.14

    /// <remarks>EMV Book C-2 Section S456.14</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsMaxTransactionAmountExceeded()
    {
        AmountAuthorizedNumeric authorizedAmount = _Database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        ReaderContactlessTransactionLimit transactionLimit = _Database.GetReaderContactlessTransactionLimit();

        // BUG: We need to make sure that the application currency and transaction currency are the same. Need to resolve the Terminal Reference Currency if they are different
        TransactionCurrencyCode currency = _Database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        return authorizedAmount.AsMoney(currency) > transactionLimit.AsMoney(currency);
    }

    #endregion

    #region S456.15

    /// <remarks>EMV Book C-2 Section S456.15</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        _Database.Update(FieldOffRequestOutcome.NotAvailable);
        _Database.Update(StatusOutcomes.SelectNext);
        _Database.Update(StartOutcomes.C);
        _Database.Update(Level2Error.MaxLimitExceeded);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.16

    /// <remarks>EMV Book C-2 Section S456.16</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool AreMandatoryDataObjectsPresent()
    {
        if (!_Database.IsPresentAndNotEmpty(ApplicationExpirationDate.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(ApplicationPan.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(CardRiskManagementDataObjectList1.Tag))
            return false;

        return true;
    }

    #endregion

    #region S456.16 - S456.17.2

    /// <remarks>EMV Book C-2 Section S456.16 - S456.17.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleMandatoryDataObjectsAreMissing(KernelSessionId sessionId)
    {
        if (!AreMandatoryDataObjectsPresent())
            return false;

        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.CardDataMissing);
        _Database.SetUiRequestOnOutcomePresent(true);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));

        return true;
    }

    #endregion

    #region S456.18 - S456.20.2

    /// <remarks>EMV Book C-2 Section S456.18 - S456.20.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleIntegratedDataStorageError(KernelSessionId sessionId)
    {
        if (!_Database.IsIdsAndTtrImplemented())
            return false;

        if (!_Database.IsIntegratedDataStorageReadFlagSet())
            return false;

        if (IsDataStorageIdValid())
            return false;

        HandleIntegratedDataStorageError(sessionId);

        return true;
    }

    #endregion

    #region S456.19

    /// <remarks>EMV Book C-2 Section S456.19</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsDataStorageIdValid()
    {
        DataStorageId dataStorageId = _Database.Get<DataStorageId>(DataStorageId.Tag);
        ApplicationPan applicationPan = _Database.Get<ApplicationPan>(ApplicationPan.Tag);
        _Database.TryGet(ApplicationPanSequenceNumber.Tag, out ApplicationPanSequenceNumber? applicationPanSequenceNumber);

        return dataStorageId.IsDataStorageIdValid(applicationPan, applicationPanSequenceNumber);
    }

    #endregion

    #region S456.20.1 - S456.20.2

    /// <remarks>EMV Book C-2 Section S456.20.1 - S456.20.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleIntegratedDataStorageError(KernelSessionId sessionId)
    {
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.CardDataError);
        _Database.SetUiRequestOnOutcomePresent(true);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.21

    /// <remarks>EMV Book C-2 Section S456.21</remarks>
    private void EnqueueKnownTagsToRead() => _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);

    #endregion

    #region S456.22 - S456.23

    /// <remarks>EMV Book C-2 Section S456.22 - S456.23</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void AttemptToExchangeDataToSend(KernelSessionId sessionId)
    {
        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        // HACK: The correlationId cannot be null where. We need to revisit the pattern we're using the resolve requests and responses and implement that pattern here
        _DataExchangeKernelService.SendResponse(sessionId, null);
    }

    #endregion

    #region S456.24 - S456.27.2

    /// <remarks>EMV Book C-2 Section S456.24 - S456.27.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryToHandleCdaError(Kernel2Session session)
    {
        if (session.GetOdaStatus() != OdaStatusTypes.Cda)
            return false;

        AttemptToHandleCdaError();

        if (TryHandlingStaticDataAuthenticationError(session))
            return true;

        return false;
    }

    #endregion

    #region S456.25

    /// <remarks>EMV Book C-2 Section S456.25</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void AttemptToHandleCdaError()
    {
        if (!AreMandatoryCdaObjectsPresent())
        {
            HandleMandatoryCdaObjectsAreNotPresent();

            return;
        }

        if (!IsCaPublicCertificatePresent())
        {
            HandleCaPublicKeyNotPresent();
        }
    }

    #endregion

    #region S456.25 continued

    /// <remarks>EMV Book C-2 Section S456.25 continued</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool AreMandatoryCdaObjectsPresent()
    {
        if (!_Database.IsPresentAndNotEmpty(CaPublicKeyIndex.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(IssuerPublicKeyCertificate.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(IssuerPublicKeyExponent.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(IccPublicKeyCertificate.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(IccPublicKeyExponent.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(StaticDataAuthenticationTagList.Tag))
            return false;

        return true;
    }

    #endregion

    #region S456.25 continued

    /// <remarks>EMV Book C-2 Section S456.25 continued</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsCaPublicCertificatePresent()
    {
        ApplicationDedicatedFileName applicationName = _Database.Get<ApplicationDedicatedFileName>(ApplicationDedicatedFileName.Tag);
        CaPublicKeyIndex caPublicKeyIndex = _Database.Get<CaPublicKeyIndex>(CaPublicKeyIndex.Tag);

        RegisteredApplicationProviderIndicator rid = applicationName.GetRegisteredApplicationProviderIndicator();

        if (!_Database.TryGet(rid, caPublicKeyIndex, out CaPublicKeyCertificate? caPublicKeyCertificate))
            return false;

        return true;
    }

    #endregion

    #region S456.25 continued

    /// <remarks>EMV Book C-2 Section S456.25 continued</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleMandatoryCdaObjectsAreNotPresent()
    {
        _Database.Set(TerminalVerificationResultCodes.IccDataMissing);
        _Database.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);
    }

    #endregion

    #region S456.25 continued

    /// <remarks>EMV Book C-2 Section S456.25 continued</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleCaPublicKeyNotPresent()
    {
        _Database.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);
    }

    #endregion

    #region S456.26 - S456.28

    /// <remarks>EMV Book C-2 Section S456.26 - S456.28</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandlingStaticDataAuthenticationError(Kernel2Session session)
    {
        if (!_Database.IsPresentAndNotEmpty(StaticDataAuthenticationTagList.Tag))
        {
            HandleStaticDataAuthenticationError(session.GetKernelSessionId());

            return true;
        }

        try
        {
            StaticDataAuthenticationTagList staticDataAuthenticationTagList =
                _Database.Get<StaticDataAuthenticationTagList>(StaticDataAuthenticationTagList.Tag);
            session.EnqueueStaticDataToBeAuthenticated(staticDataAuthenticationTagList, _Database);
        }
        catch (CryptographicAuthenticationMethodFailedException)
        {
            // TODO: Logging
            HandleStaticDataAuthenticationError(session.GetKernelSessionId());

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Logging
            HandleStaticDataAuthenticationError(session.GetKernelSessionId());

            return true;
        }

        return false;
    }

    #endregion

    #region S456.27.1 - S456.27.2

    /// <remarks>EMV Book C-2 Section S456.27.1 - S456.27.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleStaticDataAuthenticationError(KernelSessionId sessionId)
    {
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.CardDataError);
        _Database.SetUiRequestOnOutcomePresent(true);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.30 - S456.33

    /// <remarks>EMV Book C-2 Section S456.30 - S456.33</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void InitializeCvmFlags()
    {
        if (IsCvmLimitExceeded())
            SetCvmNotRequiredFlags();
        else
            SetCvmRequiredFlags();
    }

    #endregion

    #region S456.30 - S456.33

    /// <remarks>EMV Book C-2 Section S456.30 - S456.33</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsCvmLimitExceeded()
    {
        // BUG: We need to make sure that we're grabbing the correct currency code when comparing units of money. Take a look at the specification and see when you're supposed to be using TransactionReferenceCurrencyCode
        TransactionCurrencyCode transactionCurrencyCode = _Database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);
        AmountAuthorizedNumeric amountAuthorizedNumeric = _Database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);
        ReaderCvmRequiredLimit readerCvmRequiredLimit = _Database.Get<ReaderCvmRequiredLimit>(ReaderCvmRequiredLimit.Tag);

        return amountAuthorizedNumeric.AsMoney(transactionCurrencyCode) > readerCvmRequiredLimit.AsMoney(transactionCurrencyCode);
    }

    #endregion

    #region S456.31 - S456.32

    /// <remarks>EMV Book C-2 Section S456.31 - S456.32</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetCvmRequiredFlags()
    {
        _Database.SetIsReceiptPresent(true);
        _Database.SetCardVerificationMethodNotRequired(false);
    }

    #endregion

    #region S456.33

    /// <remarks>EMV Book C-2 Section S456.33</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetCvmNotRequiredFlags()
    {
        _Database.SetCardVerificationMethodNotRequired(true);
    }

    #endregion

    #region S456.34

    /// <remarks>EMV Book C-2 Section S456.34</remarks>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsStateTransitionNeededAfterReadingOfflineBalance(
        IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message rapdu, out StateId? stateId)
    {
        stateId = _OfflineBalanceReader.Process(currentStateIdRetriever, session, rapdu);

        return stateId != currentStateIdRetriever.GetStateId();
    }

    #endregion

    #region S456.35

    /// <remarks>EMV Book C-2 Section S456.35</remarks>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void ValidateProcessingRestrictions()
    {
        _CombinationCompatibilityValidator.Process(_Database);
        _CombinationCapabilityValidator.Process(_Database);
    }

    #endregion

    #region S456.36

    /// <remarks>EMV Book C-2 Section S456.36</remarks>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void SelectCardholderVerificationMethod()
    {
        _CardholderVerificationMethodSelector.Process(_Database);
    }

    #endregion

    #region S456.37 - S456.38

    /// <remarks>EMV Book C-2 Section S456.37 - S456.38</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void AttemptToSetTransactionExceedsFloorLimitFlag(KernelDatabase database)
    {
        AmountAuthorizedNumeric authorizedAmount = _Database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        ReaderContactlessFloorLimit transactionLimit = _Database.Get<ReaderContactlessFloorLimit>(ReaderContactlessFloorLimit.Tag);

        // BUG: We need to make sure that the application currency and transaction currency are the same. Need to resolve the Terminal Reference Currency if they are different
        TransactionCurrencyCode currency = _Database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        if (authorizedAmount.AsMoney(currency) > transactionLimit.AsMoney(currency))
            database.Set(TerminalVerificationResultCodes.TransactionExceedsFloorLimit);
    }

    #endregion

    #region S456.39

    /// <remarks>EMV Book C-2 Section S456.39</remarks>
    private void PerformTerminalActionAnalysis(Kernel2Session session, KernelDatabase database) =>
        session.Update(_TerminalActionAnalyzer.Process(session.GetTransactionSessionId(), database));

    #endregion

    #region S456.42, S456.50 - S456.51

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <remarks>EMV Book C-2 Section S456.42, S456.50 - S456.51</remarks>
    private bool TryToWriteDataBeforeGeneratingApplicationCryptogram(TransactionSessionId sessionId)
    {
        if (!_Database.IsPresentAndNotEmpty(TagsToWriteBeforeGeneratingApplicationCryptogram.Tag))
            return false;

        if (!_DataExchangeKernelService.TryPeek(DekResponseType.TagsToWriteBeforeGenAc, out PrimitiveValue? tagToWrite))
            return false;

        SendPutData(sessionId, tagToWrite!);

        return true;
    }

    #endregion

    #region S456.50 - S456.51

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <remarks>EMV Book C-2 Section S456.50 - S456.51</remarks>
    private void SendPutData(TransactionSessionId sessionId, PrimitiveValue tagToWrite)
    {
        PutDataRequest capdu = PutDataRequest.Create(sessionId, tagToWrite);
        _PcdEndpoint.Request(capdu);
    }

    #endregion

    #region S456.44, S456.47 - S456.49

    /// <remarks>EMV Book C-2 Section S456.44, S456.47 - S456.49</remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryRecoveringTornTransaction(Kernel2Session session)
    {
        if (!_Database.IsIdsAndTtrImplemented())
            return false;
        if (!_Database.IsTornTransactionRecoverySupported())
            return false;

        if (_Database.IsPresentAndNotEmpty(ApplicationPanSequenceNumber.Tag))
            return false;

        if (!_TornTransactionManager.TryGet(
            new TornEntry(_Database.Get<ApplicationPan>(ApplicationPan.Tag), _Database.Get<ApplicationPanSequenceNumber>(ApplicationPanSequenceNumber.Tag)),
            out TornRecord? tornRecord))
            return false;

        session.Update(tornRecord!.GetKey());

        if (!_Database.TryGet(DataRecoveryDataObjectListRelatedData.Tag, out DataRecoveryDataObjectListRelatedData? ddolRelatedData))
            throw new TerminalDataException($"The {nameof(S456)} could not complete {nameof(TryRecoveringTornTransaction)} ");

        RecoverAcRequest capdu = RecoverAcRequest.Create(session.GetTransactionSessionId(), ddolRelatedData!);
        _PcdEndpoint.Request(capdu);

        return true;
    }

    #endregion

    #region S456.45 - S456.46

    /// <remarks>EMV Book C-2 Section S456.43 - S456.46</remarks>
    private void SendGenerateAcCapdu(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
    {
        _PrepareGenAcServiceService.Process(currentStateIdRetriever, session, message);
    }

    #endregion

    #endregion
}