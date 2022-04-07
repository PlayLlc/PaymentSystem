using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time.Seconds;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForExchangeRelayResistanceDataResponse : KernelState
{
    #region Instance Members

    #region RAPDU

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // SR1.10
        Microseconds timeElapsed = session.Stopwatch.Stop();

        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryPersistingRapdu(session, (GetDataResponse) signal))
            return _KernelStateResolver.GetKernelState(StateId);

        MeasuredRelayResistanceProcessingTime processingTime = CalculateMeasuredRrpTime(timeElapsed);

        if (IsRelayOutOfLowerBounds(processingTime))
        {
            HandleRelayResistanceProtocolFailed(session, signal);

            return _KernelStateResolver.GetKernelState(StateId);
        }

        if (IsRelayRetryNeeded((Kernel2Session) session, processingTime))
            return RetryRelayResistanceProtocol((Kernel2Session) session);

        return CompleteRelayResistance((Kernel2Session) session, processingTime, signal);
    }

    #endregion

    #region SR1.3 - SR1.4, SR1.5.1 - SR1.5.2

    /// <remarks>Book C-2 Section SR1.3 - SR1.4, SR1.5.1 - SR1.5.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsLevel1ErrorPresent())
            return false;

        session.Stopwatch.Stop();

        _Database.Update(MessageIdentifiers.TryAgain);
        _Database.Update(Status.ReadyToRead);
        _Database.Update(new MessageHoldTime(0));
        _Database.Update(StatusOutcome.EndApplication);
        _Database.Update(StartOutcome.B);
        _Database.SetUiRequestOnRestartPresent(true);
        _Database.Update(signal.GetLevel1Error());
        _Database.Update(MessageOnErrorIdentifiers.TryAgain);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region SR1.11 - SR1.13

    /// <remarks>Book C-2 Section SR1.11 - SR1.13 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleInvalidResultCode(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Status.NotReady);
        _Database.Update(StatusOutcome.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.StatusBytes);
        _Database.Update(signal.GetStatusWords());
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region SR1.18

    /// <remarks>Book C-2 Section SR1.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <remarks>Book C-2 Section SR1.18 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private MeasuredRelayResistanceProcessingTime CalculateMeasuredRrpTime(Microseconds timeElapsed)
    {
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime =
            (TerminalExpectedTransmissionTimeForRelayResistanceCapdu) _Database.Get(TerminalExpectedTransmissionTimeForRelayResistanceCapdu
                                                                                        .Tag);

        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime =
            (TerminalExpectedTransmissionTimeForRelayResistanceRapdu) _Database.Get(TerminalExpectedTransmissionTimeForRelayResistanceRapdu
                                                                                        .Tag);

        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceExpectedRapduTransmissionTime =
            (DeviceEstimatedTransmissionTimeForRelayResistanceRapdu) _Database.Get(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu
                                                                                       .Tag);

        MeasuredRelayResistanceProcessingTime processingTime =
            MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissionTime,
                                                         terminalExpectedRapduTransmissionTime, deviceExpectedRapduTransmissionTime);

        _Database.Update(processingTime);

        return processingTime;
    }

    #endregion

    #region SR1.19

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <remarks>Book C-2 Section SR1.19 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsRelayOutOfLowerBounds(MeasuredRelayResistanceProcessingTime processingTime)
    {
        MinTimeForProcessingRelayResistanceApdu minTimeForProcessingRelayResistanceApdu =
            _Database.Get<MinTimeForProcessingRelayResistanceApdu>(MinTimeForProcessingRelayResistanceApdu.Tag);
        MinimumRelayResistanceGracePeriod minGracePeriod =
            _Database.Get<MinimumRelayResistanceGracePeriod>(MinimumRelayResistanceGracePeriod.Tag);

        RelaySeconds minRelayTime = (RelaySeconds) minTimeForProcessingRelayResistanceApdu - minGracePeriod;

        if (processingTime < (minRelayTime < RelaySeconds.Zero ? RelaySeconds.Zero : minRelayTime))
            return false;

        return true;
    }

    #endregion

    #region SR1.20 - SR1.21

    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section SR1.20 - SR1.21 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleRelayResistanceProtocolFailed(KernelSession session, QueryPcdResponse signal)
    {
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Status.NotReady);
        _Database.Update(StatusOutcome.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.CardDataError);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region SR1.22

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section SR1.22 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsRelayRetryNeeded(Kernel2Session session, MeasuredRelayResistanceProcessingTime relayTime)
    {
        if (session.GetRelayResistanceProtocolCount() > 2)
            return false;

        return IsRelayOutOfUpperBounds(relayTime);
    }

    #endregion

    #region SR1.23 - SR1.27

    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section SR1.23 - SR1.27 </remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private KernelState RetryRelayResistanceProtocol(Kernel2Session session)
    {
        // SR1.23
        UnpredictableNumber unpredictableNumber = _UnpredictableNumberGenerator.GenerateUnpredictableNumber();
        _Database.Update(unpredictableNumber);

        TerminalRelayResistanceEntropy entropy = new(unpredictableNumber);
        _Database.Update(entropy);

        // SR1.24
        session.IncrementRelayResistanceProtocolCount();

        // SR1.25
        ExchangeRelayResistanceDataRequest capdu = ExchangeRelayResistanceDataRequest.Create(session.GetTransactionSessionId(), entropy);

        // HACK: I  don't think we're supposed to set a timeout value for this. We're only viewing the time it takes. Which, I guess we can set the max expected time, but I don't think TimeoutValue is correct here
        TimeoutValue timeout = _Database.Get<TimeoutValue>(TimeoutValue.Tag);

        // SR1.26
        // BUG: We need to create a Timer in addition to the TimeoutManager we have
        session.Stopwatch.Start();

        _PcdEndpoint.Request(capdu);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion

    #region SR1.28 - SR1.32

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section SR1.28 - SR1.32 </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private KernelState CompleteRelayResistance(Kernel2Session session, MeasuredRelayResistanceProcessingTime relayTime, Message rapdu)
    {
        if (IsRelayOutOfUpperBounds(relayTime))
            SetRelayTimeLimitExceeded();

        if (IsAccuracyThresholdExceeded(relayTime))
            SetRelayResistanceThresholdExceeded();

        SetRelayResistancePerformed();

        return _KernelStateResolver.GetKernelState(_S3R1.Process(this, session, rapdu));
    }

    #endregion

    #region SR1.29

    /// <exception cref="TerminalDataException"></exception>
    private void SetRelayTimeLimitExceeded()
    {
        _Database.Set(TerminalVerificationResultCodes.RelayResistanceTimeLimitsExceeded);
    }

    #endregion

    #region SR1.30

    /// <remarks>Book C-2 Section SR1.30 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsAccuracyThresholdExceeded(MeasuredRelayResistanceProcessingTime processingTime)
    {
        if (!_Database.IsPresentAndNotEmpty(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag))
            return false;

        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceEstimate =
            _Database.Get<DeviceEstimatedTransmissionTimeForRelayResistanceRapdu>(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu
                                                                                      .Tag);
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalEstimate =
            _Database.Get<TerminalExpectedTransmissionTimeForRelayResistanceRapdu>(TerminalExpectedTransmissionTimeForRelayResistanceRapdu
                                                                                       .Tag);
        RelayResistanceTransmissionTimeMismatchThreshold mismatchThreshold =
            _Database.Get<RelayResistanceTransmissionTimeMismatchThreshold>(RelayResistanceTransmissionTimeMismatchThreshold.Tag);
        MinTimeForProcessingRelayResistanceApdu minThreshold =
            _Database.Get<MinTimeForProcessingRelayResistanceApdu>(MinTimeForProcessingRelayResistanceApdu.Tag);
        RelayResistanceAccuracyThreshold accuracyThreshold =
            _Database.Get<RelayResistanceAccuracyThreshold>(RelayResistanceAccuracyThreshold.Tag);

        // TODO: You need to double check this logic - You created the 'RelaySeconds' half way in between so double check you're using the * 100 correctly

        RelaySeconds minThresholdCheck = ((RelaySeconds) processingTime - minThreshold) < RelaySeconds.Zero
            ? RelaySeconds.Zero
            : (RelaySeconds) processingTime - minThreshold;

        RelaySeconds a = ((RelaySeconds) deviceEstimate * 100) / (RelaySeconds) terminalEstimate;

        if ((((RelaySeconds) deviceEstimate * 100) / (RelaySeconds) terminalEstimate) < (RelaySeconds) mismatchThreshold)
            return true;

        if ((((RelaySeconds) terminalEstimate * 100) / (RelaySeconds) deviceEstimate) < (RelaySeconds) mismatchThreshold)
            return true;

        if (minThresholdCheck > (RelaySeconds) accuracyThreshold)
            return true;

        return false;
    }

    #endregion

    #region SR1.31

    /// <remarks>Book C-2 Section SR1.31 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetRelayResistanceThresholdExceeded()
    {
        _Database.Set(TerminalVerificationResultCodes.RelayResistanceThresholdExceeded);
    }

    #endregion

    #region SR1.32

    /// <remarks>Book C-2 Section SR1.32 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetRelayResistancePerformed()
    {
        _Database.Set(TerminalVerificationResultCodes.RelayResistancePerformed);
    }

    #endregion

    #region Shared

    /// <summary>
    ///     IsRelayOutOfUpperBounds
    /// </summary>
    /// <param name="relayTime"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsRelayOutOfUpperBounds(MeasuredRelayResistanceProcessingTime relayTime)
    {
        MaxTimeForProcessingRelayResistanceApdu maxProcessingTime =
            _Database.Get<MaxTimeForProcessingRelayResistanceApdu>(MaxTimeForProcessingRelayResistanceApdu.Tag);

        MaximumRelayResistanceGracePeriod maxGraceTime =
            _Database.Get<MaximumRelayResistanceGracePeriod>(MaximumRelayResistanceGracePeriod.Tag);

        RelaySeconds maxRelayTime = (RelaySeconds) maxProcessingTime + maxGraceTime;

        if (relayTime > maxRelayTime)
            return true;

        return false;
    }

    #endregion

    #endregion

    #region SR1.14 - SR1.17

    /// <remarks>Book C-2 Section SR1.14 - SR1.17 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryPersistingRapdu(KernelSession session, GetDataResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        try
        {
            // BUG: I'm pretty sure we're supposed to discard any values that the card doesn't have. Look at the logic and fix this
            if (!signal.TryGetPrimitiveValue(out PrimitiveValue? getDataElement))
                throw new NotImplementedException();

            _Database.Update(getDataElement!);
            _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Logging

            HandleBerParsingException(session, signal);

            return false;
        }
        catch (Exception)
        {
            // TODO: Logging

            HandleBerParsingException(session, signal);

            return false;
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleBerParsingException(KernelSession session, QueryPcdResponse signal)
    {
        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Status.NotReady);
        _Database.Update(StatusOutcome.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.ParsingError);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion
}