using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Data Storage EncodingId constructed as follows: Application PAN (without any 'F' padding) || Application PAN
///     Sequence Number If necessary, it is padded to the left with one hexadecimal zero  to ensure whole bytes. If
///     necessary, it is padded to the left with hexadecimal zeros to  ensure a minimum length of 8 bytes.
/// </summary>
public record DataStorageId : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F5E;
    private const byte _MinByteLength = 8;
    private const byte _MaxByteLength = 11;

    #endregion

    #region Constructor

    public DataStorageId(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageId Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageId Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageId Decode(ReadOnlySpan<byte> value)
    {
        const byte minCharLength = 16;
        const byte maxCharLength = 22;
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.NumericCodec.DecodeToBigInteger(value);

        Check.Primitive.ForMinCharLength(result.GetNumberOfDigits(), minCharLength, Tag);
        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), maxCharLength, Tag);

        return new DataStorageId(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount() => (ushort) PlayCodec.NumericCodec.GetByteCount(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <summary>
    ///     Concatenate from left to right the Application PAN (without any 'F' padding) with the Application PAN Sequence
    ///     Number (if the Application PAN Sequence Number is not present, then it is replaced by a '00' byte). The result, Y,
    ///     must be padded to the left with a hexadecimal zero if necessary to ensure whole bytes. It must also be padded to
    ///     the left with hexadecimal zeros if necessary to ensure a minimum length of 8 bytes.
    /// </summary>
    /// <remarks>Emv Book C-2 Section S456.19</remarks>
    public bool IsDataStorageIdValid(ApplicationPan pan, ApplicationPanSequenceNumber? sequenceNumber)
    {
        DataStorageId other = pan.AsDataStorageId(sequenceNumber);

        return _Value == other._Value;
    }

    #endregion
}