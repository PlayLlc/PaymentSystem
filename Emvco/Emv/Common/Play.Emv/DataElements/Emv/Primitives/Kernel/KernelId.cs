using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Contains a value that uniquely identifies each Kernel. There is one occurrence of this data object for
///     each Kernel in the Reader.
/// </summary>
public record KernelId : DataElement<byte>, IEqualityComparer<KernelId>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF810C;
    public static readonly KernelId Unavailable = new KernelId(0);

    #endregion

    #region Constructor

    public KernelId(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ShortKernelIdTypes GetShortKernelId() => ShortKernelIdTypes.Get(_Value);
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization
     



    private const byte _ByteLength = 1;

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static KernelId Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static KernelId Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);
 
        return new KernelId(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();
    #endregion

    #region Equality

    public bool Equals(KernelId? x, KernelId? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(KernelId obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(ShortKernelIdTypes left, KernelId right) => left.Equals(right);
    public static bool operator ==(KernelId left, ShortKernelIdTypes right) => right.Equals(left);
    public static explicit operator byte(KernelId value) => value._Value;
    public static bool operator !=(ShortKernelIdTypes left, KernelId right) => !left.Equals(right);
    public static bool operator !=(KernelId left, ShortKernelIdTypes right) => !right.Equals(left);

    #endregion
}