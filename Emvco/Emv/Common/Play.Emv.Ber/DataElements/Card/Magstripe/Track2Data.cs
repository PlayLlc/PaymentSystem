using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Track 2 Data contains the data objects of the track 2 according to [ISO/IEC 7813], excluding start sentinel, end
///     sentinel and LRC. The Track 2 Data has a maximum length of 37 positions and is present in the file read using the
///     READ RECORD command during a mag-stripe mode transaction. It is made up of the following sub-fields:
/// </summary>
/// <remarks>
///     There are two formats used to encode Track 2 data. Those two different formats are represented by the 2
///     different constant start and end sentinels as well as the field separator
/// </remarks>
public record Track2Data : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F6B;
    private const byte _MaxByteLength = 19;
    private const byte _StartSentinel1 = 0xB;
    private const byte _StartSentinel2 = (byte) ';';
    private const byte _FieldSeparator1 = 0xD;
    private const byte _FieldSeparator2 = (byte) '=';
    private const byte _EndSentinel1 = 0xF;
    private const byte _EndSentinel2 = (byte) '?';

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public Track2Data(BigInteger value) : base(value)
    {
        Check.Primitive.ForMaximumValue((byte) value.GetByteCount(), _MaxByteLength, Tag);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     GetPrimaryAccountNumber
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public ApplicationPan GetPrimaryAccountNumber()
    {
        Span<byte> buffer = _Value.ToByteArray();

        // There are two different formats used to encode Track 2
        if ((buffer[0] == _StartSentinel1) || (buffer[0] == _StartSentinel2))
            buffer[1..].CopyTo(buffer[..]);

        for (int i = 0; i < ApplicationPan.GetMaxByteCount(); i++)
        {
            if ((buffer[i] == _FieldSeparator1) || (buffer[i] == _FieldSeparator2))
                return ApplicationPan.Decode(buffer[..i]);
        }

        throw new BerParsingException($"The {nameof(Track2Data)} could not decode a valid {nameof(ApplicationPan)}");
    }

    /// <summary>
    ///     GetTrack2DiscretionaryData
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public Track2DiscretionaryData GetTrack2DiscretionaryData()
    {
        Span<byte> buffer = _Value.ToByteArray();

        return Track2DiscretionaryData.Decode(buffer[GetDiscretionaryDataOffset(buffer)..]);
    }

    /// <summary>
    ///     GetPrimaryAccountNumberOffset
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    private int GetPrimaryAccountNumberOffset(ReadOnlySpan<byte> buffer)
    {
        int offset = 0;

        // There are two different formats used to encode Track 2
        if ((buffer[0] == _StartSentinel1) || (buffer[0] == _StartSentinel2))
            offset++;

        for (; offset < ApplicationPan.GetMaxByteCount(); offset++)
        {
            if ((buffer[offset] == _FieldSeparator1) || (buffer[offset] == _FieldSeparator2))
                return offset;
        }

        throw new BerParsingException($"The {nameof(Track2Data)} could not decode a valid {nameof(ApplicationPan)}");
    }

    private int GetExpiryDateOffset(ReadOnlySpan<byte> buffer) => GetPrimaryAccountNumberOffset(buffer) + 1;
    private int GetServiceCodeOffset(ReadOnlySpan<byte> buffer) => GetExpiryDateOffset(buffer) + 2;
    private int GetDiscretionaryDataOffset(ReadOnlySpan<byte> buffer) => GetServiceCodeOffset(buffer) + 3;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2Data Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override Track2Data Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2Data Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new Track2Data(result);
    }

    public new byte[] EncodeValue() => _Value.ToByteArray();
    public new byte[] EncodeValue(int length) => _Value.ToByteArray()[..length];

    #endregion
}