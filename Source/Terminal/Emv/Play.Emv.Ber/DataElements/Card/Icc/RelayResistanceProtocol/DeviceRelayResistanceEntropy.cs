﻿using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Random number returned by the Card in the response to the EXCHANGE RELAY RESISTANCE DATA command.
/// </summary>
public record DeviceRelayResistanceEntropy : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8302;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public DeviceRelayResistanceEntropy(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DeviceRelayResistanceEntropy Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DeviceRelayResistanceEntropy Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DeviceRelayResistanceEntropy Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new DeviceRelayResistanceEntropy(new RelaySeconds(result));
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode((uint) _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode((uint) _Value, length);

    #endregion

    #region Operator Overrides

    public static explicit operator RelaySeconds(DeviceRelayResistanceEntropy value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount((uint) _Value);

    #endregion
}