using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the priority of a given application or group of applications in a directory
/// </summary>
public record ApplicationPriorityIndicator : DataElement<byte>, IEqualityComparer<ApplicationPriorityIndicator>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x87;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public ApplicationPriorityIndicator(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool ApplicationCannotBeSelectedWithoutConfirmationByTheCardholder() => _Value.IsBitSet(Bits.Eight);
    public ApplicationPriorityRank GetApplicationPriorityRank() => ApplicationPriorityRankTypes.Get(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public static bool StaticEquals(ApplicationPriorityIndicator? x, ApplicationPriorityIndicator? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationPriorityIndicator Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ApplicationPriorityIndicator Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationPriorityIndicator Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new ApplicationPriorityIndicator(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationPriorityIndicator? x, ApplicationPriorityIndicator? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationPriorityIndicator obj) => obj.GetHashCode();

    #endregion
}