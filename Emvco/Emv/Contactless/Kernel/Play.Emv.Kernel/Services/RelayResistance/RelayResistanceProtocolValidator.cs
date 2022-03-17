using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Sessions;
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
    private static MeasuredRelayResistanceProcessingTime CalculateMeasuredRrpTime(Seconds timeElapsed, IQueryTlvDatabase tlvDatabase)
    {
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissio
                   TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode(tlvD
                                                                                      .Get(TerminalExpectedTransmissionTimeForRelayResistan
                                                                                               .Tag).EncodeValue().AsSpan());
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissio
                   TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode(tlvD
                                                                                      .Get(TerminalExpectedTransmissionTimeForRelayResistan
                                                                                               .Tag).EncodeValue().AsSpan());
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceExpectedRapduTransmissio
                   DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(tlvD
                                                                                     .Get(DeviceEstimatedTransmissionTimeForRelayResistan
                                                                                              .Tag).EncodeValue().AsSpan());

        MeasuredRelayResistanceProcessingTime processin
                   MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissi
                                                                terminalExpectedRapduTransmissionTime, deviceExpectedRapduTransmissionTime);

        return processingTime;
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static bool IsRelayResistanceWithinMinimumRange(MeasuredRelayResistanceProcessingTime processingTime, IQueryTlvDatabase tlvDatabase)
    {
        MinTimeForProcessingRelayResistanceApdu minTimeForProcessingRelayRe
            
            MinTimeForProcessingRelayResistanceApdu.Decode(tlvDatabase.Get(MinTimeForProcessingRelayResistanceApdu.Tag)
                                                                             .AsSpan());
        MinimumRelayResistanceGracePeriod mi
            
            MinimumRelayResistanceGracePeriod.Decode(tlvDatabase.Get(MinimumRelayResistanceGracePeriod.Tag).EncodeValue().AsSpan());

        int expectedProcessingTime = (ushort) minTimeForProcessingRelayResistanceApdu - (ushort) minGracePeriod;

        if ((ushort) processingTime < (expectedProcessingTime < 0 ? 0 : expectedProcessingTime))
            return false;

        return true;
    }

    public bool IsRelayResistanceWithinMaximumRange() => throw new NotImplement

    #endregion
  #endregion
}