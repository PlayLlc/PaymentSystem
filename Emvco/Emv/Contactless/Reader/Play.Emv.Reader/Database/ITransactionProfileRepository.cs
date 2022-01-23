using System.Collections.Generic;

using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public interface ITransactionProfileRepository
{
    public Dictionary<CombinationCompositeKey, TransactionProfile> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}