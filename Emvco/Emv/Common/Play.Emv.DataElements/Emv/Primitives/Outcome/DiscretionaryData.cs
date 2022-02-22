using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: The Discretionary Data is a list of Kernel-specific data objects sent to the Terminal as a separate
///     field in the OUT DataExchangeSignal.
/// </summary>
public record DiscretionaryData : DataExchangeResponse, IEqualityComparer<DiscretionaryData>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8106;

    #endregion

    #region Constructor

    public DiscretionaryData(params TagLengthValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static DiscretionaryData Decode(ReadOnlyMemory<byte> value) => new(_Codec.DecodeTagLengthValues(value));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DiscretionaryData Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodeTagLengthValues(value));

    #endregion

    #region Equality

    public bool Equals(DiscretionaryData? x, DiscretionaryData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DiscretionaryData obj) => obj.GetHashCode();

    #endregion
}