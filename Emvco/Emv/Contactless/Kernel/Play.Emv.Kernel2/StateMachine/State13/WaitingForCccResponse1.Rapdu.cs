using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForCccResponse1
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        ComputeCryptographicChecksumResponse rapdu = (ComputeCryptographicChecksumResponse) signal;

        // S13.1 - S13.5
        if (TryHandlingL1Error(session.GetKernelSessionId(), signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.9 - S13.10
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

        // S13.14.2 -  S13.14.4
        if (TryHandlingDoubleTapResponse())
            return _KernelStateResolver.GetKernelState(Idle.StateId);

        // S13.14.5 - S13.14.8
        NumberOfNonZeroBits nun = CalculateNun();

        // S13.15 - S13.16
        if (TryHandlingMissingCvc3Track1Data(session))
            return _KernelStateResolver.GetKernelState(StateId);

        // S13.42.1 - S13.43 
        return _KernelStateResolver.GetKernelState(Idle.StateId);
    }

    #region S13.1 - S13.5

    /// <remarks>Book C-2 Section S13.1 - S13.5</remarks>
    private bool TryHandlingL1Error(KernelSessionId sessionId, QueryPcdResponse rapdu)
    {
        if (@rapdu.IsLevel1ErrorPresent())
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
            _Database.Update(Status.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
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

    #region S13.9 - S13.10

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
        _Database.Update(Status.CardReadSuccessful);
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

    #region S13.14.2 -  S13.14.4

    /// <remarks>Book C-2 Section S13.14.2 -  S13.14.4</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingDoubleTapResponse()
    {
        if (_Database.IsPresentAndNotEmpty(CardholderVerificationCode3Track2.Tag))
            return false;

        if (!_Database.IsPresentAndNotEmpty(PosCardholderInteractionInformation.Tag))
            return false;

        if (_Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag).IsSecondTapNeeded())
        {
            HandleDoubleTapResponse();

            return true;
        }

        HandleDeclinedResponse();

        return true;
    }

    #endregion

    #region S13.14.5 - S13.14.8

    /// <exception cref="TerminalDataException"></exception>
    private NumberOfNonZeroBits CalculateNun()
    {
        NumberOfNonZeroBits nun = new(_Database.Get<PunatcTrack2>(PunatcTrack2.Tag),
            _Database.Get<NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2.Tag));

        if (_Database.TryGet(PosCardholderInteractionInformation.Tag, out PosCardholderInteractionInformation? pcii))
            return nun;

        return !pcii!.IsOnDeviceCvmVerificationSuccessful() ? nun : nun.AsPlusFiveModuloTen();
    }

    #endregion

    #region S13.15 - S13.16

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

    #region ___Update Discretionary Data in Track 1 & 2

    /// <exception cref="TerminalDataException"></exception>
    private void UpdateTrackData()
    {
        _Database.FailedMagstripeCounter.Reset();
        NumberOfNonZeroBits q = new(_Database.Get<PositionOfCardVerificationCode3Track2>(PositionOfCardVerificationCode3Track2.Tag));
        NumericApplicationTransactionCounterTrack2 t =
            _Database.Get<NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2.Tag);
        CardholderVerificationCode3Track2 cvc = _Database.Get<CardholderVerificationCode3Track2>(CardholderVerificationCode3Track2.Tag);
        Track2Data track2 = _Database.Get<Track2Data>(Track2Data.Tag);
        ushort cvcBase10 = PlayCodec.NumericCodec.DecodeToUInt16(t.EncodeValue());

        /*
         * 1) Convert the binary encoded CVC3 (Track2) to the BCD encoding of the corresponding number expressed in base 10.
         *
         * numberOfDigitsToCopy = (byte)q;
         * digitsToCopy = cvcBase10.ToNibbleArray()[^numberOfDigitsToCopy..];
         *
         * Copy the rightmost digits of cvcBase10 to Discretionary Data - the position of the bits to copy dictated by set bits in PCVC3
         *
         * 2) Copy the q least significant digits of the BCD encoded CVC3 (Track2) in the eligible positions of the
         * 'Discretionary Data' in Track 2 Data. The eligible positions are indicated by the q non-zero bits in PCVC3(Track2). 
         *
         * 3) Replace the nUN least significant eligible positions of the 'Discretionary Data' in Track 2 Data by the nUN least
         * significant digits of Unpredictable Number (Numeric). The eligible positions in the 'Discretionary Data' in Track 2
         * Data are indicated by the nUN least significant non-zero bits in PUNATC(Track2).
         *
         * 4) If t ≠ 0, convert the Application Transaction Counter to the BCD encoding of the corresponding number expressed
         * in base 10.
         *
         * 5) Replace the t most significant eligible positions of the 'Discretionary Data' in Track 2 Data by the t least
         * significant digits of the BCD encoded Application Transaction Counter. The eligible positions in the
         * 'Discretionary Data' in Track 2 Data are indicated by the t most significant non-zero bits in PUNATC(Track2).
         */
    }

    #endregion

    #region ___Online OUT Responses and Stuff

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

        try
        {
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Status.NotReady);
            _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.SetUiRequestOnOutcomePresent(true);
            _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
            _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetOutcome()));
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
            // HACK: This is incase there's an exception retrieving the OUT response from the database, but we should probably do something better here
            _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
        }
    }

    #endregion

    #region S13.41, S13.44 - S13.45

    /// <remarks>Book C-2 Section S13.41, S13.44 - S13.45</remarks>
    private void HandleDoubleTapResponse()
    { }

    #endregion

    #region S13.42.1 - S13.43

    /// <remarks>Book C-2 Section S13.42.1 - S13.43</remarks>
    private void HandleDeclinedResponse()
    { }

    #endregion
}