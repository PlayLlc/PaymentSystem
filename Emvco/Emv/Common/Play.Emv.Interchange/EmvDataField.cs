using Play.Interchange.Codecs;
using Play.Interchange.DataFields;

namespace Play.Emv.Interchange;

public abstract record EmvDataField<T> : InterchangeDataField where T : struct
{
    #region Instance Values

    protected readonly T _Value;

    #endregion

    #region Constructor

    protected EmvDataField(T value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    protected abstract InterchangeCodec GetCodec();
    public ushort GetByteCount() => GetCodec().GetByteCount(GetEncodingId(), _Value);
    public byte[] Encode() => Encode(GetCodec());
    public void Encode(Span<byte> buffer, ref int offset) => Encode(GetCodec(), buffer, ref offset);
    public override ushort GetByteCount(InterchangeCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public override byte[] Encode(InterchangeCodec codec)
    {
        byte[] result = new byte[GetByteCount(codec)];
        int offset = 0;
        codec.Encode(GetEncodingId(), _Value, result.AsSpan(), ref offset);

        return result;
    }

    public override void Encode(InterchangeCodec codec, Span<byte> buffer, ref int offset)
    {
        codec.Encode(GetEncodingId(), _Value, buffer, ref offset);
    }

    #endregion
}