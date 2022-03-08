using System.Collections.Generic;

using Play.Emv.DataElements.Emv.Primitives.Issuer;
using Play.Emv.DataElements.Emv.Primitives.Kernel;
using Play.Emv.DataElements.Emv.Primitives.Merchant;
using Play.Emv.DataElements.Emv.Primitives.Terminal;

namespace Play.Emv.Reader.Database;

public interface IKernelConfigurationRepository
{
    public Dictionary<KernelId, KernelConfiguration> Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}