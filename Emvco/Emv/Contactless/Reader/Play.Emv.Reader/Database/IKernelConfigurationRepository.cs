using System.Collections.Generic;

using Play.Emv.DataElements;

namespace Play.Emv.Reader.Database;

public interface IKernelConfigurationRepository
{
    public Dictionary<KernelId, KernelConfiguration> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}