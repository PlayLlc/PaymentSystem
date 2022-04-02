using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;

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
    {
        _Value = value;
    }

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

    #region Instance Members

    public byte[] AsByteArray() => _Value.ToByteArray();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => (ushort) _Value.GetByteCount();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}