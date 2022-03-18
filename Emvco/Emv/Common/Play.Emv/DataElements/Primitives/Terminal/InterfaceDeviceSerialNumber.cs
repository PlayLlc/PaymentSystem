using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Unique and permanent serial number assigned to the Interface Device by the manufacturer. In other words it's the
///     serial number of the reader device
/// </summary>
public record InterfaceDeviceSerialNumber : DataElement<char[]>, IEqualityComparer<InterfaceDeviceSerialNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F1E;
    private const byte _ByteLength = 8;

    #endregion

    public static explicit operator ReadOnlySpan<char>(InterfaceDeviceSerialNumber value) => value._Value.AsSpan();
    #region Constructor

    public InterfaceDeviceSerialNumber(char[] value) : base(value)
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
    /// <exception cref="BerParsingException"></exception>
    public static InterfaceDeviceSerialNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        char[] result = PlayCodec.AlphaNumericCodec.DecodeToChars(value);

        return new InterfaceDeviceSerialNumber(result);
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
     

    #endregion
}