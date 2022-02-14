﻿using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains the result of OWHF2(DS Input (Term)) or OWHF2AES(DS Input (Term)), if DS Input (Term) is provided by the
///     Terminal. This data object is to be supplied to the Card with the GENERATE AC command, as per DSDOL formatting.
/// </summary>
public record DataStorageDigestHash : DataElement<ulong>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0xDF61;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public DataStorageDigestHash(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageDigestHash Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value).ToUInt64Result()
            ?? throw new DataElementNullException(BerEncodingId);

        return new DataStorageDigestHash(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataStorageDigestHash Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}