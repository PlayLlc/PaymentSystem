using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the card data input capability of the Terminal and Reader.
/// </summary>
public record SecurityCapability : DataElement<byte>, IEqualityComparer<SecurityCapability>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0xDF811F;

    #endregion

    #region Constructor

    public SecurityCapability(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static SecurityCapability Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static SecurityCapability Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<byte> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(SecurityCapability)} could not be initialized because the {nameof(NumericDataElementCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new SecurityCapability(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(SecurityCapability? x, SecurityCapability? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(SecurityCapability obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(SecurityCapability value) => value._Value;

    #endregion
}