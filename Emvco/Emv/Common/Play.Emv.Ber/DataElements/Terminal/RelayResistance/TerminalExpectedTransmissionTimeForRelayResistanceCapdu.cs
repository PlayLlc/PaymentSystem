using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Represents the time that the Kernel expects to need for transmitting the EXCHANGE RELAY RESISTANCE DATA command to
///     the Card. The Terminal Expected Transmission Time For Relay Resistance C-APDU is expressed in units of hundreds of
///     microseconds.
/// </summary>
public record TerminalExpectedTransmissionTimeForRelayResistanceCapdu : DataElement<ushort>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8134;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public TerminalExpectedTransmissionTimeForRelayResistanceCapdu(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TerminalExpectedTransmissionTimeForRelayResistanceCapdu Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TerminalExpectedTransmissionTimeForRelayResistanceCapdu Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new TerminalExpectedTransmissionTimeForRelayResistanceCapdu(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Operator Overrides

    public static implicit operator RelaySeconds(TerminalExpectedTransmissionTimeForRelayResistanceCapdu value) => value._Value;

    #endregion
}