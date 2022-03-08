using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.Templates.FileControlInformation;

public abstract class FileControlInformationProprietaryTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0xA5;

    #endregion

    #region Instance Members

    public abstract FileControlInformationIssuerDiscretionaryDataTemplate GetFileControlInformationIssuerDiscretionaryData();

    #endregion
}