using System;

using Play.Ber.DataObjects;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Kernel2.StateMachine;

public class S78 : CommonProcessing
{
    #region Instance Values

    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForMagStripeReadRecordResponse.StateId, WaitingForMagstripeFirstWriteFlag.StateId
    };

    #endregion

    #region Constructor

    public S78(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint, IGenerateUnpredictableNumber unpredictableNumberGenerator) :
        base(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint)
    {
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
    }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
    {
        HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

        // S78.1 - S78.7
        if (TryWaitingForFirstWriteFlag(session))
            return WaitingForMagstripeFirstWriteFlag.StateId;

        // S78.8
        if (IsAmountAuthorizedEmpty())
        {
            // S78.9
            HandleLevel3Error(session.GetKernelSessionId());

            return currentStateIdRetriever.GetStateId();
        }

        // S78.10 - S78.11
        if (TryHandlingMaxTransactionAmountExceeded(session.GetKernelSessionId()))
            return currentStateIdRetriever.GetStateId();

        // S78.12 - S78.14
        HandleDataExchange(session.GetKernelSessionId());

        // S78.15
        GenerateUnpredictableNumber();

        // S78.16 - S78.18
        if (TryWaitingForCryptographicCheckSum1(session.GetTransactionSessionId()))
            return WaitingForCccResponse1.StateId;

        HandleWaitingForCryptographicCheckSum2(session.GetTransactionSessionId());

        return WaitingForCccResponse2.StateId;
    }

    #region S78.1 - S78.7

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool TryWaitingForFirstWriteFlag(KernelSession session)
    {
        if (!IsProceedToFirstWriteFlagNonZero())
            return false;

        if (IsProceedToFirstWriteFlagEmpty())
            EnqueueProceedToFirstWriteFlag();

        HandleWaitingForFirstWriteFlag(session);

        return true;
    }

    #endregion

    #region S78.1

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool IsProceedToFirstWriteFlagEmpty() => !_Database.IsPresentAndNotEmpty(ProceedToFirstWriteFlag.Tag);

    #endregion

    #region S78.2

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private void EnqueueProceedToFirstWriteFlag() =>
        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, ProceedToFirstWriteFlag.Tag);

    #endregion

    #region S78.3 - S78.6

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private void HandleWaitingForFirstWriteFlag(KernelSession session)
    {
        // S78.3
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);

        // S78.4 - S78.5
        AttemptToExchangeData(session.GetKernelSessionId());

        // S78.6
        session.Timer.Start((Milliseconds) (TimeoutValue) _Database.Get(TimeoutValue.Tag));
    }

    #endregion

    #region S78.4 - S78.5

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
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

    #region S78.7

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool IsProceedToFirstWriteFlagNonZero()
    {
        if (_Database.TryGet(ProceedToFirstWriteFlag.Tag, out PrimitiveValue? result))
            return false;

        return (ProceedToFirstWriteFlag) result! != 0;
    }

    #endregion

    #region S78.8

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool IsAmountAuthorizedEmpty() => !_Database.IsPresentAndNotEmpty(AmountAuthorizedNumeric.Tag);

    #endregion

    #region S78.9

    private void HandleLevel3Error(KernelSessionId sessionId)
    {
        try
        {
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(Level3Error.AmountNotPresent);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(sessionId));
        }
    }

    #endregion

    #region S78.10 - S78.11

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool TryHandlingMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        if (!IsMaxTransactionAmountExceeded())
            return false;

        HandleMaxTransactionAmountExceeded(sessionId);

        return true;
    }

    #endregion

    #region S78.10

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool IsMaxTransactionAmountExceeded()
    {
        AmountAuthorizedNumeric authorizedAmount = _Database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        ReaderContactlessTransactionLimit transactionLimit = _Database.GetReaderContactlessTransactionLimit();

        // BUG: We need to make sure that the application currency and transaction currency are the same. Need to resolve the Terminal Reference Currency if they are different
        TransactionCurrencyCode currency = _Database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        return authorizedAmount.AsMoney(currency) > transactionLimit.AsMoney(currency);
    }

    #endregion

    #region S78.11

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private void HandleMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        _Database.Update(FieldOffRequestOutcome.NotAvailable);
        _Database.Update(StatusOutcome.SelectNext);
        _Database.Update(StartOutcome.C);
        _Database.Update(Level2Error.MaxLimitExceeded);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S78.12 - S78.14

    /// <exception cref="TerminalDataException"></exception>
    private void HandleDataExchange(KernelSessionId sessionId)
    {
        ResolveKnownTagsToReadYet();

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        // HACK: The correlationId cannot be null where. We need to revisit the pattern we're using the resolve requests and responses and implement that pattern here
        _DataExchangeKernelService.SendResponse(sessionId, null);
    }

    #endregion

    #region S78.12

    /// <exception cref="TerminalDataException"></exception>
    private void ResolveKnownTagsToReadYet()
    {
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);
    }

    #endregion

    #region S78.15

    /// <exception cref="TerminalDataException"></exception>
    private void GenerateUnpredictableNumber()
    {
        NumberOfNonZeroBits nun = new(_Database.Get<PunatcTrack2>(PunatcTrack2.Tag),
                                      _Database.Get<NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2
                                          .Tag));

        UnpredictableNumber unpredictableNumber = _UnpredictableNumberGenerator.GenerateUnpredictableNumber(nun);
        _Database.Update(unpredictableNumber);
    }

    #endregion

    #region S78.16 - S78.18

    /// <exception cref="TerminalDataException"></exception>
    private bool TryWaitingForCryptographicCheckSum1(TransactionSessionId sessionId)
    {
        if (!_Database.IsOnDeviceCardholderVerificationSupported())
            return false;

        if (!_Database.Get<ApplicationInterchangeProfile>(ApplicationInterchangeProfile.Tag).IsOnDeviceCardholderVerificationSupported())
            return false;

        // We populate the UnpredictableNumberDataObjectList with a default value when we initialize the Kernel Session.
        // So even if the card doesn't return a UnpredictableNumberDataObjectList, we can use its default value
        UnpredictableNumberDataObjectList udol = _Database.Get<UnpredictableNumberDataObjectList>(UnpredictableNumberDataObjectList.Tag);

        _PcdEndpoint.Request(ComputeCryptographicChecksumRequest.Create(sessionId, udol));

        return true;
    }

    #endregion

    #region S78.19 - S78.22

    /// <exception cref="TerminalDataException"></exception>
    private void HandleWaitingForCryptographicCheckSum2(TransactionSessionId sessionId)
    {
        if (IsCvmRequiredLimitExceeded())
            SetOfflinePinRequired();

        // S78.21
        UnpredictableNumberDataObjectList udol = _Database.Get<UnpredictableNumberDataObjectList>(UnpredictableNumberDataObjectList.Tag);

        // S78.22
        _PcdEndpoint.Request(ComputeCryptographicChecksumRequest.Create(sessionId, udol));
    }

    #endregion

    #region S78.19

    /// <exception cref="TerminalDataException"></exception>
    private bool IsCvmRequiredLimitExceeded()
    {
        AmountAuthorizedNumeric authorizedAmount = _Database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        ReaderCvmRequiredLimit transactionLimit = _Database.Get<ReaderCvmRequiredLimit>(ReaderCvmRequiredLimit.Tag);

        // BUG: We need to make sure that the application currency and transaction currency are the same. Need to resolve the Terminal Reference Currency if they are different
        TransactionCurrencyCode currency = _Database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        return authorizedAmount.AsMoney(currency) > transactionLimit.AsMoney(currency);
    }

    #endregion

    #region S78.20

    /// <exception cref="TerminalDataException"></exception>
    private void SetOfflinePinRequired()
    {
        _Database.SetOnDeviceCvmRequired(true);
        _Database.Update(CvmPerformedOutcome.ConfirmationCodeVerified);
    }

    #endregion
}