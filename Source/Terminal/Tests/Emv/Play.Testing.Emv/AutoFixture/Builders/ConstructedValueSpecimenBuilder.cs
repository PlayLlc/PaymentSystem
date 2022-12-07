using Play.Ber.DataObjects;

namespace Play.Testing.Emv;

public abstract class ConstructedValueSpecimenBuilder<_T> : SpecimenBuilder where _T : ConstructedValue
{
    #region Instance Values

    protected DefaultConstructedValueSpecimen<_T> _DefaultSpecimen;

    #endregion

    #region Constructor

    protected ConstructedValueSpecimenBuilder(DefaultConstructedValueSpecimen<_T> value)
    {
        _DefaultSpecimen = value;
    }

    #endregion

    #region Instance Members

    public _T GetDefault() => _DefaultSpecimen.GetDefault();
    public byte[] GetDefaultEncodedTagLengthValue() => _DefaultSpecimen.GetDefaultEncodedTagLengthValue();
    public byte[] GetDefaultEncodedValue() => _DefaultSpecimen.GetDefaultEncodedValue();

    #endregion
}