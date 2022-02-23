using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements.Emv;

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

    public Money AsMoney(CultureProfile cultureProfile) => new(_Value, cultureProfile);
    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TerminalFloorLimit Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalFloorLimit Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalFloorLimit)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(EncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalFloorLimit)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new TerminalFloorLimit(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

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