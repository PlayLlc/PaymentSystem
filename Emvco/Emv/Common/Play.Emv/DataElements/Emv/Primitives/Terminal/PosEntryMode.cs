﻿using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
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

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="DataObjectParsingException"></exception>
    /// <exception cref="Play.Ber.Exceptions._Temp.BerFormatException"></exception>
    public static PosEntryMode Decode(ReadOnlyMemory<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<byte>
            ?? throw new DataObjectParsingException(EncodingId);

        return new PosEntryMode(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataObjectParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PosEntryMode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value).ToByteResult() ?? throw new DataObjectParsingException(EncodingId);

        return new PosEntryMode(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

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