using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: Combines all parameters to be sent with the OUT DataExchangeSignal or MSG DataExchangeSignal.
/// </summary>
public record UserInterfaceRequestData : DataElement<BigInteger>, IRetrievePrimitiveValueMetadata,
    IEqualityComparer<UserInterfaceRequestData>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8116;
    private const byte _MessageIdentifierOffset = 160;
    private const byte _StatusOffset = 152;
    private const byte _HoldTimeOffset = 128;
    private const byte _LanguagePreferenceOffset = 64;
    private const byte _ValueQualifierOffset = 56;
    private const byte _MoneyOffset = 8;
    private const byte _CurrencyCodeOffset = 0;

    #endregion

    #region Constructor

    public UserInterfaceRequestData(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ulong? GetAmount() =>
        GetValueQualifier() == ValueQualifier.None ? null : ((ulong) (_Value >> _MoneyOffset)).GetMaskedValue(0xFFFF000000000000);

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static Builder GetBuilder() => new();
    public Builder Update() => new(this);
    public NumericCurrencyCode GetCurrencyCode() => new((ushort) (_Value >> _CurrencyCodeOffset));

    public MessageHoldTime GetHoldTimeValue() =>
        new(new Milliseconds(((ulong) (_Value >> _HoldTimeOffset)).GetMaskedValue(0xFFFF000000000000) * 100));

    public LanguagePreference GetLanguagePreference() => new((ulong) (_Value >> _LanguagePreferenceOffset));
    public MessageIdentifier GetMessageIdentifier() => MessageIdentifier.Get((byte) (_Value >> _MessageIdentifierOffset));
    public Status GetStatus() => Status.Get((byte) (_Value >> _StatusOffset));
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public ValueQualifier GetValueQualifier() => ValueQualifier.Get((byte) (_Value >> _ValueQualifierOffset));

    #endregion

    #region Serialization

    public static UserInterfaceRequestData Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static UserInterfaceRequestData Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 22;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(UserInterfaceRequestData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(UserInterfaceRequestData)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new UserInterfaceRequestData(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(UserInterfaceRequestData? x, UserInterfaceRequestData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UserInterfaceRequestData obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static UserInterfaceRequestData operator |(UserInterfaceRequestData left, UserInterfaceRequestData right) =>
        new(left._Value | right._Value);

    #endregion

    // TODO: Well, obviously unit test i guess. initially the bit shifting was backwards
    public class Builder : PrimitiveValueBuilder<BigInteger>
    {
        #region Constructor

        internal Builder(UserInterfaceRequestData value)
        {
            _Value = value._Value;
        }

        internal Builder()
        {
            Set(MessageIdentifier.NotAvailable);
            Set(Status.NotAvailable);
            Set(MessageHoldTime.MinimumValue);

            //Set(LanguagePreference);
            Set(ValueQualifier.None);
        }

        #endregion

        #region Instance Members

        public void Set(MessageIdentifier bitsToSet)
        {
            _Value.ClearBits(byte.MaxValue << _MessageIdentifierOffset);
            _Value |= (BigInteger) bitsToSet << _MessageIdentifierOffset;
        }

        public void Set(Status bitsToSet)
        {
            _Value.ClearBits(byte.MaxValue << _StatusOffset);
            _Value |= (BigInteger) bitsToSet << _StatusOffset;
        }

        public void Set(MessageHoldTime bitsToSet)
        {
            _Value.ClearBits(0xFFFFFF << _HoldTimeOffset);
            _Value |= (BigInteger) bitsToSet.GetHoldTime().GetMaskedValue(0xFF000000) << _HoldTimeOffset;
        }

        public void Set(LanguagePreference bitsToSet)
        {
            _Value.ClearBits(ulong.MaxValue << _LanguagePreferenceOffset);
            _Value |= new BigInteger(bitsToSet.EncodeValue()) << _LanguagePreferenceOffset;
        }

        public void Set(ValueQualifier bitsToSet)
        {
            _Value.ClearBits(byte.MaxValue << _ValueQualifierOffset);
            _Value |= (BigInteger) bitsToSet << _ValueQualifierOffset;
        }

        public void Set(Money bitsToSet)
        {
            _Value.ClearBits(0xFFFFFFFFFFFF << _MoneyOffset);
            _Value |= (BigInteger) (ulong) bitsToSet << _MoneyOffset;
        }

        public void Set(NumericCurrencyCode bitsToSet)
        {
            _Value.ClearBits(ushort.MaxValue << _CurrencyCodeOffset);
            _Value |= (BigInteger) (ushort) bitsToSet << _CurrencyCodeOffset;
        }

        public override UserInterfaceRequestData Complete() => new(_Value);

        protected override void Set(BigInteger bitsToSet)
        {
            _Value |= bitsToSet;
        }

        #endregion
    }
}