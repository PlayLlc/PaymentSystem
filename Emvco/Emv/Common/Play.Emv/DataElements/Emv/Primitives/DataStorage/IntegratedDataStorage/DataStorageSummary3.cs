using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     This data allows the Kernel to check whether the Card has seen the same transaction data as were sent by the
///     Terminal/Kernel. It is located in the ICC Dynamic Data recovered from the Signed Dynamic Application Data.
/// </summary>
public record DataStorageSummary3 : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8102;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MinByteLength = 8;
    private const byte _MaxByteLength = 16;

    #endregion

    #region Constructor

    public DataStorageSummary3(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageSummary3 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageSummary3 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new DataElementParsingException(
                $"The {nameof(DataStorageSummary3)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new DataStorageSummary3(result.Value);
    }

    #endregion
}