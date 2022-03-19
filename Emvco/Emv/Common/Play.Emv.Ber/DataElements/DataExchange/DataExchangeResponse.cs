using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber.DataElements;

public abstract record DataExchangeResponse : DataExchangeList<PrimitiveValue>
{
    #region Constructor

    protected DataExchangeResponse(PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public void Enqueue(DataExchangeResponse item)
    {
        Enqueue(item._Value.ToArray());
    }

    public PrimitiveValue[] AsPrimitiveValues() => _Value.ToArray();

    public bool TryGet(Tag tag, out PrimitiveValue? result)
    {
        result = _Value.FirstOrDefault(a => a.GetTag() == tag);

        return result is null;
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     EncodeValue
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public new byte[] EncodeValue()
    {
        return _Value.ToArray().SelectMany(a => a.EncodeTagLengthValue(_Codec)).ToArray();
    }

    #endregion
}