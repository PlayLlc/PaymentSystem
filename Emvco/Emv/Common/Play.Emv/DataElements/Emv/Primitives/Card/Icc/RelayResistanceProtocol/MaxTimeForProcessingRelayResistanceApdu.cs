using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv.ValueTypes;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record MaxTimeForProcessingRelayResistanceApdu : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8304;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MaxTimeForProcessingRelayResistanceApdu(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MaxTimeForProcessingRelayResistanceApdu Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MaxTimeForProcessingRelayResistanceApdu Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MaxTimeForProcessingRelayResistanceApdu(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Operator Overrides

    public static explicit operator RelaySeconds(MaxTimeForProcessingRelayResistanceApdu value) => value._Value;
    public static explicit operator ushort(MaxTimeForProcessingRelayResistanceApdu value) => value._Value;

    #endregion
}