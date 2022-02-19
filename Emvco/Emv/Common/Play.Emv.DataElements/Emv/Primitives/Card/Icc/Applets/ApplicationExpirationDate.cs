using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Lists a number of card features beyond regular payment.
/// </summary>
public record ApplicationExpirationDate : DataElement<uint>, IEqualityComparer<ApplicationExpirationDate>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x5F24;
    public static readonly BerEncodingId BerEncodingId = NumericDataElementCodec.Identifier;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ApplicationExpirationDate(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool CombinedDataAuthenticationIndicator() => _Value.IsBitSet(9);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;

    public SdsSchemeIndicator GetSdsSchemeIndicator()
    {
        const byte bitOffset = 1;

        return SdsSchemeIndicator.Get((byte) (_Value >> bitOffset));
    }

    public override Tag GetTag() => Tag;
    public bool SupportForBalanceReading() => _Value.IsBitSet(10);
    public bool SupportForFieldOffDetection() => _Value.IsBitSet(11);

    #endregion

    #region Serialization

    public static ApplicationExpirationDate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationExpirationDate Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationExpirationDate)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationExpirationDate)} could not be initialized because the {nameof(NumericDataElementCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new ApplicationExpirationDate(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(GetBerEncodingId(), _Value, _ByteLength);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationExpirationDate? x, ApplicationExpirationDate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationExpirationDate obj) => obj.GetHashCode();

    #endregion
}