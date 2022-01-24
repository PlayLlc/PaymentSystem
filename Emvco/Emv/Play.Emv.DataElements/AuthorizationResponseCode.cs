using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Code that defines the disposition of a message
/// </summary>
public record AuthorizationResponseCode : DataElement<ushort>, IEqualityComparer<AuthorizationResponseCode>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x8A;
    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;

    #endregion

    #region Constructor

    public AuthorizationResponseCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;

    #endregion

    #region Serialization

    public static AuthorizationResponseCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static AuthorizationResponseCode Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 2;
        const ushort charLength = 2;

        if (value.Length != byteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(AuthorizationResponseCode)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ushort> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ushort>
            ?? throw new
                InvalidOperationException($"The {nameof(AuthorizationResponseCode)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        if (result.CharCount != charLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(AuthorizationResponseCode)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new AuthorizationResponseCode(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(AuthorizationResponseCode? x, AuthorizationResponseCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AuthorizationResponseCode obj) => obj.GetHashCode();

    #endregion
}