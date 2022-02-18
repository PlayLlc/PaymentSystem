using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Issuer.Policies;

namespace Play.Emv.Issuer.Messages;

internal sealed class AuthorizationRequest : AcquirerMessage
{
    #region Static Metadata

    public static AcquirerMessageId AcquirerMessageId = new(nameof(AuthorizationRequest));

    #endregion

    #region Instance Values

    private readonly List<TagLengthValue> _Values;

    #endregion

    #region Constructor

    private AuthorizationRequest(List<TagLengthValue> values)
    {
        _Values = values;
    }

    #endregion

    #region Instance Members

    // TODO: Double dispatch so outside actors can't circumvent the pattern and spread knowledge to unrelated processes
    // TODO: Should it be one policy or many? 
    public static DataNeeded GetDataNeeded(IEnforceAcquirerInterfacePolicy policy) => new(policy.GetExtendedIssuerInterface());

    public static AuthorizationRequest Create(Dictionary<Tag, TagLengthValue> values, IEnforceAcquirerInterfacePolicy acquirerPolicies) =>
        new(acquirerPolicies.Enforce(values));

    public override AcquirerMessageId GetAcquirerMessageId() => AcquirerMessageId;

    #endregion
}