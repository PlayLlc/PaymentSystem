using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv.ValueTypes;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Icc.GetData;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Contracts.SignalIn.Quereies;
using Play.Emv.Pcd.Contracts.SignalOut.Queddries;
using Play.Globalization.Time;
using Play.Globalization.Time.Seconds;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForExchangeRelayResistanceDataResponse : KernelState
{
    #region RAPDU

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // SR1.10
        Microseconds timeElapsed = session.StopTimeout();

        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryPersistingRapdu(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        MeasuredRelayResistanceProcessingTime processingTime = CalculateMeasuredRrpTime(timeElapsed);

        if (IsRelayOutOfLowerBounds(processingTime))
        {
            HandleRelayResistanceProtocolFailed(session, signal);

            return _KernelStateResolver.GetKernelState(StateId);
        }

        if (IsRelayRetryNeeded((Kernel2Session) session, processingTime))
            return RetryRelayResistanceProtocol((Kernel2Session) session);

        return CompleteRelayResistance((Kernel2Session) session, processingTime);
    }

    #endregion

    #region SR1.3 - SR1.4, SR1.5.1 - SR1.5.2

    /// <remarks>Book C-2 Section SR1.3 - SR1.4, SR1.5.1 - SR1.5.2</remarks>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsSuccessful())
            return false;

        session.StopTimeout();

        _KernelDatabase.Update(MessageIdentifier.TryAgain);
        _KernelDatabase.Update(Status.ReadyToRead);
        _KernelDatabase.Update(new MessageHoldTime(0));
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(StartOutcome.B);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelDatabase.Update(signal.GetLevel1Error());
        _KernelDatabase.Update(MessageOnErrorIdentifier.TryAgain);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region SR1.11 - SR1.13

    /// <remarks>Book C-2 Section SR1.11 - SR1.13 </remarks>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    private bool TryHandleInvalidResultCode(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.StatusBytes);
        _KernelDatabase.Update(signal.GetStatusWords());
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region SR1.14 - SR1.17

    /// <remarks>Book C-2 Section SR1.14 - SR1.17 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryPersistingRapdu(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        try
        {
            _KernelDatabase.Update(((GetDataResponse) signal).GetTagLengthValueResult());
            _DataExchangeKernelService.Resolve((GetDataResponse) signal);

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
    private void HandleBerParsingException(KernelSession session, QueryPcdResponse signal)
    {
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.ParsingError);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region SR1.18

    /// <remarks>Book C-2 Section SR1.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <remarks>Book C-2 Section SR1.18 </remarks>
    private MeasuredRelayResistanceProcessingTime CalculateMeasuredRrpTime(Microseconds timeElapsed)
    {
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime =
            TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode(_KernelDatabase
                                                                               .Get(TerminalExpectedTransmissionTimeForRelayResistanceCapdu
                                                                                        .Tag).EncodeValue().AsSpan());
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime =
            TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode(_KernelDatabase
                                                                               .Get(TerminalExpectedTransmissionTimeForRelayResistanceRapdu
                                                                                        .Tag).EncodeValue().AsSpan());
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceExpectedRapduTransmissionTime =
            DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(_KernelDatabase
                                                                              .Get(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu
                                                                                       .Tag).EncodeValue().AsSpan());

        MeasuredRelayResistanceProcessingTime processingTime =
            MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissionTime,
                                                         terminalExpectedRapduTransmissionTime, deviceExpectedRapduTransmissionTime);

        _KernelDatabase.Update(processingTime);

        return processingTime;
    }

    #endregion

    #region SR1.19

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <remarks>Book C-2 Section SR1.19 </remarks>
    public bool IsRelayOutOfLowerBounds(MeasuredRelayResistanceProcessingTime processingTime)
    {
        MinTimeForProcessingRelayResistanceApdu minTimeForProcessingRelayResistanceApdu =
            MinTimeForProcessingRelayResistanceApdu.Decode(_KernelDatabase.Get(MinTimeForProcessingRelayResistanceApdu.Tag).EncodeValue()
                                                               .AsSpan());
        MinimumRelayResistanceGracePeriod minGracePeriod =
            MinimumRelayResistanceGracePeriod.Decode(_KernelDatabase.Get(MinimumRelayResistanceGracePeriod.Tag).EncodeValue().AsSpan());

        RelaySeconds minRelayTime = (RelaySeconds) minTimeForProcessingRelayResistanceApdu - minGracePeriod;

        if (processingTime < (minRelayTime < RelaySeconds.Zero ? RelaySeconds.Zero : minRelayTime))
            return false;

        return true;
    }

    #endregion

    #region SR1.20 - SR1.21

    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section SR1.20 - SR1.21 </remarks>
    private void HandleRelayResistanceProtocolFailed(KernelSession session, QueryPcdResponse signal)
    {
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.CardDataError);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region SR1.22

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section SR1.22 </remarks>
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
    private KernelState RetryRelayResistanceProtocol(Kernel2Session session)
    {
        // SR1.23
        UnpredictableNumber unpredictableNumber = _UnpredictableNumberGenerator.GenerateUnpredictableNumber();
        _KernelDatabase.Update(unpredictableNumber);

        TerminalRelayResistanceEntropy entropy = new(unpredictableNumber);
        _KernelDatabase.Update(entropy);

        // SR1.24
        session.IncrementRelayResistanceProtocolCount();

        // SR1.25
        ExchangeRelayResistanceDataRequest capdu = ExchangeRelayResistanceDataRequest.Create(session.GetTransactionSessionId(), entropy);

        // HACK: I  don't think we're supposed to set a timeout value for this. We're only viewing the time it takes. Which, I guess we can set the max expected time, but I don't think TimeoutValue is correct here
        TimeoutValue timeout = TimeoutValue.Decode(_KernelDatabase.Get(TimeoutValue.Tag).GetValue().AsSpan());

        // SR1.26
        // BUG: We need to create a Timer in addition to the TimeoutManager we have
        session.StartTimeout(new Milliseconds(long.MaxValue));

        _PcdEndpoint.Request(capdu);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion

    #region SR1.28 - SR1.32

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section SR1.28 - SR1.32 </remarks>
    private KernelState CompleteRelayResistance(Kernel2Session session, MeasuredRelayResistanceProcessingTime relayTime)
    {
        if (IsRelayOutOfUpperBounds(relayTime))
            SetRelayTimeLimitExceeded();

        if (IsAccuracyThresholdExceeded(relayTime))
            SetRelayResistanceThresholdExceeded();

        SetRelayResistancePerformed();

        return _S3R1.Process(this, session);
    }

    #endregion

    #region SR1.29

    /// <exception cref="TerminalDataException"></exception>
    public void SetRelayTimeLimitExceeded()
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.RelayResistanceTimeLimitsExceeded);
    }

    #endregion

    #region SR1.30

    /// <remarks>Book C-2 Section SR1.30 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public bool IsAccuracyThresholdExceeded(MeasuredRelayResistanceProcessingTime processingTime)
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag))
            return false;
        if (!_KernelDatabase.IsPresentAndNotEmpty(TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag))
            return false;

        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceEstimate =
            DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(_KernelDatabase
                                                                              .Get(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu
                                                                                       .Tag).EncodeValue().AsSpan());
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalEstimate =
            TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode(_KernelDatabase
                                                                               .Get(TerminalExpectedTransmissionTimeForRelayResistanceRapdu
                                                                                        .Tag).EncodeValue().AsSpan());
        RelayResistanceTransmissionTimeMismatchThreshold mismatchThreshold =
            RelayResistanceTransmissionTimeMismatchThreshold.Decode(_KernelDatabase
                                                                        .Get(RelayResistanceTransmissionTimeMismatchThreshold.Tag)
                                                                        .EncodeValue().AsSpan());
        MinTimeForProcessingRelayResistanceApdu minThreshold =
            MinTimeForProcessingRelayResistanceApdu.Decode(_KernelDatabase.Get(MinTimeForProcessingRelayResistanceApdu.Tag).EncodeValue()
                                                               .AsSpan());
        RelayResistanceAccuracyThreshold accuracyThreshold =
            RelayResistanceAccuracyThreshold.Decode(_KernelDatabase.Get(RelayResistanceAccuracyThreshold.Tag).EncodeValue().AsSpan());
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
    public void SetRelayResistanceThresholdExceeded()
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.RelayResistanceThresholdExceeded);
    }

    #endregion

    #region SR1.32

    /// <remarks>Book C-2 Section SR1.32 </remarks>
    public void SetRelayResistancePerformed()
    {
        _KernelDatabase.Set(TerminalVerificationResultCodes.RelayResistancePerformed);
    }

    #endregion

    #region Shared

    private bool IsRelayOutOfUpperBounds(MeasuredRelayResistanceProcessingTime relayTime)
    {
        MaxTimeForProcessingRelayResistanceApdu maxProcessingTime =
            MaxTimeForProcessingRelayResistanceApdu.Decode(_KernelDatabase.Get(MaxTimeForProcessingRelayResistanceApdu.Tag).EncodeValue()
                                                               .AsSpan());
        MaximumRelayResistanceGracePeriod maxGraceTime =
            MaximumRelayResistanceGracePeriod.Decode(_KernelDatabase.Get(MaximumRelayResistanceGracePeriod.Tag).EncodeValue().AsSpan());

        RelaySeconds maxRelayTime = (RelaySeconds) maxProcessingTime + maxGraceTime;

        if (relayTime > maxRelayTime)
            return true;

        return false;
    }

    #endregion
}