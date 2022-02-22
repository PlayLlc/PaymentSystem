using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Specifies the issuer's conditions that cause the denial of a transaction without attempt to go online
/// </summary>
public record IssuerActionCodeDenial : DataElement<ulong>, IEqualityComparer<IssuerActionCodeDenial>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F0E;

    #endregion

    #region Constructor

    public IssuerActionCodeDenial(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ActionCodes AsActionCodes() => new(_Value);
    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerActionCodeDenial Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerActionCodeDenial Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 5;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerActionCodeDenial)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ulong> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerActionCodeDenial)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new IssuerActionCodeDenial(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerActionCodeDenial? x, IssuerActionCodeDenial? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerActionCodeDenial obj) => obj.GetHashCode();

    #endregion
}