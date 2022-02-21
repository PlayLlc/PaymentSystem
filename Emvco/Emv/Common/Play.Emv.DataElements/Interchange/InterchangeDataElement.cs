using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Interchange;

public abstract record InterchangeDataElement<T> : DataElement<T>
{
    #region Constructor

    protected InterchangeDataElement(T value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ushort GetDataFieldId() => (ushort) ((uint) GetTag()).GetMaskedValue(0xC0);

    #endregion
}