using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     An issuer assigned value that is retained by the terminal during the verification process of the Signed Static
///     Application Data
/// </summary>
public record DataAuthenticationCode : DataElement<ushort>, IEqualityComparer<DataAuthenticationCode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F45;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public DataAuthenticationCode(ushort value) : base(value)
    { }

    #endregion

    #region Serialization

    public override DataAuthenticationCode Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    public static DataAuthenticationCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataAuthenticationCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new DataAuthenticationCode(result);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(DataAuthenticationCode? x, DataAuthenticationCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataAuthenticationCode obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public byte[] AsBytes()
    {
        return new[] {(byte) (_Value >> 8), (byte) _Value};
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}