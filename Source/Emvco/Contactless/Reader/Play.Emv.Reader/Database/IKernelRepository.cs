using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Database;

public interface IKernelRepository
{
    #region Instance Members

    public Dictionary<KernelId, PrimitiveValue[]> GetKernelConfigurations(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification);

    #endregion
}