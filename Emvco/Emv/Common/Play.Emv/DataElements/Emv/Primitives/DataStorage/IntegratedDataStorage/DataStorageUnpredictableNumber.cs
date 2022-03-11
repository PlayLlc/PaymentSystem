using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

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

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageUnpredictableNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageUnpredictableNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(EncodingId, value) as DecodedResult<uint>
            ?? throw new DataElementParsingException(
                $"The {nameof(DataStorageSummary1)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new DataStorageUnpredictableNumber(result.Value);
    }

    #endregion
}