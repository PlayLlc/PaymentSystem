using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the priority of a given application or group of applications in a directory
/// </summary>
public record ApplicationPriorityIndicator : DataElement<byte>, IEqualityComparer<ApplicationPriorityIndicator>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x87;

    #endregion

    #region Constructor

    public ApplicationPriorityIndicator(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool ApplicationCannotBeSelectedWithoutConfirmationByTheCardholder() => _Value.IsBitSet(Bits.Eight);
    public ApplicationPriorityRank GetApplicationPriorityRank() => ApplicationPriorityRankTypes.Get(_Value);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    public static bool StaticEquals(ApplicationPriorityIndicator? x, ApplicationPriorityIndicator? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    public static ApplicationPriorityIndicator Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationPriorityIndicator Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 1;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationPriorityIndicator)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationPriorityIndicator)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new ApplicationPriorityIndicator(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(ApplicationPriorityIndicator? x, ApplicationPriorityIndicator? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationPriorityIndicator obj) => obj.GetHashCode();

    #endregion
}