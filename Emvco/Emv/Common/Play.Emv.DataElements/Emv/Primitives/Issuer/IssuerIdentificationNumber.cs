using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     The number that identifies the major industry and the card issuer and that forms the first part of the Primary
///     Account Number (PAN)
/// </summary>
public record IssuerIdentificationNumber : DataElement<uint>, IEqualityComparer<IssuerIdentificationNumber>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    public static readonly Tag Tag = 0x42;

    #endregion

    #region Constructor

    public IssuerIdentificationNumber(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerIdentificationNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static IssuerIdentificationNumber Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 3;
        const ushort charLength = 6;

        Check.Primitive.ForExactLength(value, byteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value).ToUInt32Result()
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForCharLength(result.CharCount, charLength, Tag);

        return new IssuerIdentificationNumber(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerIdentificationNumber? x, IssuerIdentificationNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerIdentificationNumber obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(IssuerIdentificationNumber value) => value._Value;

    #endregion
}