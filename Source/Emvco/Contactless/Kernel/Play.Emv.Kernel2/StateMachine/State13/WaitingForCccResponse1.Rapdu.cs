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

public partial class WaitingForCccResponse1
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        ComputeCryptographicChecksumResponse rapdu = (ComputeCryptographicChecksumResponse) signal;

        // S13.1 - S13.5
        if (TryHandlingL1Error(session.GetKernelSessionId(), signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.6, S13.9 - S13.10
        if (TryHandlingStatusBytesError(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.11 - S13.13
        if (TryHandlingBerParsingException(session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.12.1
        SetDisplayMessage();

        // S13.14.1 - S13.14.4
        if (TryHandlingMissingCardData(session))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.14.2 -  S13.14.3, S13.41 - S13.45
        if (TryHandlingDoubleTapResponse(session))
            return _KernelStateResolver.GetKernelState(Idle.StateId);

        // S13.14.5 - S13.14.8
        NumberOfNonZeroBits nun = CalculateNun();

        // S13.15 - S13.16
        if (TryHandlingMissingCvc3Track1Data(session))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.17 - S13.19
        UpdateTrack2Data(nun);

        // S13.20 - S13.22
        UpdateTrack1Data(nun);

        // S13.24 - S13.26
        HandleOnlineResponse(session);

        // S13.42.1 - S13.43 
        return _KernelStateResolver.GetKernelState(Idle.StateId);
    }

    #region S13.1 - S13.5

    /// <remarks>Book C-2 Section S13.1 - S13.5</remarks>
    private bool TryHandlingL1Error(KernelSessionId sessionId, QueryPcdResponse rapdu)
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

    #region S13.2, S13.30

    /// <remarks>Book C-2 Section S13.2, S13.30</remarks>
    private void Sleep()
    {
        int exponent = _Database.FailedMagstripeCounter.GetSequenceValue();
        Milliseconds waitTime = new((uint) (Math.Pow(2, exponent) * 300));

        // HACK: This seems pretty hack-ish
        Thread.Sleep(waitTime);
    }

    #endregion

    #region S13.4 - S13.5

    /// <remarks>Book C-2 Section S13.4 - S13.5</remarks>
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

    #region S13.6, S13.9 - S13.10

    /// <remarks>Book C-2 Section S13.9 - S13.10</remarks>
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

    #region S13.11 - S13.13

    /// <remarks>Book C-2 Section S13.11 - S13.13</remarks>
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

    #region S13.12.1

    /// <remarks>Book C-2 Section S13.12.1</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetDisplayMessage()
    {
        _Database.Update(MessageIdentifiers.ClearDisplay);
        _Database.Update(Statuses.CardReadSuccessful);
        _Database.Update(MessageHoldTime.MinimumValue);
        _DisplayEndpoint.Request(new DisplayMessageRequest(_Database.GetUserInterfaceRequestData()));
    }

    #endregion

    #region S13.13

    /// <remarks>Book C-2 Section S13.13</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleBerParsingException(KernelSession session)
    {
        _Database.Update(Level2Error.ParsingError);
        HandleInvalidResponse(session);
    }

    #endregion

    #region S13.14.1 - S13.14.4

    /// <remarks>Book C-2 Section S13.14.1 - S13.14.4</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingMissingCardData(KernelSession session)
    {
        if (!_Database.IsPresentAndNotEmpty(ApplicationTransactionCounter.Tag))
        {
            HandleMissingCardData(session);

            return true;
        }

        if (!_Database.IsPresentAndNotEmpty(CardholderVerificationCode3Track2.Tag))
            return false;

        if (!_Database.IsPresentAndNotEmpty(PosCardholderInteractionInformation.Tag))
        {
            HandleMissingCardData(session);

            return true;
        }

        return false;
    }

    #endregion

    #region S13.14.4

    /// <remarks>Book C-2 Section S13.14.4</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleMissingCardData(KernelSession session)
    {
        _Database.Update(Level2Error.CardDataMissing);
        HandleInvalidResponse(session);
    }

    #endregion

    #region S13.14.2 -  S13.14.3, S13.41 - S13.45

    /// <remarks>Book C-2 Section S13.14.2 -  S13.14.4</remarks>
    private bool TryHandlingDoubleTapResponse(KernelSession session)
    {
        try
        {
            if (_Database.IsPresentAndNotEmpty(CardholderVerificationCode3Track2.Tag))
                return false;

            if (!_Database.IsPresentAndNotEmpty(PosCardholderInteractionInformation.Tag))
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

    #region S13.14.5 - S13.14.8

    /// <remarks>Book C-2 Section S13.14.5 - S13.14.8</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private NumberOfNonZeroBits CalculateNun()
    {
        NumberOfNonZeroBits nun = new(_Database.Get<PunatcTrack2>(PunatcTrack2.Tag),
            _Database.Get<NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2.Tag));

        if (_Database.TryGet(PosCardholderInteractionInformation.Tag, out PosCardholderInteractionInformation? pcii))
            return nun;

        return !pcii!.IsOfflineDeviceCvmVerificationSuccessful() ? nun : nun.AsPlusFiveModuloTen();
    }

    #endregion

    #region S13.15 - S13.16

    /// <remarks>Book C-2 Section S13.15 - S13.16</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingMissingCvc3Track1Data(KernelSession session)
    {
        if (!_Database.IsPresentAndNotEmpty(Track1Data.Tag))
            return false;

        if (_Database.IsPresentAndNotEmpty(CardholderVerificationCode3Track1.Tag))
            return false;

        _Database.Update(Level2Error.CardDataMissing);

        HandleInvalidResponse(session);

        return true;
    }

    #endregion

    #region S13.17 - S13.19

    /// <remarks>Book C-2 Section S13.17 - S13.19</remarks>
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

    #region S13.20 - S13.22

    /// <remarks>Book C-2 Section S13.20 - S13.22</remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
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

    #region S13.24 - S13.26

    /// <remarks>Book C-2 Section S13.24 - S13.26</remarks>
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

    #region S13.24 - S13.26

    /// <remarks>Book C-2 Section S13.24 - S13.26</remarks>
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

    #region S13.25

    /// <remarks>Book C-2 Section S13.25</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleOnlineNoCvmRequiredResponse(KernelSession session)
    {
        _Database.Update(StatusOutcomes.OnlineRequest);
        MagstripeCvmCapabilityNoCvmRequired cvmCapability = _Database.Get<MagstripeCvmCapabilityNoCvmRequired>(MagstripeCvmCapabilityNoCvmRequired.Tag);

        if ((byte) cvmCapability! == CvmCodes.SignaturePaper)
            _Database.SetIsReceiptPresent(true);

        _Database.Update(CvmPerformedOutcome.Get((byte) cvmCapability));

        _Database.SetIsDataRecordPresent(true);
        _Database.CreateMagstripeDataRecord(_DataExchangeKernelService);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion

    #region S13.26

    /// <remarks>Book C-2 Section S13.26</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleOnlineCvmRequiredResponse(KernelSession session)
    {
        _Database.Update(StatusOutcomes.OnlineRequest);
        MagstripeCvmCapabilityCvmRequired cvmCapability = _Database.Get<MagstripeCvmCapabilityCvmRequired>(MagstripeCvmCapabilityCvmRequired.Tag);

        _Database.Update(CvmPerformedOutcome.Get((byte) cvmCapability));
        _Database.SetIsReceiptPresent(true);
        _Database.SetIsDataRecordPresent(true);
        _Database.CreateMagstripeDataRecord(_DataExchangeKernelService);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion

    #region S13.30 - S13.33

    /// <remarks>Book C-2 Section S13.30 - S13.33</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleInvalidResponse(KernelSession session)
    {
        // S13.30
        Sleep();

        // S13.31
        _Database.FailedMagstripeCounter.Increment();

        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(StartOutcomes.B);
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.SetUiRequestOnOutcomePresent(true);
        _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
    }

    #endregion

    #region S13.42.1 - S13.43

    /// <remarks>Book C-2 Section S13.42.1 - S13.43</remarks>
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

    #region 13.44

    /// <remarks>Book C-2 Section 13.44</remarks>
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

    #region S13.44 - S13.45

    /// <remarks>Book C-2 Section S13.41, S13.44 - S13.45</remarks>
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
}