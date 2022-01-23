using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.DataElements;

/// <summary>
///     ATC value of the last transaction that went online
/// </summary>
public record LastOnlineApplicationTransactionCounterRegister : DataElement<ushort>,
    IEqualityComparer<LastOnlineApplicationTransactionCounterRegister>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F13;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public LastOnlineApplicationTransactionCounterRegister(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public new byte[] Encode() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

    #endregion

    #region Serialization

    public static LastOnlineApplicationTransactionCounterRegister Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static LastOnlineApplicationTransactionCounterRegister Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(LastOnlineApplicationTransactionCounterRegister)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<ushort> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ushort>
            ?? throw new InvalidOperationException(
                $"The {nameof(LastOnlineApplicationTransactionCounterRegister)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new LastOnlineApplicationTransactionCounterRegister(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => Encode();

    #endregion

    #region Equality

    public bool Equals(LastOnlineApplicationTransactionCounterRegister? x, LastOnlineApplicationTransactionCounterRegister? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(LastOnlineApplicationTransactionCounterRegister obj) => obj.GetHashCode();

    #endregion
}