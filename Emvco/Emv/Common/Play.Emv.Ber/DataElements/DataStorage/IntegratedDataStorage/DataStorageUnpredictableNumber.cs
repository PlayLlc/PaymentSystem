﻿using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the Card challenge (random), obtained in the response to the GET PROCESSING OPTIONS command, to be used by
///     the Terminal in the summary calculation when providing DS ODS Term.
/// </summary>
public record DataStorageUnpredictableNumber : DataElement<uint>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F7F;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public DataStorageUnpredictableNumber(uint value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageUnpredictableNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageUnpredictableNumber Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageUnpredictableNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new DataStorageUnpredictableNumber(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}