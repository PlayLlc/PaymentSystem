using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     This data allows the Kernel to check the consistency between DS Summary 1 and DS Summary 2, and so to ensure that
///     DS ODS Card is provided by a genuine Card. It is located in the ICC Dynamic Data recovered from the Signed Dynamic
///     Application Data.
/// </summary>
public record DataStorageSummary2 : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8101;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MinByteLength = 8;
    private const byte _MaxByteLength = 16;

    #endregion

    #region Constructor

    public DataStorageSummary2(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageSummary2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageSummary2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageSummary2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new DataStorageSummary2(result);
    }

    #endregion

    public static explicit operator BigInteger(DataStorageSummary2 value) => value._Value;
    public static bool operator ==(DataStorageSummary1 left, DataStorageSummary2 right) => (BigInteger) left == right._Value;
    public static bool operator !=(DataStorageSummary1 left, DataStorageSummary2 right) => (BigInteger) left != right._Value;
    public static bool operator ==(DataStorageSummary2 left, DataStorageSummary1 right) => left._Value == (BigInteger) right;
    public static bool operator !=(DataStorageSummary2 left, DataStorageSummary1 right) => left._Value != (BigInteger) right;
}