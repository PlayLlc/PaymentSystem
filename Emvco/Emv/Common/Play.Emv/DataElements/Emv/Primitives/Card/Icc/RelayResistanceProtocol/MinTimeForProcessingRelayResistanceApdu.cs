using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv.ValueTypes;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the minimum estimated time the Card requires for processing the EXCHANGE RELAY RESISTANCE DATA command.
///     The Min Time For Processing Relay Resistance APDU is expressed in units of hundreds of microseconds.
/// </summary>
public record MinTimeForProcessingRelayResistanceApdu : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8303;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MinTimeForProcessingRelayResistanceApdu(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MinTimeForProcessingRelayResistanceApdu Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MinTimeForProcessingRelayResistanceApdu Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MinTimeForProcessingRelayResistanceApdu(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Operator Overrides

    public static explicit operator RelaySeconds(MinTimeForProcessingRelayResistanceApdu value) => value._Value;

    #endregion
}