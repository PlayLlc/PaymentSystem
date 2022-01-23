using System.Collections.Generic;

using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public interface IPersistentKernelValuesRepository
{
    public Dictionary<KernelId, PersistentValues> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}