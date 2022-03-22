using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services.ProcessingRestrictions;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security.Certificates;
using Play.Emv.Security.Exceptions;
using Play.Globalization.Time.Seconds;
using Play.Icc.FileSystem.DedicatedFiles;

using KernelDatabase = Play.Emv.Kernel.Databases.KernelDatabase;

namespace Play.Emv.Kernel2.StateMachine;

/// <summary>
///     This object includes logic that is common to states 4, 5, and 6
/// </summary>
public class S456 : CommonProcessing
{
    #region Instance Values

    private readonly OfflineBalanceReader _OfflineBalanceReader;
    private readonly IValidateCombinationCapability _CombinationCapabilityValidator;
    private readonly IValidateCombinationCompatibility _CombinationCompatibilityValidator;
    private readonly ISelectCardholderVerificationMethod _CardholderVerificationMethodSelector;

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId
    };

    #endregion

    #region Constructor

    public S456(
        KernelDatabase kernelDatabase, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint, OfflineBalanceReader offlineBalanceReader,
        IValidateCombinationCapability combinationCapabilityValidator, IValidateCombinationCompatibility combinationCompatibilityValidator,
        ISelectCardholderVerificationMethod cardholderVerificationMethodSelector) : base(kernelDatabase, dataExchangeKernelService,
                                                                                         kernelStateResolver, pcdEndpoint, kernelEndpoint)
    {
        _OfflineBalanceReader = offlineBalanceReader;
        _CombinationCapabilityValidator = combinationCapabilityValidator;
        _CombinationCompatibilityValidator = combinationCompatibilityValidator;
        _CardholderVerificationMethodSelector = cardholderVerificationMethodSelector;
    }

    #endregion

    #region Instance Members

    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
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
        if (IsStateTransitionNeededAfterReadingOfflineBalance(currentStateIdRetriever, session, out StateId? stateIdTransition))
            return stateIdTransition!.Value;

        // S456.35
        ValidateProcessingRestrictions();

        // S456.36
        SelectCardholderVerificationMethod();

        throw new NotImplementedException();
    }

    #region S456.1

    private bool TryToGetNeededData() => _DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead);

    #endregion

    #region S456.2 - S456.4

    private bool TryToReadApplicationData(Kernel2Session session)
    {
        if (!session.IsActiveTagEmpty())
            return false;

        _DataExchangeKernelService.ResolveTagsToReadYet(_KernelDatabase);
        _DataExchangeKernelService.SendResponse(session.GetKernelSessionId());

        return true;
    }

    #endregion

    #region S456.5

    private bool IsProceedToFirstWriteFlagEmpty() => !_KernelDatabase.IsPresentAndNotEmpty(ProceedToFirstWriteFlag.Tag);

    #endregion

    #region S456.8

    private void AttemptToExchangeData(KernelSessionId sessionId)
    {
        if (_DataExchangeKernelService.IsEmpty(DekRequestType.DataNeeded))
            _DataExchangeKernelService.SendRequest(sessionId);

        if (!_DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead))
            return;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        _DataExchangeKernelService.SendResponse(sessionId);
    }

    #endregion

    #region S456.6 - S456.10

    /// <exception cref="TerminalDataException"></exception>
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

    /// <exception cref="TerminalDataException"></exception>
    private void HandleWaitingForFirstWriteFlag(KernelSession session)
    {
        _DataExchangeKernelService.Resolve(_KernelDatabase);
        AttemptToExchangeData(session.GetKernelSessionId());
        session.Timer.Start((Milliseconds) (TimeoutValue) _KernelDatabase.Get(TimeoutValue.Tag));
    }

    #endregion

    #region S456.11

    /// <exception cref="TerminalDataException"></exception>
    private bool IsProceedToFirstWriteFlagNonZero()
    {
        if (_KernelDatabase.TryGet(ProceedToFirstWriteFlag.Tag, out PrimitiveValue? result))
            return false;

        return (ProceedToFirstWriteFlag) result! != 0;
    }

    #endregion

    #region S456.12

    /// <exception cref="TerminalDataException"></exception>
    private bool IsAmountAuthorizedEmpty() => !_KernelDatabase.IsPresentAndNotEmpty(AmountAuthorizedNumeric.Tag);

    #endregion

    #region S456.13

    /// <exception cref="TerminalDataException"></exception>
    private void HandleLevel3Error(KernelSessionId sessionId)
    {
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(Level3Error.AmountNotPresent);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.14 - S456.15

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        if (!IsMaxTransactionAmountExceeded())
            return false;

        HandleMaxTransactionAmountExceeded(sessionId);

        return true;
    }

    #endregion

    #region S456.14

    /// <exception cref="TerminalDataException"></exception>
    private bool IsMaxTransactionAmountExceeded()
    {
        AmountAuthorizedNumeric authorizedAmount = _KernelDatabase.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        ReaderContactlessTransactionLimit transactionLimit = _KernelDatabase.GetReaderContactlessTransactionLimit();

        // BUG: We need to make sure that the application currency and transaction currency are the same. Need to resolve the Terminal Reference Currency if they are different
        TransactionCurrencyCode currency = _KernelDatabase.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        return authorizedAmount.AsMoney(currency) > transactionLimit.AsMoney(currency);
    }

    #endregion

    #region S456.15

    /// <exception cref="TerminalDataException"></exception>
    private void HandleMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        _KernelDatabase.Update(FieldOffRequestOutcome.NotAvailable);
        _KernelDatabase.Update(StatusOutcome.SelectNext);
        _KernelDatabase.Update(StartOutcome.C);
        _KernelDatabase.Update(Level2Error.MaxLimitExceeded);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.16

    /// <exception cref="TerminalDataException"></exception>
    private bool AreMandatoryDataObjectsPresent()
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationExpirationDate.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationPan.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(CardRiskManagementDataObjectList1.Tag))
            return false;

        return true;
    }

    #endregion

    #region S456.16 - S456.17.2

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandleMandatoryDataObjectsAreMissing(KernelSessionId sessionId)
    {
        if (!AreMandatoryDataObjectsPresent())
            return false;

        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.CardDataMissing);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));

        return true;
    }

    #endregion

    #region S456.18 - S456.20.2

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandleIntegratedDataStorageError(KernelSessionId sessionId)
    {
        if (!_KernelDatabase.IsIdsAndTtrSupported())
            return false;

        if (!_KernelDatabase.IsIntegratedDataStorageReadFlagSet())
            return false;

        if (IsDataStorageIdValid())
            return false;

        HandleIntegratedDataStorageError(sessionId);

        return true;
    }

    #endregion

    #region S456.19

    /// <exception cref="TerminalDataException"></exception>
    private bool IsDataStorageIdValid()
    {
        DataStorageId dataStorageId = _KernelDatabase.Get<DataStorageId>(DataStorageId.Tag);
        ApplicationPan applicationPan = _KernelDatabase.Get<ApplicationPan>(ApplicationPan.Tag);
        _KernelDatabase.TryGet(ApplicationPanSequenceNumber.Tag, out ApplicationPanSequenceNumber? applicationPanSequenceNumber);

        return dataStorageId.IsDataStorageIdValid(applicationPan, applicationPanSequenceNumber);
    }

    #endregion

    #region S456.20.1 - S456.20.2

    /// <exception cref="TerminalDataException"></exception>
    private void HandleIntegratedDataStorageError(KernelSessionId sessionId)
    {
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.CardDataError);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.21

    private void EnqueueKnownTagsToRead() => _DataExchangeKernelService.Resolve(_KernelDatabase);

    #endregion

    #region S456.22 - S456.23

    private void AttemptToExchangeDataToSend(KernelSessionId sessionId)
    {
        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        _DataExchangeKernelService.SendResponse(sessionId);
    }

    #endregion

    #region S456.24 - S456.27.2

    /// <exception cref="TerminalDataException"></exception>
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

    /// <exception cref="TerminalDataException"></exception>
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

            return;
        }
    }

    #endregion

    #region S456.25 continued

    /// <exception cref="TerminalDataException"></exception>
    private bool AreMandatoryCdaObjectsPresent()
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(CaPublicKeyIndex.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IssuerPublicKeyCertificate.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IssuerPublicKeyExponent.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IccPublicKeyCertificate.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IccPublicKeyExponent.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(StaticDataAuthenticationTagList.Tag))
            return false;

        return true;
    }

    #endregion

    #region S456.25 continued

    /// <exception cref="TerminalDataException"></exception>
    private bool IsCaPublicCertificatePresent()
    {
        ApplicationDedicatedFileName applicationName = _KernelDatabase.Get<ApplicationDedicatedFileName>(ApplicationDedicatedFileName.Tag);
        CaPublicKeyIndex caPublicKeyIndex = _KernelDatabase.Get<CaPublicKeyIndex>(CaPublicKeyIndex.Tag);

        RegisteredApplicationProviderIndicator rid = applicationName.GetRegisteredApplicationProviderIndicator();

        if (!_KernelDatabase.TryGet(rid, caPublicKeyIndex, out CaPublicKeyCertificate? caPublicKeyCertificate))
            return false;

        return true;
    }

    #endregion

    #region S456.25 continued

    /// <exception cref="TerminalDataException"></exception>
    private void HandleMandatoryCdaObjectsAreNotPresent()
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.IccDataMissing);
        _KernelDatabase.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);
    }

    #endregion

    #region S456.25 continued

    /// <exception cref="TerminalDataException"></exception>
    private void HandleCaPublicKeyNotPresent()
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);
    }

    #endregion

    #region S456.26 - S456.28

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingStaticDataAuthenticationError(Kernel2Session session)
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(StaticDataAuthenticationTagList.Tag))
        {
            HandleStaticDataAuthenticationError(session.GetKernelSessionId());

            return true;
        }

        try
        {
            StaticDataAuthenticationTagList staticDataAuthenticationTagList =
                _KernelDatabase.Get<StaticDataAuthenticationTagList>(StaticDataAuthenticationTagList.Tag);
            session.EnqueueStaticDataToBeAuthenticated(staticDataAuthenticationTagList, _KernelDatabase);
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

    /// <exception cref="TerminalDataException"></exception>
    private void HandleStaticDataAuthenticationError(KernelSessionId sessionId)
    {
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.CardDataError);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.30 - S456.33

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

    /// <exception cref="TerminalDataException"></exception>
    private bool IsCvmLimitExceeded()
    {
        // BUG: We need to make sure that we're grabbing the correct currency code when comparing units of money. Take a look at the specification and see when you're supposed to be using TransactionReferenceCurrencyCode
        TransactionCurrencyCode transactionCurrencyCode = _KernelDatabase.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);
        AmountAuthorizedNumeric amountAuthorizedNumeric = _KernelDatabase.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);
        ReaderCvmRequiredLimit readerCvmRequiredLimit = _KernelDatabase.Get<ReaderCvmRequiredLimit>(ReaderCvmRequiredLimit.Tag);

        return amountAuthorizedNumeric.AsMoney(transactionCurrencyCode) > readerCvmRequiredLimit.AsMoney(transactionCurrencyCode);
    }

    #endregion

    #region S456.31 - S456.32

    /// <exception cref="TerminalDataException"></exception>
    private void SetCvmRequiredFlags()
    {
        _KernelDatabase.SetIsReceiptPresent(true);
        _KernelDatabase.SetCardVerificationMethodNotRequired(false);
    }

    #endregion

    #region S456.33

    /// <exception cref="TerminalDataException"></exception>
    private void SetCvmNotRequiredFlags()
    {
        _KernelDatabase.SetCardVerificationMethodNotRequired(true);
    }

    #endregion

    #region S456.34

    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsStateTransitionNeededAfterReadingOfflineBalance(
        IGetKernelStateId currentStateIdRetriever, Kernel2Session session, out StateId? stateId)
    {
        stateId = _OfflineBalanceReader.Process(currentStateIdRetriever, session);

        return stateId != currentStateIdRetriever.GetStateId();
    }

    #endregion

    #region S456.35

    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void ValidateProcessingRestrictions()
    {
        _CombinationCompatibilityValidator.Process(_KernelDatabase);
        _CombinationCapabilityValidator.Process(_KernelDatabase);
    }

    #endregion

    #region S456.36

    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void SelectCardholderVerificationMethod()
    {
        _CardholderVerificationMethodSelector.Process(_KernelDatabase);
    }

    #endregion

    #endregion
}