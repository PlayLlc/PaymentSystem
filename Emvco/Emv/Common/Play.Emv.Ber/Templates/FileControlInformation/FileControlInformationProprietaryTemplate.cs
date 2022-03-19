using Play.Ber.Identifiers;

namespace Play.Emv.Ber.Templates;

public abstract class FileControlInformationProprietaryTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0xA5;

    #endregion

    #region Instance Members

    public abstract FileControlInformationIssuerDiscretionaryDataTemplate GetFileControlInformationIssuerDiscretionaryData();

    #endregion
}