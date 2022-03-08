using Play.Emv.Configuration;
using Play.Emv.DataElements.Emv.Primitives.Acquirer;
using Play.Emv.DataElements.Emv.Primitives.Merchant;
using Play.Emv.DataElements.Emv.Primitives.Terminal;

namespace Play.Emv.Terminal.Configuration;

public interface ITerminalConfigurationRepository
{
    #region Instance Members

    public TerminalConfiguration GeTerminalConfiguration(
        TerminalIdentification terminalIdentification,
        AcquirerIdentifier acquirerIdentifier,
        MerchantIdentifier merchantIdentifier);

    #endregion
}