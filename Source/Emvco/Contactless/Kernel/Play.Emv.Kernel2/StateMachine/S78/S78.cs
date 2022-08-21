using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public class S78 : CommonProcessing
{
    #region Instance Values

    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;
    protected override StateId[] _ValidStateIds { get; } = {WaitingForMagStripeReadRecordResponse.StateId, WaitingForMagstripeFirstWriteFlag.StateId};

    #endregion

    #region Constructor

    public S78(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver, IEndpointClient endpointClient,
        IGenerateUnpredictableNumber unpredictableNumberGenerator) : base(database, dataExchangeKernelService, kernelStateResolver, endpointClient)
    {
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
    }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
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

    /// <remarks>EMV Book C-2 Section S78.1 - S78.7</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <remarks>EMV Book C-2 Section S78.1</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsProceedToFirstWriteFlagEmpty() => !_Database.IsPresentAndNotEmpty(ProceedToFirstWriteFlag.Tag);

    #endregion

    #region S78.2

    /// <remarks>EMV Book C-2 Section S78.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void EnqueueProceedToFirstWriteFlag() => _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, ProceedToFirstWriteFlag.Tag);

    #endregion

    #region S78.3 - S78.6

    /// <remarks>EMV Book C-2 Section S78.3 - S78.6</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <remarks>EMV Book C-2 Section S78.4 - S78.5</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <remarks>EMV Book C-2 Section S78.7</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsProceedToFirstWriteFlagNonZero()
    {
        if (_Database.TryGet(ProceedToFirstWriteFlag.Tag, out PrimitiveValue? result))
            return false;

        return (ProceedToFirstWriteFlag) result! != 0;
    }

    #endregion

    #region S78.8

    /// <remarks>EMV Book C-2 Section S78.8</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsAmountAuthorizedEmpty() => !_Database.IsPresentAndNotEmpty(AmountAuthorizedNumeric.Tag);

    #endregion

    #region S78.9

    /// <remarks>EMV Book C-2 Section S78.9</remarks>
    private void HandleLevel3Error(KernelSessionId sessionId)
    {
        try
        {
            _Database.Update(StatusOutcomes.EndApplication);
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
            _EndpointClient.Send(new StopKernelRequest(sessionId));
        }
    }

    #endregion

    #region S78.10 - S78.11

    /// <remarks>EMV Book C-2 Section S78.10 - S78.11</remarks>
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

    #region S78.10

    /// <remarks>EMV Book C-2 Section S78.10</remarks>
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

    #region S78.11

    /// <remarks>EMV Book C-2 Section S78.11</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        _Database.Update(FieldOffRequestOutcome.NotAvailable);
        _Database.Update(StatusOutcomes.SelectNext);
        _Database.Update(StartOutcomes.C);
        _Database.Update(Level2Error.MaxLimitExceeded);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);

        _EndpointClient.Send(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S78.12 - S78.14

    /// <remarks>EMV Book C-2 Section S78.12 - S78.14</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <remarks>EMV Book C-2 Section S78.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void ResolveKnownTagsToReadYet()
    {
        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);
    }

    #endregion

    #region S78.15

    /// <remarks>EMV Book C-2 Section S78.15</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void GenerateUnpredictableNumber()
    {
        NumberOfNonZeroBits nun = new(_Database.Get<PunatcTrack2>(PunatcTrack2.Tag),
            _Database.Get<NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2.Tag));

        UnpredictableNumber unpredictableNumber = _UnpredictableNumberGenerator.GenerateUnpredictableNumber(nun);
        _Database.Update(unpredictableNumber);
    }

    #endregion

    #region S78.16 - S78.18

    /// <remarks>EMV Book C-2 Section S78.16 - S78.18</remarks>
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

        _EndpointClient.Send(ComputeCryptographicChecksumRequest.Create(sessionId, udol));

        return true;
    }

    #endregion

    #region S78.19 - S78.22

    /// <remarks>EMV Book C-2 Section S78.19 - S78.22</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleWaitingForCryptographicCheckSum2(TransactionSessionId sessionId)
    {
        if (IsCvmRequiredLimitExceeded())
            SetOfflinePinRequired();

        // S78.21
        UnpredictableNumberDataObjectList udol = _Database.Get<UnpredictableNumberDataObjectList>(UnpredictableNumberDataObjectList.Tag);

        // S78.22
        _EndpointClient.Send(ComputeCryptographicChecksumRequest.Create(sessionId, udol));
    }

    #endregion

    #region S78.19

    /// <remarks>EMV Book C-2 Section S78.19</remarks>
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

    /// <remarks>EMV Book C-2 Section S78.20</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetOfflinePinRequired()
    {
        _Database.SetOnDeviceCvmRequired(true);
        _Database.Update(CvmPerformedOutcome.ConfirmationCodeVerified);
    }

    #endregion

    #endregion
}