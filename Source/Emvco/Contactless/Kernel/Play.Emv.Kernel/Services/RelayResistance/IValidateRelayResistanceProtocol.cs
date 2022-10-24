using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Globalization.Time;

namespace Play.Emv.Kernel.Services;

public interface IValidateRelayResistanceProtocol
{
    #region Instance Members

    bool IsInRange(TransactionSessionId transactionSessionId, Microseconds timeElapsed, IReadTlvDatabase tlvDatabase);
    bool IsRetryThresholdHit(int retryCount);
    bool IsRelayResistanceWithinMinimumRange(MeasuredRelayResistanceProcessingTime processingTime, IReadTlvDatabase tlvDatabase);
    bool IsRelayResistanceWithinMaximumRange(MeasuredRelayResistanceProcessingTime processingTime, IReadTlvDatabase tlvDatabase);
    MeasuredRelayResistanceProcessingTime CalculateMeasuredRrpTime(Microseconds timeElapsed, IReadTlvDatabase tlvDatabase);

    #endregion
}