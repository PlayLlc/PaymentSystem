using Play.Codecs;

namespace Play.Interchange.DataFields;

public abstract record VariableDataField<T> : InterchangeDataField
{
    #region Instance Values

    protected readonly T _Value;

    #endregion

    #region Constructor

    protected VariableDataField(T value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override ushort GetByteCount() => (ushort) (GetLeadingOctetByteCount() + _Codec.GetByteCount(GetEncodingId(), _Value!));

    public override byte[] Encode()
    {
        byte[] result = new byte[GetByteCount()];
        int offset = 0;
        _Codec.Encode(GetEncodingId(), _Value!, result.AsMemory(), ref offset);

        return result;
    }

    public override void Encode(Memory<byte> buffer, ref int offset)
    {
        EncodeLeadingOctets(buffer.Span, ref offset);
        _Codec.Encode(GetEncodingId(), _Value!, buffer, ref offset);
    }

    protected abstract ushort GetMaxByteCount();

    protected void EncodeLeadingOctets(Span<byte> buffer, ref int offset)
    {
        ReadOnlySpan<byte> leadingOctets =
            PlayEncoding.Numeric.GetBytes(_Codec.GetByteCount(GetEncodingId(), _Value!), GetLeadingOctetByteCount());

        leadingOctets.CopyTo(buffer[offset..]);
        offset += leadingOctets.Length;
    }

    protected abstract ushort GetLeadingOctetByteCount();

    #endregion
}