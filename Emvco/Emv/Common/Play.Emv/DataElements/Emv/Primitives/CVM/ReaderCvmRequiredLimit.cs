using System;
using System.Collections.Generic;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the transaction amount above which the Kernel instantiates the CVM capabilities field in Terminal
///     Capabilities with CVM Capability – CVM Required.
/// </summary>
public record ReaderCvmRequiredLimit : DataElement<ulong>, IEqualityComparer<ReaderCvmRequiredLimit>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F74;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 6;
    private const byte _CharLength = 12;

    #endregion

    #region Constructor

    public ReaderCvmRequiredLimit(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Money AsMoney(CultureProfile cultureProfile) => new(_Value, cultureProfile);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public static ReaderCvmRequiredLimit Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static ReaderCvmRequiredLimit Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result() ?? throw new DataObjectParsingException(EncodingId);

        Check.Primitive.ForCharLength(result.CharCount, _CharLength, Tag);

        return new ReaderCvmRequiredLimit(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

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
}