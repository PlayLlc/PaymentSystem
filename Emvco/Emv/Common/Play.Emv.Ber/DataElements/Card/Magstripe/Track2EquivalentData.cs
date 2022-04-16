using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core;
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

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public int GetNumberOfDigitsInPrimaryAccountNumber() => GetPrimaryAccountNumberNibbleOffset(_Value.ToByteArray().AsNibbleArray());

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetPrimaryAccountNumberNibbleOffset(ReadOnlySpan<Nibble> value)
    {
        for (int i = 0; i < (ApplicationPan.GetMaxByteCount() * 2); i++)
        {
            if (value[i] == _FieldSeparator1)
                return i;
        }

        throw new DataElementParsingException(
            $"A valid {nameof(ApplicationPan)} could not be resolved from the {nameof(Track2EquivalentData)}");
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetExpirationDateNibbleOffset(ReadOnlySpan<Nibble> buffer) => GetPrimaryAccountNumberNibbleOffset(buffer) + 1;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetServiceCodeNibbleOffset(ReadOnlySpan<Nibble> buffer) => GetExpirationDateNibbleOffset(buffer) + 4;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetDiscretionaryDataNibbleOffset(ReadOnlySpan<Nibble> buffer) => GetServiceCodeNibbleOffset(buffer) + 3;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PrimaryAccountNumber GetPrimaryAccountNumber()
    {
        ReadOnlySpan<Nibble> buffer = _Value.ToByteArray().AsNibbleArray();

        return new PrimaryAccountNumber(buffer[..GetPrimaryAccountNumberNibbleOffset(buffer)]);
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public ShortDate GetExpirationDate()
    {
        ReadOnlySpan<Nibble> buffer = _Value.ToByteArray().AsNibbleArray();

        return new ShortDate(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(buffer[..GetExpirationDateNibbleOffset(buffer)].AsByteArray()
            .AsSpan()));
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public ServiceCode GetServiceCode()
    {
        ReadOnlySpan<Nibble> buffer = _Value.ToByteArray().AsNibbleArray();

        return new ServiceCode(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(buffer[..GetServiceCodeNibbleOffset(buffer)].AsByteArray()
            .AsSpan()));
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public byte[] GetDiscretionaryData()
    {
        ReadOnlySpan<Nibble> buffer = _Value.ToByteArray().AsNibbleArray();

        return buffer[GetDiscretionaryDataNibbleOffset(buffer)..].AsByteArray();
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public Track2EquivalentData UpdateDiscretionaryData(ReadOnlySpan<byte> discretionaryData)
    {
        ReadOnlySpan<Nibble> valueBuffer = _Value.ToByteArray().AsNibbleArray();
        ReadOnlySpan<Nibble> discretionaryDataBuffer = discretionaryData.AsNibbleArray();
        int discretionaryDataOffset = GetDiscretionaryDataNibbleOffset(valueBuffer);

        int nibbleCount = discretionaryDataOffset + discretionaryDataBuffer.Length;

        Span<Nibble> resultBuffer = stackalloc Nibble[nibbleCount + (nibbleCount % 2)];

        valueBuffer[..discretionaryDataOffset].CopyTo(resultBuffer);
        discretionaryDataBuffer.CopyTo(resultBuffer[discretionaryDataOffset..]);

        if ((nibbleCount % 2) != 0)
            resultBuffer[^1] = Nibble.MaxValue;

        return Decode(resultBuffer.AsByteArray().AsSpan());
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public Track2EquivalentData UpdateDiscretionaryData(ReadOnlySpan<Nibble> discretionaryData)
    {
        ReadOnlySpan<Nibble> valueBuffer = _Value.ToByteArray().AsNibbleArray();
        int discretionaryDataOffset = GetDiscretionaryDataNibbleOffset(valueBuffer);

        int nibbleCount = discretionaryDataOffset + discretionaryData.Length;

        Span<Nibble> resultBuffer = stackalloc Nibble[nibbleCount + (nibbleCount % 2)];

        valueBuffer[..discretionaryDataOffset].CopyTo(resultBuffer);
        discretionaryData.CopyTo(resultBuffer[discretionaryDataOffset..]);

        if ((nibbleCount % 2) != 0)
            resultBuffer[^1] = Nibble.MaxValue;

        return Decode(resultBuffer.AsByteArray().AsSpan());
    }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2EquivalentData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="BerParsingException"></exception>
    public override Track2EquivalentData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
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