using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Specifies the acquirer's conditions that cause a transaction to be rejected on an offline only
///     Terminal.
/// </summary>
public record TerminalActionCodeDefault : DataElement<ulong>, IEqualityComparer<TerminalActionCodeDefault>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly TerminalActionCodeDefault Default = new(0x840000000C);
    public static readonly Tag Tag = 0xDF8120;
    private const byte _ByteLength = 5;

    #endregion

    #region Constructor

    public TerminalActionCodeDefault(ulong value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalActionCodeDefault Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalActionCodeDefault Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalActionCodeDefault Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new TerminalActionCodeDefault(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TerminalActionCodeDefault? x, TerminalActionCodeDefault? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalActionCodeDefault obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public ActionCodes AsActionCodes() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}