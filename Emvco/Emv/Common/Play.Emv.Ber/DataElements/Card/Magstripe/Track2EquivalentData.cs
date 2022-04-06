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

    #region _New

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private int GetPrimaryAccountNumberNibbleOffset(ReadOnlySpan<Nibble> value)
    {
        for (int i = 0; i < (PrimaryAccountNumber._MaxByteLength * 2); i++)
        {
            if (value[i] == _FieldSeparator1)
                return i;
        }

        throw new
            DataElementParsingException($"A valid {nameof(PrimaryAccountNumber)} could not be resolved from the {nameof(Track2EquivalentData)}");
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
    public PrimaryAccountNumber GetPrimaryAccountNumber()
    {
        ReadOnlySpan<Nibble> buffer = _Value.ToByteArray().AsNibbleArray();

        return PrimaryAccountNumber.Decode(buffer[..GetPrimaryAccountNumberNibbleOffset(buffer)].AsByteArray().AsSpan());
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
    public Track2EquivalentData UpdateDiscretionaryData(ReadOnlySpan<byte> value)
    {
        ReadOnlySpan<Nibble> valueBuffer = value.AsNibbleArray();
    }

    #endregion
}