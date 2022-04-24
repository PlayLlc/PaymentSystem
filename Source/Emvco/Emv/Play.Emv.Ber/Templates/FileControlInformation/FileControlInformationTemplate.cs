using Play.Ber.Identifiers;

namespace Play.Emv.Ber.Templates;

public abstract class FileControlInformationTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0x6F;

    #endregion

    #region Instance Members

    public abstract FileControlInformationProprietaryTemplate GetFileControlInformationProprietary();

    #endregion
}