﻿using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains Terminal provided data to be forwarded to the Card with the GENERATE AC command, as per DSDOL formatting.
/// </summary>
public record DataStorageOperatorDataSetTerminal : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF63;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MaxByteLength = 160;

    #endregion

    #region Constructor

    public DataStorageOperatorDataSetTerminal(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetTerminal Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageOperatorDataSetTerminal Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetTerminal Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new DataStorageOperatorDataSetTerminal(result);
    }

    #endregion
}