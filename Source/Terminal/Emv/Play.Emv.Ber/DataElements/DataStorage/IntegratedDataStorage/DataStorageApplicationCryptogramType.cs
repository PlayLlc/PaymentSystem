﻿using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the AC type indicated by the Terminal for which Integrated Data Storage data must be stored in the Card.
/// </summary>
public record DataStorageApplicationCryptogramType : DataElement<byte>, IEqualityComparer<DataStorageApplicationCryptogramType>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8108;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    /// <exception cref="CardDataException"></exception>
    public DataStorageApplicationCryptogramType(byte value) : base(value)
    {
        if (!CryptogramTypes.IsValid(value))
            throw new CardDataException($"The argument {nameof(value)} was not recognized as a valid {nameof(CryptogramTypes)}");
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageApplicationCryptogramType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageApplicationCryptogramType Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageApplicationCryptogramType Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        if (!CryptogramTypes.IsValid(result))
            throw new CardDataException($"The argument {nameof(value)} was not recognized as a valid {nameof(CryptogramTypes)}");

        return new DataStorageApplicationCryptogramType(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(DataStorageApplicationCryptogramType? x, DataStorageApplicationCryptogramType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataStorageApplicationCryptogramType obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator CryptogramType(DataStorageApplicationCryptogramType cryptogramTypes) => new(cryptogramTypes._Value);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}