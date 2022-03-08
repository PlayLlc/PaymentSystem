using System.Collections.Generic;

using Play.Emv.DataElements.Emv.Primitives.Issuer;
using Play.Emv.DataElements.Emv.Primitives.Kernel;
using Play.Emv.DataElements.Emv.Primitives.Merchant;
using Play.Emv.DataElements.Emv.Primitives.Terminal;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Database;

public interface IPersistentKernelValuesRepository
{
    public Dictionary<KernelId, PersistentValues> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}