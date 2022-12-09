﻿using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the transaction amount above which the Kernel instantiates the CVM capabilities field in Terminal
///     Capabilities with CVM Capability – CVM Required.
/// </summary>
public record ReaderCvmRequiredLimit : DataElement<ulong>, IEqualityComparer<ReaderCvmRequiredLimit>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8126;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public ReaderCvmRequiredLimit(ulong value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static ReaderCvmRequiredLimit Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ReaderCvmRequiredLimit Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static ReaderCvmRequiredLimit Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        return new ReaderCvmRequiredLimit(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(ReaderCvmRequiredLimit? x, ReaderCvmRequiredLimit? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ReaderCvmRequiredLimit obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public Money AsMoney(NumericCurrencyCode currencyCode) => new(_Value, currencyCode);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    public override ushort GetValueByteCount() => PlayCodec.NumericCodec.GetByteCount(_Value);

    #endregion
}