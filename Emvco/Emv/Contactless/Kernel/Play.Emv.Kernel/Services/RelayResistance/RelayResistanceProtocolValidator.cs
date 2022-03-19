using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Kernel.Services;

internal class RelayResistanceProtocolValidator
{
    #region Instance Values

    private readonly TransactionSessionId _SessionId;
    private readonly int _MaximumRetryCount;
    private int _RetryCount;

    #endregion

    #region Constructor

    public RelayResistanceProtocolValidator(TransactionSessionId sessionId, int maximumRetryCount)
    {
        _SessionId = sessionId;
        _MaximumRetryCount = maximumRetryCount;
    }

    #endregion

    #region Instance Members

    public bool IsRetryThresholdHit() => ++_RetryCount > _MaximumRetryCount;

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
    private bool IsInRange(TransactionSessionId transactionSessionId, Milliseconds timeElapsed, IQueryTlvDatabase tlvDatabase)
    {
        if (transactionSessionId != _SessionId)
        {
            throw new
                TerminalDataException($"The {nameof(RelayResistanceProtocolValidator)} recieved a request with an invalid {nameof(TransactionSessionId)}. The expected {nameof(TransactionSessionId)} was: [{_SessionId}], but the value received was: [{transactionSessionId}]");
        }

        MeasuredRelayResistanceProcessingTime processingTime = CalculateMeasuredRrpTime(timeElapsed, tlvDatabase);

        if (IsRelayResistanceWithinMinimumRange(processingTime, tlvDatabase))
            return false;

        if (IsRelayResistanceWithinMaximumRange())
            return false;

        return true;
    }

    /// <summary>
    ///     CalculateMeasuredRrpTime
    /// </summary>
    /// <param name="timeElapsed"></param>
    /// <param name="tlvDatabase"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
      private MeasuredRelayResistanceProcessingTime CalculateMeasuredRrpTime(Seconds timeElapsed, IQueryTlvDatabase tlvDatabase)
    {
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime =
            TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode(tlvDatabase
                                                                               .Get(TerminalExpectedTransmissionTimeForRelayResistanceCapdu
                                                                                        .Tag).EncodeValue().AsSpan());
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime =
            TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode(tlvDatabase
                                                                               .Get(TerminalExpectedTransmissionTimeForRelayResistanceRapdu
                                                                                        .Tag).EncodeValue().AsSpan());
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceExpectedRapduTransmissionTime =
            DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(tlvDatabase
                                                                              .Get(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu
                                                                                       .Tag).EncodeValue().AsSpan());

        MeasuredRelayResistanceProcessingTime processingTime =
            MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissionTime,
                                                         terminalExpectedRapduTransmissionTime, deviceExpectedRapduTransmissionTime);

        return processingTime;
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsRelayResistanceWithinMinimumRange(MeasuredRelayResistanceProcessingTime processingTime, IQueryTlvDatabase tlvDatabase)
    {
        MinTimeForProcessingRelayResistanceApdu minTimeForProcessingRelayResistanceApdu =
            MinTimeForProcessingRelayResistanceApdu.Decode(tlvDatabase.Get(MinTimeForProcessingRelayResistanceApdu.Tag).EncodeValue()
                                                               .AsSpan());
        MinimumRelayResistanceGracePeriod minGracePeriod =
            MinimumRelayResistanceGracePeriod.Decode(tlvDatabase.Get(MinimumRelayResistanceGracePeriod.Tag).EncodeValue().AsSpan());

        RelaySeconds expectedProcessingTime = (RelaySeconds)minTimeForProcessingRelayResistanceApdu - (RelaySeconds)minGracePeriod;

        if ((RelaySeconds)processingTime < (expectedProcessingTime < RelaySeconds.Zero ? 0 : expectedProcessingTime))
            return false;

        return true;
    }

    public bool IsRelayResistanceWithinMaximumRange() => throw new NotImplementedException();

    #endregion

}