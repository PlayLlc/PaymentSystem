using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Globalization;
using Play.Globalization.Country;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the country of the terminal, represented according to ISO 3166
/// </summary>
public record TerminalCountryCode : DataElement<NumericCountryCode>, IEqualityComparer<TerminalCountryCode>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = NumericCodec.Identifier;
    public static readonly TerminalCountryCode Default = new(new NumericCountryCode(0));
    public static readonly Tag Tag = 0x9F1A;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public TerminalCountryCode(NumericCountryCode value) : base(value)
    { }

    public TerminalCountryCode(CultureProfile value) : base(value.GetNumericCountryCode())
    { }

    #endregion

    #region Instance Members

    public NumericCountryCode AsCountryCode() => _Value;
    public override PlayEncodingId GetBerEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TerminalCountryCode Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalCountryCode Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort charLength = 3;

        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalCountryCode)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<ushort> result = codec.Decode(PlayEncodingId, value) as DecodedResult<ushort>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalCountryCode)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalCountryCode)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new TerminalCountryCode(new NumericCountryCode(result.Value));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(PlayEncodingId, _Value, _ByteLength);

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

    public static bool operator ==(TerminalCountryCode left, NumericCountryCode right) => left._Value.Equals(right);
    public static bool operator ==(NumericCountryCode left, TerminalCountryCode right) => right._Value.Equals(left);
    public static bool operator !=(TerminalCountryCode left, NumericCountryCode right) => !left._Value.Equals(right);
    public static bool operator !=(NumericCountryCode left, TerminalCountryCode right) => !right._Value.Equals(left);

    #endregion
}