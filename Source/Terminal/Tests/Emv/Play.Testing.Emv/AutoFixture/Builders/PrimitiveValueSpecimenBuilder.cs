using Play.Ber.DataObjects;

namespace Play.Testing.Emv;

public abstract class PrimitiveValueSpecimenBuilder<_T> : SpecimenBuilder where _T : PrimitiveValue
{
    #region Instance Values

    protected DefaultPrimitiveValueSpecimen<_T> _DefaultSpecimen;

    #endregion

    #region Constructor

    protected PrimitiveValueSpecimenBuilder(DefaultPrimitiveValueSpecimen<_T> value)
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