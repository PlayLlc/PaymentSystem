using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Designates the unique location of a terminal at a merchant
/// </summary>
public record TerminalIdentification : DataElement<ulong>, IEqualityComparer<TerminalIdentification>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F1C;
    private const byte _CharLength = 8;

    #endregion

    #region Constructor

    public TerminalIdentification(ulong value) : base(value)
    {
        if (value.GetNumberOfDigits() > 8)
            throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must have 8 digits or less");
    }

    #endregion

    #region Instance Members

    public override string ToString() => AsToken();
    public Span<char> AsSpan() => _Value.AsSpanFromRight(_CharLength);
    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public string AsToken() => _Value.AsStringFromRight(_CharLength);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TerminalIdentification Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalIdentification Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const byte byteLength = 8;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalIdentification)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ulong> result = codec.Decode(PlayEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalIdentification)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        if (result.CharCount != _CharLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalIdentification)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {_CharLength} bytes in length");
        }

        return new TerminalIdentification(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(PlayEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(PlayEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TerminalIdentification? x, TerminalIdentification? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(InterfaceDeviceSerialNumber interfaceDeviceSerialNumber) => (ulong) interfaceDeviceSerialNumber == _Value;
    public int GetHashCode(TerminalIdentification obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(InterfaceDeviceSerialNumber left, TerminalIdentification right) => left.Equals(right);
    public static bool operator !=(InterfaceDeviceSerialNumber left, TerminalIdentification right) => !left.Equals(right);
    public static bool operator ==(TerminalIdentification left, InterfaceDeviceSerialNumber right) => right.Equals(left);
    public static bool operator !=(TerminalIdentification left, InterfaceDeviceSerialNumber right) => !right.Equals(left);
    public static explicit operator Span<char>(TerminalIdentification value) => value.AsSpan();
    public static explicit operator ulong(TerminalIdentification value) => value._Value;
    public static explicit operator ReadOnlySpan<char>(TerminalIdentification value) => value.AsSpan();
    public static explicit operator string(TerminalIdentification value) => value.AsToken();

    #endregion
}