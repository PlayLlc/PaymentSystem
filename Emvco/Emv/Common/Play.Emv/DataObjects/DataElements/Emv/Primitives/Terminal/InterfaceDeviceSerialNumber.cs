using Play.Codecs;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Unique and permanent serial number assigned to the Interface Device by the manufacturer. In other words it's the
///     serial number of the reader device
/// </summary>
public record InterfaceDeviceSerialNumber : DataElement<ulong>, IEqualityComparer<InterfaceDeviceSerialNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F1E;

    #endregion

    #region Constructor

    public InterfaceDeviceSerialNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(EncodingId, _Value);

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

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(InterfaceDeviceSerialNumber)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(InterfaceDeviceSerialNumber)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new InterfaceDeviceSerialNumber(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

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

    #region Operator Overrides

    public static explicit operator ulong(InterfaceDeviceSerialNumber value) => value._Value;

    #endregion
}