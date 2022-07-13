using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Valid cardholder account number
/// </summary>
public record ApplicationPan : DataElement<TrackPrimaryAccountNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = CompressedNumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5A;
    private const byte _MaxByteCount = 10;
    private const byte _MaxCharCount = 19;

    #endregion

    #region Constructor

    public ApplicationPan(TrackPrimaryAccountNumber value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static int GetMaxByteCount() => _MaxByteCount;
    public static int GetMaxCharCount() => _MaxCharCount;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <summary>
    ///     Concatenate from left to right the Application PAN (without any 'F' padding) with the Application PAN Sequence
    ///     Number (if the Application PAN Sequence Number is not present, then it is replaced by a '00' byte). The result, Y,
    ///     must be padded to the left with a hexadecimal zero if necessary to ensure whole bytes. It must also be padded to
    ///     the left with hexadecimal zeroes if necessary to ensure a minimum length of 8 bytes.
    /// </summary>
    /// <remarks>Emv Book C-2 Section S456.19</remarks>
    /// <exception cref="OverflowException"></exception>
    public DataStorageId AsDataStorageId(ApplicationPanSequenceNumber? sequenceNumber)
    {
        const byte minDataStorageIdLength = 8;

        Span<byte> valueBuffer = _Value.Encode().RemoveLeftPadding(new Nibble(0xF));

        int resultByteCount = valueBuffer.Length + (sequenceNumber == null ? 0 : 1);

        if (resultByteCount < minDataStorageIdLength)
            return CreateDataStorageIdMinimumLengthDataStorageId(valueBuffer, sequenceNumber);

        Span<byte> resultBuffer = stackalloc byte[resultByteCount];

        if (sequenceNumber != null)
            resultBuffer[^1] = (byte) sequenceNumber!;

        valueBuffer.CopyTo(resultBuffer);

        return new DataStorageId(new BigInteger(resultBuffer));
    }

    /// <summary>
    ///     Checks whether the Issuer EncodingId provided in the argument matches the leftmost 5 - 8 (issuer exact length is 3 bytes) PAN digits (allowing for
    ///     the possible padding of the Issuer EncodingId with hexadecimal 'F's)
    /// </summary>
    /// <param name="issuerIdentifier"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public bool IsIssuerIdentifierMatching(IssuerIdentificationNumber issuerIdentifier)
    {
        uint thisPan = PlayCodec.NumericCodec.DecodeToUInt32(_Value.Encode()[5..8]);

        return thisPan == (uint) issuerIdentifier;
    }

    private DataStorageId CreateDataStorageIdMinimumLengthDataStorageId(ReadOnlySpan<byte> value, ApplicationPanSequenceNumber? sequenceNumber)
    {
        Span<byte> resultBuffer = stackalloc byte[8];

        if (sequenceNumber != null)
            resultBuffer[^1] = (byte) sequenceNumber!;

        value.CopyTo(resultBuffer);

        return new DataStorageId(new BigInteger(resultBuffer));
    }

    /// <exception cref="OverflowException"></exception>
    private bool IsCheckSumValid() => _Value.IsCheckSumValid();

    #endregion

    #region Serialization

    public override byte[] EncodeValue() => _Value.Encode();

    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static ApplicationPan Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public override ApplicationPan Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static ApplicationPan Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteCount, Tag);

        Nibble[] result = PlayCodec.CompressedNumericCodec.DecodeToNibbles(value);

        Check.Primitive.ForMaxCharLength(result.Length, _MaxCharCount, Tag);

        return new ApplicationPan(new TrackPrimaryAccountNumber(result));
    }

    #endregion

    #region Equality

    public bool Equals(ApplicationPan? x, ApplicationPan? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationPan obj) => obj.GetHashCode();

    #endregion
}