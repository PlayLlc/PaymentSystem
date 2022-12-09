using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Combines all parameters to be sent with the OUT DataExchangeSignal or MSG DataExchangeSignal.
/// </summary>
public record UserInterfaceRequestData : DataElement<BigInteger>, IRetrievePrimitiveValueMetadata, IEqualityComparer<UserInterfaceRequestData>
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
    private const byte _ByteLength = 22;

    #endregion

    #region Constructor

    private UserInterfaceRequestData(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool IsValueQualifierPresent() => GetValueQualifier() != ValueQualifiers.None;

    public Money? GetAmount() =>
        GetValueQualifier() == ValueQualifiers.None
            ? null
            : new Money(((ulong) (_Value >> _MoneyOffset)).GetMaskedValue(0xFFFF000000000000), GetCurrencyCode());

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static Builder GetBuilder() => new();
    public NumericCurrencyCode GetCurrencyCode() => new((ushort) (_Value >> _CurrencyCodeOffset));
    public MessageHoldTime GetHoldTimeValue() => new(new Milliseconds((long) ((ulong) (_Value >> _HoldTimeOffset)).GetMaskedValue(0xFFFF000000000000)));
    public LanguagePreference GetLanguagePreference() => new((ulong) (_Value >> _LanguagePreferenceOffset));
    public DisplayMessageIdentifiers GetMessageIdentifier() => DisplayMessageIdentifiers.Get((byte) (_Value >> _MessageIdentifierOffset));
    public DisplayStatuses GetStatus() => DisplayStatuses.Get((byte) (_Value >> _StatusOffset));
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public ValueQualifiers GetValueQualifier()
    {
        if (ValueQualifiers.Empty.TryGet((byte) (_Value >> _ValueQualifierOffset), out EnumObject<byte>? result))
            return (ValueQualifiers) result!;

        return ValueQualifiers.None;
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static UserInterfaceRequestData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override UserInterfaceRequestData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static UserInterfaceRequestData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new UserInterfaceRequestData(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

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

    public static UserInterfaceRequestData operator |(UserInterfaceRequestData left, UserInterfaceRequestData right) => new(left._Value | right._Value);

    #endregion

    public class Builder : PrimitiveValueBuilder<BigInteger>
    {
        #region Constructor

        internal Builder(UserInterfaceRequestData value)
        {
            _Value = value._Value;
        }

        internal Builder()
        {
            Set(DisplayMessageIdentifiers.NotAvailable);
            Set(DisplayStatuses.NotAvailable);
            Set(MessageHoldTime.MinimumValue);

            //Set(LanguagePreference);
            Set(ValueQualifiers.None);
        }

        #endregion

        #region Instance Members

        public void Reset(UserInterfaceRequestData value)
        {
            _Value = value._Value;
        }

        public void Set(DisplayMessageIdentifiers bitsToSet)
        {
            _Value = _Value.ClearBits(byte.MaxValue << _MessageIdentifierOffset);
            _Value |= (BigInteger) bitsToSet << _MessageIdentifierOffset;
        }

        public void Set(DisplayStatuses bitsToSet)
        {
            _Value = _Value.ClearBits(byte.MaxValue << _StatusOffset);
            _Value |= (BigInteger) bitsToSet << _StatusOffset;
        }

        public void Set(MessageHoldTime bitsToSet)
        {
            _Value = _Value.ClearBits(0xFFFFFF << _HoldTimeOffset);
            _Value |= (BigInteger) ((long) bitsToSet.GetHoldTime()).GetMaskedValue(0xFF000000) << _HoldTimeOffset;
        }

        public void Set(LanguagePreference bitsToSet)
        {
            _Value = _Value.ClearBits(ulong.MaxValue << _LanguagePreferenceOffset);
            _Value |= new BigInteger(bitsToSet.EncodeValue()) << _LanguagePreferenceOffset;
        }

        public void Set(ValueQualifiers bitsToSet)
        {
            _Value = _Value.ClearBits(byte.MaxValue << _ValueQualifierOffset);
            _Value |= (BigInteger) bitsToSet << _ValueQualifierOffset;
        }

        public void Set(Money bitsToSet)
        {
            _Value = _Value.ClearBits(0xFFFFFFFFFFFF << _MoneyOffset);
            _Value |= (BigInteger) (ulong) bitsToSet << _MoneyOffset;
        }

        public void Set(NumericCurrencyCode bitsToSet)
        {
            _Value = _Value.ClearBits(ushort.MaxValue << _CurrencyCodeOffset);
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