using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains the Card indication, obtained in the response to the GET PROCESSING OPTIONS command, about either the
///     stored summary associated with DS ODS Card if present, or about a default zero-filled summary if DS ODS Card is not
///     present and DS Unpredictable Number is present
/// </summary>
public record DataStorageSummary1 : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F7D;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MinByteLength = 8;
    private const byte _MaxByteLength = 16;

    #endregion

    #region Constructor

    public DataStorageSummary1(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageSummary1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageSummary1 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new DataElementParsingException(
                $"The {nameof(DataStorageSummary1)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new DataStorageSummary1(result.Value);
    }

    #endregion
}