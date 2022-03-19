using Play.Ber.Exceptions;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber.DataElements;

public abstract record DataExchangeRequest : DataExchangeList<Tag>
{
    #region Constructor

    protected DataExchangeRequest(Tag[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool TryPeek(out Tag result) => _Value.TryPeek(out result);

    public void Enqueue(DataExchangeRequest item)
    {
        Enqueue(item._Value.ToArray());
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
        return _Value.ToArray().SelectMany(a => a.Serialize()).ToArray();
    }

    #endregion
}