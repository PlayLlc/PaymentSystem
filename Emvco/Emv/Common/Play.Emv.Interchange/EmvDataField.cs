using Play.Interchange.Codecs;
using Play.Interchange.DataFields;

namespace Play.Emv.Interchange;

public abstract record EmvDataField<T> : InterchangeDataField
{
    #region Static Metadata

    protected static readonly InterchangeCodec _Codec = new();

    #endregion

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

    public ushort GetByteCount() => _Codec.GetByteCount(GetEncodingId(), _Value!);
    public byte[] Encode() => Encode(_Codec);
    public void Encode(Memory<byte> buffer, ref int offset) => Encode(_Codec, buffer, ref offset);
    public override ushort GetByteCount(InterchangeCodec codec) => codec.GetByteCount(GetEncodingId(), _Value!);

    public override byte[] Encode(InterchangeCodec codec)
    {
        byte[] result = new byte[GetByteCount(codec)];
        int offset = 0;
        codec.Encode(GetEncodingId(), _Value!, result.AsMemory(), ref offset);

        return result;
    }

    public override void Encode(InterchangeCodec codec, Memory<byte> buffer, ref int offset)
    {
        codec.Encode(GetEncodingId(), _Value!, buffer, ref offset);
    }

    #endregion
}