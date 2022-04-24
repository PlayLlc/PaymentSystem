using Microsoft.Toolkit.HighPerformance;

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

    /// <exception cref="OverflowException"></exception>
    public new int GetValueByteCount()
    {
        return _Value.Sum(a => a.GetByteCount());
    }

    #endregion

    #region Serialization

    /// <exception cref="OverflowException"></exception>
    public override byte[] EncodeValue()
    {
        Span<byte> buffer = stackalloc byte[_Value.Sum(a => a.GetByteCount())];
        int offset = 0;

        for (int i = 0; i < _Value.Count; i++)
            _Value.ElementAt(i).Serialize(buffer, ref offset);

        return buffer.ToArray();
    }

    #endregion
}