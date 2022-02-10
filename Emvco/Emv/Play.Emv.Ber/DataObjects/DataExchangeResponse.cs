using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber.DataObjects;

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

    public TagLengthValue[] AsTagLengthValues() => _Value.ToArray();

    public bool TryGet(Tag tag, out TagLengthValue? result)
    {
        result = _Value.FirstOrDefault(a => a.GetTag() == tag);

        return result is null;
    }

    #endregion

    #region Serialization

    public new byte[] EncodeValue()
    {
        return _Value.ToArray().SelectMany(a => a.EncodeTagLengthValue()).ToArray();
    }

    #endregion
}