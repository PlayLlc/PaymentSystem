using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements;

public record Track2EquivalentData : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x57;
    private const byte _MaxByteLength = 19;
    private const byte _StartSentinel1 = 0xB;
    private const byte _StartSentinel2 = (byte) ';';
    private const byte _FieldSeparator1 = 0xD;
    private const byte _FieldSeparator2 = (byte) '=';
    private const byte _EndSentinel1 = 0xF;
    private const byte _EndSentinel2 = (byte) '?';

    #endregion

    #region Constructor

    public Track2EquivalentData(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public PrimaryAccountNumber GetPrimaryAccountNumber()
    {
        ReadOnlySpan<byte> buffer = _Value.ToByteArray();
        int offset = GetPrimaryAccountNumberNibbleOffset(buffer);

        Span<byte> result = stackalloc byte[(offset / 2) + (offset % 2)];

        buffer[..offset].CopyTo(result);

        if ((offset % 2) != 0)
            result.ShiftRightOneNibble();

        return PrimaryAccountNumber.Decode(result);
    }

    public ShortDate GetExpiryDate()
    {
        ReadOnlySpan<byte> buffer = _Value.ToByteArray();
        int offset = GetExpirationDateNibbleOffset(buffer);

        return new ShortDate(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(buffer[offset..(offset + 2)]));
    }

    public ServiceCode GetServiceCode()
    {
        ReadOnlySpan<byte> buffer = _Value.ToByteArray();
        int offset = GetServiceCodeNibbleOffset(buffer);

        Span<byte> serviceCodeOctets = stackalloc byte[2];
        buffer[offset..(offset + 2)].CopyTo(serviceCodeOctets);

        if ((offset % 2) == 0)
            serviceCodeOctets.ShiftRightOneNibble();
        else
            serviceCodeOctets[0] = serviceCodeOctets[0].GetMaskedValue(0xF0);

        return new ServiceCode(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(serviceCodeOctets));
    }

    // HACK: WHAT ARE YOU DOING YOU DWEEB?
    public Track2EquivalentData ZeroFillDiscretionaryDataWith13HexZeros()
    {
        ReadOnlySpan<byte> buffer = _Value.ToByteArray();
        int discretionaryDataNibbleOffset = GetDiscretionaryDataNibbleOffset(buffer);
        bool isEven = (discretionaryDataNibbleOffset % 2) == 0;
        int discretionaryDataByteOffset = discretionaryDataNibbleOffset / 2;
        int resultByteCount = isEven ? discretionaryDataByteOffset + 7 : discretionaryDataByteOffset + 6;
        Span<byte> resultContent = new byte[resultByteCount];
        resultContent.Fill(0x00);

        // HACK: I think this might not be filling the buffer completely with the value
        buffer[..discretionaryDataByteOffset].CopyTo(resultContent);

        if (!isEven)
            resultContent[discretionaryDataByteOffset].GetMaskedValue(0x0F);

        return Decode(resultContent);
    }

    private void FillOdd()
    { }

    /// <exception cref="DataElementParsingException"></exception>
    public int GetNumberOfDigitsInPrimaryAccountNumber()
    {
        int offset = 0;
        ReadOnlySpan<byte> rawContentOctets = _Value.ToByteArray();

        return GetPrimaryAccountNumberNibbleOffset(rawContentOctets);
    }

    public int GetPrimaryAccountNumberNibbleOffset(ReadOnlySpan<byte> buffer)
    {
        int offset = 0;

        // There are two different formats used to encode Track 2
        if ((buffer[0] == _StartSentinel1) || (buffer[0] == _StartSentinel2))
            offset++;

        for (; offset < PrimaryAccountNumber.GetMaxByteLength(); offset++)
        {
            if ((offset % 2) != 0)
            {
                byte leftNibble = (byte) (buffer[offset] >> 4);

                if (leftNibble is _FieldSeparator1 or _FieldSeparator2)
                    return offset;
            }
            else
            {
                byte rightNibble = (byte) buffer[offset].GetMaskedValue(0xF0);

                if (rightNibble is _FieldSeparator1 or _FieldSeparator2)
                    return offset;
            }
        }

        throw new
            DataElementParsingException($"A valid {nameof(PrimaryAccountNumber)} could not be resolved from the {nameof(Track2EquivalentData)}");
    }

    private int GetExpirationDateNibbleOffset(ReadOnlySpan<byte> buffer) => GetPrimaryAccountNumberNibbleOffset(buffer) + 1;
    private int GetServiceCodeNibbleOffset(ReadOnlySpan<byte> buffer) => GetExpirationDateNibbleOffset(buffer) + 2;
    private int GetDiscretionaryDataNibbleOffset(ReadOnlySpan<byte> buffer) => GetExpirationDateNibbleOffset(buffer) + 3;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2EquivalentData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override Track2EquivalentData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2EquivalentData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new Track2EquivalentData(result);
    }

    public new byte[] EncodeValue() => _Value.ToByteArray();
    public new byte[] EncodeValue(int length) => _Value.ToByteArray()[..length];

    #endregion
}