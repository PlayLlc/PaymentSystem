using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Unique and permanent serial number assigned to the Interface Device by the manufacturer. In other words it's the
///     serial number of the reader device
/// </summary>
public record InterfaceDeviceSerialNumber : DataElement<ulong>, IEqualityComparer<InterfaceDeviceSerialNumber>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = Numeric.Identifier;
    public static readonly Tag Tag = 0x9F1E;

    #endregion

    #region Constructor

    public InterfaceDeviceSerialNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(BerEncodingId, _Value);

    #endregion

    #region Serialization

    public static InterfaceDeviceSerialNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static InterfaceDeviceSerialNumber Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 8;
        const ushort charLength = 8;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(InterfaceDeviceSerialNumber)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(InterfaceDeviceSerialNumber)} could not be initialized because the {nameof(Numeric)} returned a null {nameof(DecodedResult<ulong>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(InterfaceDeviceSerialNumber)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new InterfaceDeviceSerialNumber(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(InterfaceDeviceSerialNumber? x, InterfaceDeviceSerialNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(InterfaceDeviceSerialNumber obj) => obj.GetHashCode();

    #endregion
}