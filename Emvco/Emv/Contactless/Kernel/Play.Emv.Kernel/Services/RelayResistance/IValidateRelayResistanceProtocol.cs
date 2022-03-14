namespace Play.Emv.Kernel.Services;

public interface IValidateRelayResistanceProtocol
{
    public bool IsInRange();
    public bool IsRetryThresholdHit();
}