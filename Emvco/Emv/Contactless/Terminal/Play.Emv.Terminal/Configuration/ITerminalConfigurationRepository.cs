using Play.Emv.Ber.DataElements;
using Play.Emv.Configuration;

namespace Play.Emv.Terminal.Configuration;

public interface ITerminalConfigurationRepository
{
    #region Instance Members

    public TerminalConfiguration GeTerminalConfiguration(
        TerminalIdentification terminalIdentification, AcquirerIdentifier acquirerIdentifier, MerchantIdentifier merchantIdentifier);

    #endregion
}