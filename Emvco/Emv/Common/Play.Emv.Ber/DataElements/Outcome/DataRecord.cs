using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: The Data Record is a list of TLV encoded data objects returned with the Outcome Parameter Set on the
///     completion of transaction processing.
/// </summary>
public record DataRecord : DataExchangeResponse, IEqualityComparer<DataRecord>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8105;

    #endregion

    #region Constructor

    public DataRecord(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    public static DataRecord Decode(ReadOnlySpan<byte> value) =>
        new(_Codec.DecodePrimitiveValuesAtRuntime(value).ToArray());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataRecord Decode(ReadOnlyMemory<byte> value) => new(_Codec.DecodePrimitiveValuesAtRuntime(value.Span).ToArray());

    #endregion

    #region Equality

    public bool Equals(DataRecord? x, DataRecord? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataRecord obj) => obj.GetHashCode();

    #endregion
}