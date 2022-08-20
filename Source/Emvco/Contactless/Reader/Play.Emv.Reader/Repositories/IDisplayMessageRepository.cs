using System.Collections.Generic;

using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Configuration;

namespace Play.Emv.Reader;

public interface IDisplayMessageRepository
{
    #region Instance Members

    public Dictionary<LanguagePreference, DisplayMessages> GetDisplayMessages(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification,
        LanguagePreference languagePreference);

    #endregion
}