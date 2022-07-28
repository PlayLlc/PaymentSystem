using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

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

    public override ushort GetValueByteCount(BerCodec codec) => (ushort)PlayCodec.AlphaNumericSpecialCodec.GetByteCount(_Value);

    public override ushort GetValueByteCount() => (ushort) PlayCodec.AlphaNumericSpecialCodec.GetByteCount(_Value);

    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public static IssuerUrl Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override IssuerUrl Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerUrl Decode(ReadOnlySpan<byte> value)
    {
        char[] result = PlayCodec.AlphaNumericSpecialCodec.DecodeToChars(value);

        return new IssuerUrl(result);
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