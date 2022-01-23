using System.Collections.Generic;

using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public interface IKernelConfigurationRepository
{
    public Dictionary<KernelId, KernelConfiguration> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}