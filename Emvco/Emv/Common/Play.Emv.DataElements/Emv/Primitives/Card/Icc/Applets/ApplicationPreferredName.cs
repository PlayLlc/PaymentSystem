using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Preferred mnemonic associated with the AID
/// </summary>
public record ApplicationPreferredName : DataElement<char[]>, IEqualityComparer<ApplicationPreferredName>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F12;
    public static readonly BerEncodingId BerEncodingId = AlphaNumericSpecialDataElementCodec.Identifier;

    #endregion

    #region Constructor

    public ApplicationPreferredName(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    public static bool StaticEquals(ApplicationPreferredName? x, ApplicationPreferredName? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    public static ApplicationPreferredName Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationPreferredName Decode(ReadOnlySpan<byte> value)
    {
        const ushort minByteLength = 1;
        const ushort maxByteLength = 16;

        if (value.Length is not >= minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationPreferredName)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<char[]> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<char[]>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationPreferredName)} could not be initialized because the {nameof(AlphaNumericSpecialDataElementCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new ApplicationPreferredName(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationPreferredName? x, ApplicationPreferredName? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationPreferredName obj) => obj.GetHashCode();

    #endregion
}