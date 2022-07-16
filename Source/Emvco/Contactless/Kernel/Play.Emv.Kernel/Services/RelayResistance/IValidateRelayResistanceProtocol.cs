using Play.Emv.Ber;
using Play.Emv.Identifiers;
using Play.Globalization.Time;

namespace Play.Emv.Kernel.Services;

public interface IValidateRelayResistanceProtocol
{
    #region Instance Members

    public bool IsInRange(TransactionSessionId transactionSessionId, Milliseconds timeElapsed, IReadTlvDatabase tlvDatabase);
    public bool IsRetryThresholdHit();

    #endregion
}