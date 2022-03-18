﻿using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains the Terminal determined operator identifier for data  storage. It is sent to the Card in the GET
///     PROCESSING  OPTIONS command.
/// </summary>
public record DataStorageRequestedOperatorId : DataElement<ulong>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F5C;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public DataStorageRequestedOperatorId(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static DataStorageRequestedOperatorId Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new DataStorageRequestedOperatorId(result);
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static DataStorageRequestedOperatorId Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}