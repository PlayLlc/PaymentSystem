using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Issuer.Policies;

public interface IEnforceAcquirerInterfacePolicy
{
    List<TagLengthValue> Enforce(Dictionary<Tag, TagLengthValue> values);
    Tag[] GetExtendedIssuerInterface();
}