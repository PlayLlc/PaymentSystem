using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Time-variant number generated by the ICC, to be captured by the terminal
/// </summary>
public record IccDynamicNumber : DataElement<ulong>, IEqualityComparer<IccDynamicNumber>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x9F4C;

    #endregion

    #region Constructor

    public IccDynamicNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static IccDynamicNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IccDynamicNumber Decode(ReadOnlySpan<byte> value)
    {
        const ushort minByteLength = 2;
        const ushort maxByteLength = 8;

        if (value.Length is < minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IccDynamicNumber)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(IccDynamicNumber)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new IccDynamicNumber(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IccDynamicNumber? x, IccDynamicNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccDynamicNumber obj) => obj.GetHashCode();

    #endregion
}