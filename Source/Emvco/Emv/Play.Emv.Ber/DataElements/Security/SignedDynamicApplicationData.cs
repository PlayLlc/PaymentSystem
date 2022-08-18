using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Digital signature on critical application parameters for DDA or CDA
/// </summary>
public record SignedDynamicApplicationData : DataElement<BigInteger>, IEqualityComparer<SignedDynamicApplicationData>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F4B;

    #endregion

    #region Constructor

    public SignedDynamicApplicationData(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.ToByteArray(true);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => (ushort) _Value.GetByteCount(true);
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static SignedDynamicApplicationData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override SignedDynamicApplicationData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static SignedDynamicApplicationData Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new SignedDynamicApplicationData(result);
    }

    #endregion

    #region Equality

    public bool Equals(SignedDynamicApplicationData? x, SignedDynamicApplicationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(SignedDynamicApplicationData obj) => obj.GetHashCode();

    #endregion
}