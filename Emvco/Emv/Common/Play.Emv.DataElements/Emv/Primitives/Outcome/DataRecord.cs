using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

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

    public DataRecord(params TagLengthValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static DataRecord Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodeTagLengthValues(value));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataRecord Decode(ReadOnlyMemory<byte> value) => new(_Codec.DecodeTagLengthValues(value));

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