﻿using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
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

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static DataStorageRequestedOperatorId Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result() ?? throw new DataObjectParsingException(EncodingId);

        return new DataStorageRequestedOperatorId(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageRequestedOperatorId Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}