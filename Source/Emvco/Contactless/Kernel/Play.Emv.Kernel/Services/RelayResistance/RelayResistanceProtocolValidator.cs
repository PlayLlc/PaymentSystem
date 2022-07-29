using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Globalization.Time;

namespace Play.Emv.Kernel.Services;

public class RelayResistanceProtocolValidator : IValidateRelayResistanceProtocol
{
    #region Instance Values

    private readonly TransactionSessionId _SessionId;
    private readonly int _MaximumRetryCount;

    #endregion

    #region Constructor

    public RelayResistanceProtocolValidator(TransactionSessionId sessionId, int maximumRetryCount)
    {
        _SessionId = sessionId;
        _MaximumRetryCount = maximumRetryCount; //usually 2
    }

    #endregion

    #region Instance Members

    //The RelayResistanceProtocolCount is part of the KernelSession.
    public bool IsRetryThresholdHit(int retryCount) => retryCount > _MaximumRetryCount;

    /// <summary>
    ///     IsInRange
    /// </summary>
    /// <param name="transactionSessionId"></param>
    /// <param name="timeElapsed"></param>
    /// <param name="tlvDatabase"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsInRange(TransactionSessionId transactionSessionId, Microseconds timeElapsed, IReadTlvDatabase tlvDatabase)
    {
        if (transactionSessionId != _SessionId)
        {
            throw new TerminalDataException(
                $"The {nameof(RelayResistanceProtocolValidator)} received a request with an invalid {nameof(TransactionSessionId)}. The expected {nameof(TransactionSessionId)} was: [{_SessionId}], but the value received was: [{transactionSessionId}]");
        }

        MeasuredRelayResistanceProcessingTime processingTime = CalculateMeasuredRrpTime(timeElapsed, tlvDatabase);

        if (!IsRelayResistanceWithinMinimumRange(processingTime, tlvDatabase))
            return false;

        if (!IsRelayResistanceWithinMaximumRange(processingTime, tlvDatabase))
            return false;

        return true;
    }

    /// <summary>
    ///     CalculateMeasuredRrpTime Book C-2 SR1.18
    /// </summary>
    /// <param name="timeElapsed"></param>
    /// <param name="tlvDatabase"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public MeasuredRelayResistanceProcessingTime CalculateMeasuredRrpTime(Microseconds timeElapsed, IReadTlvDatabase tlvDatabase)
    {
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime =
            (TerminalExpectedTransmissionTimeForRelayResistanceCapdu) tlvDatabase.Get(TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag);

        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime =
            (TerminalExpectedTransmissionTimeForRelayResistanceRapdu) tlvDatabase.Get(TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag);

        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceExpectedRapduTransmissionTime =
            (DeviceEstimatedTransmissionTimeForRelayResistanceRapdu) tlvDatabase.Get(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag);

        MeasuredRelayResistanceProcessingTime processingTime = MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissionTime,
            terminalExpectedRapduTransmissionTime, deviceExpectedRapduTransmissionTime);

        return processingTime;
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsRelayResistanceWithinMinimumRange(MeasuredRelayResistanceProcessingTime processingTime, IReadTlvDatabase tlvDatabase)
    {
        MinTimeForProcessingRelayResistanceApdu minTimeForProcessingRelayResistanceApdu =
            (MinTimeForProcessingRelayResistanceApdu) tlvDatabase.Get(MinTimeForProcessingRelayResistanceApdu.Tag);
        MinimumRelayResistanceGracePeriod minGracePeriod = (MinimumRelayResistanceGracePeriod) tlvDatabase.Get(MinimumRelayResistanceGracePeriod.Tag);

        RelaySeconds expectedProcessingTime = (RelaySeconds) minTimeForProcessingRelayResistanceApdu - (RelaySeconds) minGracePeriod;

        if ((RelaySeconds) processingTime < (expectedProcessingTime < RelaySeconds.Zero ? 0 : expectedProcessingTime))
            return false;

        return true;
    }

    public bool IsRelayResistanceWithinMaximumRange(MeasuredRelayResistanceProcessingTime processingTime, IReadTlvDatabase tlvDatabase)
    {
        MaxTimeForProcessingRelayResistanceApdu maxTimeForProcessingRelayResistanceApdu =
            (MaxTimeForProcessingRelayResistanceApdu)tlvDatabase.Get(MaxTimeForProcessingRelayResistanceApdu.Tag);

        MaximumRelayResistanceGracePeriod maxGracePeriod = (MaximumRelayResistanceGracePeriod)tlvDatabase.Get(MaximumRelayResistanceGracePeriod.Tag);

        RelaySeconds expectedProcessingTime = (RelaySeconds)maxTimeForProcessingRelayResistanceApdu + (RelaySeconds)maxGracePeriod;

        if ((RelaySeconds)processingTime > expectedProcessingTime)
            return false;

        return true;
    }

    #endregion
}