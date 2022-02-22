using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the code table according to ISO/IEC 8859 for displaying the Application Preferred Name
/// </summary>
public record IssuerCodeTableIndex : DataElement<byte>, IEqualityComparer<IssuerCodeTableIndex>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = NumericCodec.Identifier;
    public static readonly Tag Tag = 0x9F11;

    #endregion

    #region Constructor

    public IssuerCodeTableIndex(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetBerEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;

    public static bool StaticEquals(IssuerCodeTableIndex? x, IssuerCodeTableIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    public static IssuerCodeTableIndex Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerCodeTableIndex Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 1;
        const ushort charLength = 2;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerCodeTableIndex)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerCodeTableIndex)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<byte>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerCodeTableIndex)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new IssuerCodeTableIndex(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerCodeTableIndex? x, IssuerCodeTableIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerCodeTableIndex obj) => obj.GetHashCode();

    #endregion
}