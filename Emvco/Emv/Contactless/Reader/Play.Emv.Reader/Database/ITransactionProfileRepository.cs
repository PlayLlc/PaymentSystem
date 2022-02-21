using System.Collections.Generic;

using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Reader.Database;

public interface ITransactionProfileRepository
{
    public Dictionary<CombinationCompositeKey, TransactionProfile> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}