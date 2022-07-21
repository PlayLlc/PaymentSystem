using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the floor limit in the terminal in conjunction with the AID
/// </summary>
public record TerminalFloorLimit : DataElement<uint>, IEqualityComparer<TerminalFloorLimit>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F1B;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public TerminalFloorLimit(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Money AsMoney(NumericCurrencyCode numericCurrencyCode) => new(_Value, numericCurrencyCode);
    public Money AsMoney(CultureProfile cultureProfile) => new(_Value, cultureProfile.GetNumericCurrencyCode());
    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalFloorLimit Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalFloorLimit Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalFloorLimit Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new TerminalFloorLimit(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TerminalFloorLimit? x, TerminalFloorLimit? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalFloorLimit obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(TerminalFloorLimit left, ulong right) => left._Value == right;
    public static bool operator ==(ulong left, TerminalFloorLimit right) => left == right._Value;
    public static bool operator ==(TerminalFloorLimit left, uint right) => left._Value == right;
    public static bool operator ==(uint left, TerminalFloorLimit right) => left == right._Value;
    public static implicit operator uint(TerminalFloorLimit value) => value._Value;
    public static implicit operator TerminalFloorLimit(uint value) => new(value);
    public static implicit operator ulong(TerminalFloorLimit value) => value._Value;
    public static bool operator !=(TerminalFloorLimit left, ulong right) => !(left == right);
    public static bool operator !=(ulong left, TerminalFloorLimit right) => !(left == right);
    public static bool operator !=(TerminalFloorLimit left, uint right) => !(left == right);
    public static bool operator !=(uint left, TerminalFloorLimit right) => !(left == right);

    #endregion
}