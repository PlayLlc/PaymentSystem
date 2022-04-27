using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber.Templates;

public abstract record FileControlInformationIssuerDiscretionaryDataTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0xBF0C;

    #endregion

    #region Instance Values

    protected PrimitiveValue[] _Values;

    #endregion

    #region Constructor

    protected FileControlInformationIssuerDiscretionaryDataTemplate()
    {
        _Values = Array.Empty<PrimitiveValue>();
    }

    protected FileControlInformationIssuerDiscretionaryDataTemplate(PrimitiveValue[] values)
    {
        _Values = values;
    }

    #endregion

    #region Instance Members

    public PrimitiveValue[] GetOptionalValues() => _Values;

    #endregion
}