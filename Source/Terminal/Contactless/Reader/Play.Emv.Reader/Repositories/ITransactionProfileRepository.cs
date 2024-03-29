﻿using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;

namespace Play.Emv.Reader;

public interface ITransactionProfileRepository
{
    #region Instance Members

    public Dictionary<CombinationCompositeKey, PrimitiveValue[]> GetTransactionProfiles(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification);

    #endregion
}