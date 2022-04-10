namespace Play.Emv.Kernel.Services;

public interface IValidateRelayResistanceProtocol
{
    #region Instance Members

    public bool IsInRange();
    public bool IsRetryThresholdHit();

    #endregion
}