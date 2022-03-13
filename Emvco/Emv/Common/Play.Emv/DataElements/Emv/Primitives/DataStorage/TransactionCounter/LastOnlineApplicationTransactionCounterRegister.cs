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
///     ATC value of the last transaction that went online
/// </summary>
public record LastOnlineApplicationTransactionCounterRegister : DataElement<ushort>,
    IEqualityComparer<LastOnlineApplicationTransactionCounterRegister>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F13;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public LastOnlineApplicationTransactionCounterRegister(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public byte[] Encode() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Serialization
     


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static LastOnlineApplicationTransactionCounterRegister Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static LastOnlineApplicationTransactionCounterRegister Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

       
        return new LastOnlineApplicationTransactionCounterRegister(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(LastOnlineApplicationTransactionCounterRegister? x, LastOnlineApplicationTransactionCounterRegister? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(LastOnlineApplicationTransactionCounterRegister obj) => obj.GetHashCode();

    #endregion
}