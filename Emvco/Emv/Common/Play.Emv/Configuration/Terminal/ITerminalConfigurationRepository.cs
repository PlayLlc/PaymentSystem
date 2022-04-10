using Play.Emv.Ber.DataElements;

namespace Play.Emv.Configuration;

internal interface ITerminalConfigurationRepository
{
    #region Instance Members

    public TerminalConfiguration Get(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);

    #endregion
}