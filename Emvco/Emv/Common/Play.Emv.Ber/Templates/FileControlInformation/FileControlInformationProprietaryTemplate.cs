using Play.Ber.Identifiers;
using Play.Emv.Ber;

namespace Play.Emv.Templates;

public abstract class FileControlInformationProprietaryTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0xA5;

    #endregion

    #region Instance Members

    public abstract FileControlInformationIssuerDiscretionaryDataTemplate GetFileControlInformationIssuerDiscretionaryData();

    #endregion
}