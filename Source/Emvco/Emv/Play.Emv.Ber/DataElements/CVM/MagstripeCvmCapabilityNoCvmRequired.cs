﻿using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public record MagstripeCvmCapabilityNoCvmRequired : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF812C;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public MagstripeCvmCapabilityNoCvmRequired(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MagstripeCvmCapabilityNoCvmRequired Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MagstripeCvmCapabilityNoCvmRequired Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MagstripeCvmCapabilityNoCvmRequired Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new MagstripeCvmCapabilityNoCvmRequired(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(MagstripeCvmCapabilityNoCvmRequired value) => value._Value;

    #endregion
}