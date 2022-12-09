﻿using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains a Kernel challenge (random) to be used in the value field of the EXCHANGE RELAY RESISTANCE DATA command.
/// </summary>
public record TerminalRelayResistanceEntropy : DataElement<uint>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8301;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public TerminalRelayResistanceEntropy(uint value) : base(value)
    { }

    public TerminalRelayResistanceEntropy(UnpredictableNumber unpredictableNumber) : base((uint) unpredictableNumber)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalRelayResistanceEntropy Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalRelayResistanceEntropy Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalRelayResistanceEntropy Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new TerminalRelayResistanceEntropy(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}