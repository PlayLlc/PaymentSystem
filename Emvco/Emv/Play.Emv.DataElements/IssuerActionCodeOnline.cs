using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.DataElements;

/// <summary>
///     Specifies the issuer's conditions that cause a transaction to be transmitted online
/// </summary>
public record IssuerActionCodeOnline : DataElement<ulong>, IEqualityComparer<IssuerActionCodeOnline>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F0F;

    #endregion

    #region Constructor

    public IssuerActionCodeOnline(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ActionCodes AsActionCodes() => new(_Value);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerActionCodeOnline Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerActionCodeOnline Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 5;

        if (value.Length != byteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(IssuerActionCodeOnline)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ulong> result = codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new
                InvalidOperationException($"The {nameof(IssuerActionCodeOnline)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new IssuerActionCodeOnline(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerActionCodeOnline? x, IssuerActionCodeOnline? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerActionCodeOnline obj) => obj.GetHashCode();

    #endregion
}