using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Designates the unique location of a terminal at a merchant
/// </summary>
public record TerminalIdentification : DataElement<char[]>, IEqualityComparer<TerminalIdentification>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F1C;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public TerminalIdentification(ReadOnlySpan<char> value) : base(value.ToArray())
    {
        if (value.Length > 8)
            throw new DataElementParsingException($"The argument {nameof(value)} must have 8 digits or less");
    }

    #endregion

    #region Instance Members

    public override string ToString() => AsToken();
    public Span<char> AsSpan() => _Value.AsSpan()[^_ByteLength..];
    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public string AsToken() => new(_Value.AsSpan()[^_ByteLength..]);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalIdentification Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalIdentification Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalIdentification Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ReadOnlySpan<char> result = PlayCodec.AlphaNumericCodec.DecodeToChars(value);

        return new TerminalIdentification(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

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

    public bool Equals(InterfaceDeviceSerialNumber interfaceDeviceSerialNumber) =>
        ((ReadOnlySpan<char>) interfaceDeviceSerialNumber).IsValueEqual((ReadOnlySpan<char>) _Value);

    public int GetHashCode(TerminalIdentification obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(InterfaceDeviceSerialNumber left, TerminalIdentification right) => left.Equals(right);
    public static bool operator !=(InterfaceDeviceSerialNumber left, TerminalIdentification right) => !left.Equals(right);
    public static bool operator ==(TerminalIdentification left, InterfaceDeviceSerialNumber right) => right.Equals(left);
    public static bool operator !=(TerminalIdentification left, InterfaceDeviceSerialNumber right) => !right.Equals(left);
    public static explicit operator Span<char>(TerminalIdentification value) => value.AsSpan();
    public static explicit operator ReadOnlySpan<char>(TerminalIdentification value) => value.AsSpan();
    public static explicit operator string(TerminalIdentification value) => value.AsToken();

    #endregion
}