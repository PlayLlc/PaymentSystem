using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Issuer.Messages;

namespace Play.Emv.Issuer.Policies;

public interface IEnforceAcquirerInterfacePolicy
{
    List<TagLengthValue> Enforce(Dictionary<Tag, TagLengthValue> values);
    Tag[] GetExtendedIssuerInterface();
}