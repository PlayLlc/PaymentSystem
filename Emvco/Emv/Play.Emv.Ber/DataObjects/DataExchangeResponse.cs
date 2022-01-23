using Play.Ber.DataObjects;

namespace Play.Ber.Emv.DataObjects;

public abstract record DataExchangeResponse : DataExchangeList<TagLengthValue>
{
    #region Constructor

    protected DataExchangeResponse(TagLengthValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public void Enqueue(DataExchangeResponse item)
    {
        Enqueue(item._Value.ToArray());
    }

    public TagLengthValue[] AsTagLengthValues()
    {
        return _Value.ToArray();
    }

    #endregion

    #region Serialization

    public new byte[] EncodeValue()
    {
        return _Value.ToArray().SelectMany(a => a.EncodeTagLengthValue()).ToArray();
    }

    #endregion
}