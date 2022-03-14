namespace Play.Emv.Terminal.Common.Services.RelayResistance;

public interface IValidateRelayResistanceProtocol
{
    public bool IsInRange();
    public bool IsRetryThresholdHit();
}