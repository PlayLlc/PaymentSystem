using Play.Ber.DataObjects;

namespace Play.Testing.Emv;

public class DefaultPrimitiveValueSpecimen<_T> : DefaultSpecimen<_T> where _T : PrimitiveValue
{
    #region Instance Values

    protected virtual _T _Default { get; }
    protected virtual byte[] _RawTagLengthValue { get; }
    protected virtual byte[] _ContentOctets { get; }

    #endregion

    #region Constructor

    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    public DefaultPrimitiveValueSpecimen(_T value, byte[] contentOctets)
    {
        _Default = value;
        _RawTagLengthValue = new TagLengthValue(value.GetTag(), contentOctets).EncodeTagLengthValue();
        _ContentOctets = contentOctets;
    }

    #endregion

    #region Instance Members

    public byte[] GetDefaultEncodedTagLengthValue() => _RawTagLengthValue;
    public byte[] GetDefaultEncodedValue() => _ContentOctets;
    public override _T GetDefault() => _Default;

    #endregion
}