using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
/// </summary>
public record DcAcType : DataElement<byte>, IEqualityComparer<DcAcType>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0xDF8108;

    #endregion

    #region Constructor

    public DcAcType(byte value) : base(value)
    {
        if (!AcType.IsValid(value))
            throw new ArgumentException($"The argument {nameof(value)} was not recognized as a valid {nameof(AcType)}");
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DcAcType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DcAcType Decode(ReadOnlySpan<byte> value) => new(value[0]);

    #endregion

    #region Equality

    public bool Equals(DcAcType? x, DcAcType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DcAcType obj) => obj.GetHashCode();

    #endregion
}