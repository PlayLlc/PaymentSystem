using System;
using System.Threading;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForCccResponse2
{
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        ComputeCryptographicChecksumResponse rapdu = (ComputeCryptographicChecksumResponse) signal;

        // S13.1 - S13.5
        if (TryHandlingL1Error(session.GetKernelSessionId(), rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.9 - S13.10
        if (TryHandlingL1Error(session.GetKernelSessionId(), rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S14.11 - S14.13
        if (TryHandlingBerParsingException(session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S14.12.1
        SetDisplayMessage();

        //  S14.14 - S14.17
        if (TryHandlingMissingCardData(session))
            return _KernelStateResolver.GetKernelState(StateId);

        // S14.15, S14.19.1 - S14.23
        if (TryHandlingDoubleTapResponse(session))
            return _KernelStateResolver.GetKernelState(Idle.StateId);

        // S14.20 - S14.21.1
        if (TryHandlingInvalidOfflinePin(session))
            return _KernelStateResolver.GetKernelState(StateId);

        // S14.24 - S14.25
        NumberOfNonZeroBits nun = GetNumberOfNonZeroBits();

        // S14.25.1 - S14.27
        UpdateTrack2Data(nun);

        // S14.28 - S14.30
        UpdateTrack1Data(nun);

        // S14.32 - S14.34
        HandleOnlineResponse(session);

        return _KernelStateResolver.GetKernelState(Idle.StateId);
    }

    #region S14.1 - S14.5

    /// <remarks>Book C-2 Section S14.1 - S14.5</remarks>
    private bool TryHandlingL1Error(KernelSessionId sessionId, ComputeCryptographicChecksumResponse rapdu)
    {
        if (rapdu.IsLevel1ErrorPresent())
            return false;

        Sleep();

        // S13.3
        _Database.FailedMagstripeCounter.Increment();

        HandleL1Error(sessionId, rapdu);

        return true;
    }

    #endregion

    #region S14.2

    /// <remarks>Book C-2 Section S14.2</remarks>
    private void Sleep()
    {
        int exponent = _Database.FailedMagstripeCounter.GetSequenceValue();
        Milliseconds waitTime = new((uint) (Math.Pow(2, exponent) * 300));

        // HACK: This seems pretty hack-ish
        Thread.Sleep(waitTime);
    }

    #endregion

    #region S14.4 - S14.5

    /// <remarks>Book C-2 Section S14.4 - S14.5</remarks>
    private void HandleL1Error(KernelSessionId sessionId, QueryPcdResponse rapdu)
    {
        try
        {
            _Database.Update(MessageIdentifiers.TryAgain);
            _Database.Update(Statuses.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(StatusOutcomes.EndApplication);
            _Database.Update(StartOutcomes.B);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.Update(rapdu.GetLevel1Error());
            _Database.Update(MessageOnErrorIdentifiers.TryAgain);
            _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
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

    #region S14.9 - S14.10

    /// <remarks>Book C-2 Section S14.9 - S14.10</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingStatusBytesError(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsLevel2ErrorPresent())
            return false;

        _Database.Update(Level2Error.StatusBytes);

        HandleInvalidResponse(session);

        return true;
    }

    #endregion

    #region S14.11 - S14.13

    /// <remarks>Book C-2 Section S14.11 - S14.13</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingBerParsingException(KernelSession session, ComputeCryptographicChecksumResponse signal)
    {
        try
        {
            _Database.Update(signal.GetPrimitiveDataObjects());

            return false;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingException(session);

            return true;
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingException(session);

            return true;
        }
    }

    #endregion

    #region S14.13

    /// <remarks>Book C-2 Section S14.13</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleBerParsingException(KernelSession session)
    {
        _Database.Update(Level2Error.ParsingError);
        HandleInvalidResponse(session);
    }

    #endregion

    #region S14.12.1

    /// <remarks>Book C-2 Section S14.12.1</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetDisplayMessage()
    {
        _Database.Update(MessageIdentifiers.ClearDisplay);
        _Database.Update(Statuses.CardReadSuccessful);
        _Database.Update(MessageHoldTime.MinimumValue);
        _DisplayEndpoint.Request(new DisplayMessageRequest(_Database.GetUserInterfaceRequestData()));
    }

    #endregion

    #region S14.14 - S14.17

    /// <remarks>Book C-2 Section S14.14 - S14.17</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingMissingCardData(KernelSession session)
    {
        if (!_Database.IsPresentAndNotEmpty(ApplicationTransactionCounter.Tag))
        {
            HandleMissingCardData(session);

            return true;
        }

        if (!_Database.IsPresentAndNotEmpty(PosCardholderInteractionInformation.Tag))
        {
            HandleMissingCardData(session);

            return true;
        }

        if (!_Database.IsPresentAndNotEmpty(CardholderVerificationCode3Track2.Tag))
            return false;

        if (!_Database.IsPresentAndNotEmpty(Track1Data.Tag))
            return false;

        if (!_Database.IsPresentAndNotEmpty(CardholderVerificationCode3Track1.Tag))
        {
            HandleMissingCardData(session);

            return true;
        }

        return false;
    }

    #endregion

    #region S14.17

    /// <remarks>Book C-2 Section S14.17</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleMissingCardData(KernelSession session)
    {
        _Database.Update(Level2Error.CardDataMissing);
        HandleInvalidResponse(session);
    }

    #endregion

    #region S14.15, S14.19.1 - S14.23

    /// <remarks>Book C-2 Section S14.15, S14.19.1 - S14.23</remarks>
    private bool TryHandlingDoubleTapResponse(KernelSession session)
    {
        try
        {
            if (_Database.IsPresentAndNotEmpty(CardholderVerificationCode3Track2.Tag))
                return false;

            if (_Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag).IsSecondTapNeeded())
            {
                HandleDoubleTapResponse(session);

                return true;
            }

            HandleDeclinedResponse(session);

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            // HACK: This is in case there's an exception retrieving the OUT response from the database, but we should probably do something better here
            _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            // HACK: This is in case there's an exception retrieving the OUT response from the database, but we should probably do something better here
            _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
        }

        return false;
    }

    #endregion

    #region S14.19.2.1 - S14.19.3

    /// <remarks>Book C-2 Section S14.19.2.1 - S14.19.3</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleDeclinedResponse(KernelSession session)
    {
        Sleep();
        _Database.FailedMagstripeCounter.Increment();
        _Database.Update(MessageHoldTime.MinimumValue);
        _Database.Update(Statuses.ReadyToRead);
        _Database.SetUiRequestOnRestartPresent(true);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(StartOutcomes.B);
        _Database.SetIsDataRecordPresent(true);
        _Database.CreateMagstripeDataRecord(_DataExchangeKernelService);
        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion

    #region S14.20 - S14.21.1

    /// <remarks>Book C-2 Section S14.20 - S14.21.1</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingInvalidOfflinePin(KernelSession session)
    {
        PosCardholderInteractionInformation pcii = _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

        if (pcii.IsOfflineDeviceCvmVerificationSuccessful())
            return false;

        if (!IsCvmLimitExceeded())
            return false;

        HandleInvalidOfflinePin(session);

        return true;
    }

    #endregion

    #region S14.21

    /// <remarks>Book C-2 Section S14.21</remarks>
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

    #region S14.21.1

    /// <remarks>Book C-2 Section S14.21.1</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleInvalidOfflinePin(KernelSession session)
    {
        _Database.Update(Level2Error.CardDataError);
        HandleInvalidResponse(session);
    }

    #endregion

    #region S14.22 - S14.23

    /// <remarks>Book C-2 Section S14.22 - S14.23</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleDoubleTapResponse(KernelSession session)
    {
        DisplayPhoneMessage();
        Sleep();
        _Database.FailedMagstripeCounter.Increment();

        _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
        _Database.Update(MessageIdentifiers.Declined);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.Declined);
        _Database.SetIsDataRecordPresent(true);
        _Database.SetUiRequestOnOutcomePresent(true);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
        _Database.CreateMagstripeDataRecord(_DataExchangeKernelService);

        _Database.Update(StartOutcomes.B);
        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion

    #region S14.22

    /// <remarks>Book C-2 Section S14.22</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void DisplayPhoneMessage()
    {
        PhoneMessageTable phoneMessageTable = _Database.Get<PhoneMessageTable>(PhoneMessageTable.Tag);
        PosCardholderInteractionInformation pcii = _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

        if (!phoneMessageTable.TryGetMatch(pcii, out MessageTableEntry? messageTableEntry))
            return;

        _Database.Update(MessageIdentifiers.Get(messageTableEntry!.GetMessageIdentifier()));
        _Database.Update(messageTableEntry.GetStatus());
    }

    #endregion

    #region S14.24 - S14.25

    /// <remarks>Book C-2 Section S14.24 - S14.25</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private NumberOfNonZeroBits GetNumberOfNonZeroBits()
    {
        PosCardholderInteractionInformation pcii = _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);
        NumberOfNonZeroBits numberOfNonZeroBits = new(_Database.Get<PunatcTrack2>(PunatcTrack2.Tag),
            _Database.Get<NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2.Tag));

        if (pcii.IsOfflineDeviceCvmVerificationSuccessful())
            return numberOfNonZeroBits;

        return numberOfNonZeroBits.AsPlusFiveModuloTen();
    }

    #endregion

    #region S14.25.1 - S14.26

    /// <remarks>Book C-2 Section S14.25.1 - S14.26</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void UpdateTrack2Data(NumberOfNonZeroBits nun)
    {
        _Database.FailedMagstripeCounter.Reset();
        PositionOfCardVerificationCode3Track2 pcvc = _Database.Get<PositionOfCardVerificationCode3Track2>(PositionOfCardVerificationCode3Track2.Tag);
        NumericApplicationTransactionCounterTrack2 natc =
            _Database.Get<NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2.Tag);
        ApplicationTransactionCounter atc = _Database.Get<ApplicationTransactionCounter>(ApplicationTransactionCounter.Tag);
        PunatcTrack2 punatc = _Database.Get<PunatcTrack2>(PunatcTrack2.Tag);
        CardholderVerificationCode3Track2 cvc = _Database.Get<CardholderVerificationCode3Track2>(CardholderVerificationCode3Track2.Tag);
        UnpredictableNumber unpredictableNumber = _Database.Get<UnpredictableNumber>(UnpredictableNumber.Tag);
        Track2Data track2 = _Database.Get<Track2Data>(Track2Data.Tag);

        _Database.Update(track2.UpdateDiscretionaryData(nun, cvc, pcvc, punatc, unpredictableNumber, natc, atc));
    }

    #endregion

    #region S14.28 - S14.30

    /// <remarks>Book C-2 Section S14.28 - S14.30</remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void UpdateTrack1Data(NumberOfNonZeroBits nun)
    {
        if (_Database.TryGet(Track1Data.Tag, out Track1Data? track1))
            return;

        PositionOfCardVerificationCode3Track1 pcvc = _Database.Get<PositionOfCardVerificationCode3Track1>(PositionOfCardVerificationCode3Track1.Tag);
        NumericApplicationTransactionCounterTrack1 natc =
            _Database.Get<NumericApplicationTransactionCounterTrack1>(NumericApplicationTransactionCounterTrack1.Tag);
        ApplicationTransactionCounter atc = _Database.Get<ApplicationTransactionCounter>(ApplicationTransactionCounter.Tag);
        PunatcTrack1 punatc = _Database.Get<PunatcTrack1>(PunatcTrack1.Tag);
        CardholderVerificationCode3Track1 cvc = _Database.Get<CardholderVerificationCode3Track1>(CardholderVerificationCode3Track1.Tag);
        UnpredictableNumberNumeric unpredictableNumber = _Database.Get<UnpredictableNumberNumeric>(UnpredictableNumberNumeric.Tag);

        _Database.Update(track1!.UpdateDiscretionaryData(nun, cvc, pcvc, punatc, unpredictableNumber, natc, atc));
    }

    #endregion

    #region S14.32 - S14.34

    /// <remarks>Book C-2 Section S14.32 - S14.34</remarks>
    private void HandleOnlineResponse(KernelSession session)
    {
        try
        {
            if (IsCvmLimitExceeded())
            {
                HandleOnlineCvmRequiredResponse(session);

                return;
            }

            HandleOnlineNoCvmRequiredResponse(session);
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            // HACK: This is in case there's an exception retrieving the OUT response from the database, but we should probably do something better here
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            // HACK: This is in case there's an exception retrieving the OUT response from the database, but we should probably do something better here
        }
        finally
        {
            _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
        }
    }

    #endregion

    #region S14.33

    /// <remarks>Book C-2 Section S14.33</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleOnlineNoCvmRequiredResponse(KernelSession session)
    {
        _Database.Update(StatusOutcomes.OnlineRequest);
        _Database.Update(CvmPerformedOutcome.NoCvm);
        _Database.SetIsDataRecordPresent(true);
        _Database.CreateMagstripeDataRecord(_DataExchangeKernelService);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion

    #region S14.34

    /// <remarks>Book C-2 Section S14.34</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleOnlineCvmRequiredResponse(KernelSession session)
    {
        _Database.Update(StatusOutcomes.OnlineRequest);
        _Database.Update(CvmPerformedOutcome.ConfirmationCodeVerified);

        if (IsCvmLimitExceeded())
            _Database.SetIsReceiptPresent(true);
        _Database.SetIsDataRecordPresent(true);
        _Database.CreateMagstripeDataRecord(_DataExchangeKernelService);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion

    #region S14.40 - S14.43

    /// <remarks>Book C-2 Section S14.40 - S14.43</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleInvalidResponse(KernelSession session)
    {
        Sleep();

        _Database.FailedMagstripeCounter.Increment();

        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(StartOutcomes.B);
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnOutcomePresent(true);
        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion
}