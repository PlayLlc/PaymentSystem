using System.Collections.Generic;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Database;

public interface IPersistentKernelValuesRepository
{
    #region Instance Members

    public Dictionary<KernelId, PersistentValues> Get(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);

    #endregion
}