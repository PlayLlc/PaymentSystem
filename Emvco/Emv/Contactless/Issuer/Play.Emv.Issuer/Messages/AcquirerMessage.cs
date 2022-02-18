namespace Play.Emv.Issuer.Messages;

public abstract class AcquirerMessage
{
    #region Instance Members

    public abstract AcquirerMessageId GetAcquirerMessageId();

    #endregion

    //public void Validate(Dictionary<Tag, TagLengthValue> values, List<IEnforceAcquirerInterfacePolicy> policies)
    //{
    //    foreach (var requiredDataElement in GetRequiredDataElements())
    //        Check.Request.ForRequiredDataElement(values, requiredDataElement, GetAcquirerMessageId());

    //    Policies(values, policies);
    //}

    //protected void Policies(Dictionary<Tag, TagLengthValue> values, List<IEnforceAcquirerInterfacePolicy> policies)
    //{
    //    foreach (IEnforceAcquirerInterfacePolicy policy in policies)
    //        policy.Enforce(values);
    //}
}