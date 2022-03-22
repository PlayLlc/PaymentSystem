﻿using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the result of OWHF2(DS Input (Term)) or OWHF2AES(DS Input (Term)), if DS Input (Term) is provided by the
///     Terminal. This data object is to be supplied to the Card with the GENERATE AC command, as per DSDOL formatting.
/// </summary>
public record DataStorageDigestHash : DataElement<ulong>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF61;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public DataStorageDigestHash(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exception"></exception>
    public static DataStorageDigestHash Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result()
            ?? throw new DataElementParsingException(EncodingId);

        return new DataStorageDigestHash(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageDigestHash Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageDigestHash Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    #endregion
}