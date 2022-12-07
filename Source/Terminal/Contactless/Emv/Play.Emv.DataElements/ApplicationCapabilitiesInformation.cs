using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Lists a number of card features beyond regular payment.
/// </summary>
public record ApplicationCapabilitiesInformation : DataElement<uint>, IEqualityComparer<ApplicationCapabilitiesInformation>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F5D;
    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ApplicationCapabilitiesInformation(uint value) : base(value)
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
    public bool IsSupportForFieldOffDetectionSet() => _Value.IsBitSet(11);
    public new byte[] Encode() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

    #endregion

    #region Serialization

    public static ApplicationCapabilitiesInformation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationCapabilitiesInformation Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationCapabilitiesInformation)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationCapabilitiesInformation)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new ApplicationCapabilitiesInformation(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => Encode();

    #endregion

    #region Equality

    public bool Equals(ApplicationCapabilitiesInformation? x, ApplicationCapabilitiesInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationCapabilitiesInformation obj) => obj.GetHashCode();

    #endregion
}