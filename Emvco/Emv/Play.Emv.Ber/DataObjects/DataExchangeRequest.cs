using Play.Ber.Identifiers;

namespace Play.Emv.Ber.DataObjects;

public abstract record DataExchangeRequest : DataExchangeList<Tag>
{
    #region Constructor

    protected DataExchangeRequest(Tag[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public void Enqueue(DataExchangeRequest item)
    {
        Enqueue(item._Value.ToArray());
    }

    #endregion

    #region Serialization

    public new byte[] EncodeValue()
    {
        return _Value.ToArray().SelectMany(a => a.Serialize()).ToArray();
    }

    #endregion
}