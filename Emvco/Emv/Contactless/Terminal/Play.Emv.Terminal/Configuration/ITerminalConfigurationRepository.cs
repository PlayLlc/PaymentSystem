using Play.Emv.DataElements;

using TerminalConfiguration = Play.Emv.Configuration.TerminalConfiguration;

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