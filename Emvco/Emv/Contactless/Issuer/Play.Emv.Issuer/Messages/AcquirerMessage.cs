using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.Issuer.Exceptions;
using Play.Emv.Issuer.Policies;

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