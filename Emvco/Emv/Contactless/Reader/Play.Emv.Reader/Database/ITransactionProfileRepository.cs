using System.Collections.Generic;

using Play.Emv.DataElements.Emv.Primitives.Issuer;
using Play.Emv.DataElements.Emv.Primitives.Merchant;
using Play.Emv.DataElements.Emv.Primitives.Terminal;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Reader.Database;

public interface ITransactionProfileRepository
{
    public Dictionary<CombinationCompositeKey, TransactionProfile> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}