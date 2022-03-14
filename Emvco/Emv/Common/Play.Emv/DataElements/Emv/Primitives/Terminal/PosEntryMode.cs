using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record PosEntryMode : DataElement<byte>, IEqualityComparer<PosEntryMode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F39;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public PosEntryMode(byte value) : base(value)
    { }

    public PosEntryMode(PosEntryModeTypes value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static PosEntryMode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="CodecParsingException"></exception>
    public static PosEntryMode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.NumericCodec.DecodeToByte(value);

        return new PosEntryMode(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(PosEntryMode? x, PosEntryMode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PosEntryMode obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(PosEntryMode value) => value._Value;

    #endregion
}