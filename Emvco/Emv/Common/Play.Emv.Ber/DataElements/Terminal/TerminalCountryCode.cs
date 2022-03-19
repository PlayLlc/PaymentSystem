using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;
using Play.Globalization;
using Play.Globalization.Country;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the country of the terminal, represented according to ISO 3166
/// </summary>
public record TerminalCountryCode : DataElement<NumericCountryCode>, IEqualityComparer<TerminalCountryCode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly TerminalCountryCode Default = new(new NumericCountryCode(0));
    public static readonly Tag Tag = 0x9F1A;
    private const byte _ByteLength = 2;
    private const byte _CharLength = 3;

    #endregion

    #region Constructor

    public TerminalCountryCode(NumericCountryCode value) : base(value)
    { }

    public TerminalCountryCode(CultureProfile value) : base(value.GetNumericCountryCode())
    { }

    #endregion

    #region Instance Members

    public NumericCountryCode AsCountryCode() => _Value;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TerminalCountryCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static TerminalCountryCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new TerminalCountryCode(new NumericCountryCode(result));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(TerminalCountryCode? x, TerminalCountryCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalCountryCode obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator NumericCountryCode(TerminalCountryCode value) => value._Value;
    public static bool operator ==(TerminalCountryCode left, NumericCountryCode right) => left._Value.Equals(right);
    public static bool operator ==(NumericCountryCode left, TerminalCountryCode right) => right._Value.Equals(left);
    public static bool operator !=(TerminalCountryCode left, NumericCountryCode right) => !left._Value.Equals(right);
    public static bool operator !=(NumericCountryCode left, TerminalCountryCode right) => !right._Value.Equals(left);

    #endregion
}