using Play.Interchange.Exceptions;

namespace Play.Interchange.DataFields;

public abstract record FixedDataField<T> : InterchangeDataField
{
    #region Instance Values

    protected readonly T _Value;

    #endregion

    #region Constructor

    protected FixedDataField(T value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override ushort GetByteCount() => _Codec.GetByteCount(GetEncodingId(), _Value!);

    public override byte[] Encode()
    {
        byte[] result = new byte[GetByteCount()];
        int offset = 0;
        _Codec.Encode(GetEncodingId(), _Value!, result.AsMemory(), ref offset);

        return result;
    }

    public override void Encode(Memory<byte> buffer, ref int offset)
    {
        _Codec.Encode(GetEncodingId(), _Value!, buffer, ref offset);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    public byte[] Decode(ReadOnlySpan<byte> value)
    {
        Check.DataField.ForExactLength(value, GetByteCount(), GetDataFieldId());

        return value.ToArray();
    }

    #endregion
}