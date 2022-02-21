using System.Collections.Generic;

using Play.Emv.DataElements.Emv;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Database;

public interface IPersistentKernelValuesRepository
{
    public Dictionary<KernelId, PersistentValues> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}