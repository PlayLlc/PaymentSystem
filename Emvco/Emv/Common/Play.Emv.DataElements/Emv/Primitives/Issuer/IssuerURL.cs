using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     The URL provides the location of the Issuer�s Library Server on the Internet.
/// </summary>
public record IssuerUrl : DataElement<char[]>, IEqualityComparer<IssuerUrl>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x5F50;
    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;

    #endregion

    #region Constructor

    public IssuerUrl(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public static IssuerUrl Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerUrl Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<char[]>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerUrl)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new IssuerUrl(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerUrl? x, IssuerUrl? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerUrl obj) => obj.GetHashCode();

    #endregion
}