using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the card's preference for the kernel on which the contactless application can be processed.
/// </summary>
public record KernelIdentifier : DataElement<ulong>, IEqualityComparer<KernelIdentifier>
{
    #region Static Metadata

    //TODO: figure out the best way to map these values with the logic needed
    public static readonly KernelIdentifier AmericanExpress;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly KernelIdentifier ChinaUnionPay;
    public static readonly KernelIdentifier Discover;
    public static readonly KernelIdentifier Jcb;
    public static readonly KernelIdentifier MasterCard;
    public static readonly Tag Tag = 0x9F2A;
    public static readonly KernelIdentifier VisaInternational;
    private const byte _MinByteLength = 1;
    private const byte _MaxByteLength = 8;

    #endregion

    #region Constructor

    static KernelIdentifier()
    {
        const byte americanExpress = 0x04;
        const byte discover = 0x06;
        const byte jcb = 0x05;
        const byte masterCard = 0x02;
        const byte unionPay = 0x07;
        const byte visa = 0x03;

        AmericanExpress = new KernelIdentifier(americanExpress);
        Discover = new KernelIdentifier(discover);
        Jcb = new KernelIdentifier(jcb);
        MasterCard = new KernelIdentifier(masterCard);
        ChinaUnionPay = new KernelIdentifier(unionPay);
        VisaInternational = new KernelIdentifier(visa);
    }

    public KernelIdentifier(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ShortKernelIdTypes AsKernelId() => ShortKernelIdTypes.Get((byte) _Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public KernelType GetKernelType()
    {
        const byte bitOffset = 7;

        if (KernelType.TryGet((byte) (_Value >> bitOffset), out KernelType? result))
            throw new DataElementParsingException($"The {nameof(KernelType)} could not be determined from the {nameof(KernelIdentifier)}");

        return result;
    }

    /// <summary>
    ///     GetShortKernelId
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    public ShortKernelIdTypes GetShortKernelId()
    {
        const byte bitOffset = 56;

        return ShortKernelIdTypes.Get((byte) (_Value >> bitOffset));
    }

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsDefaultKernelIdentifierNeeded() => _Value == 0;

    public bool IsDomesticKernel()
    {
        ulong value = _Value >> 62;

        return (value == 0b10) || (value == 0b11);
    }

    public bool IsInternationalKernel() => (_Value >> 62) == 0;
    public bool IsKernelIdentifierInProprietaryFormat() => (_Value >> 62) == 0b11;

    /// <summary>
    ///     Determines whether the other <see cref="KernelIdentifier" /> is a match according to Combination Selection
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <remarks>Book B Section 3.3.2.5 C</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    public bool IsKernelMatch(ShortKernelIdTypes other)
    {
        if (IsShortKernelIdFlagSet())
        {
            if (!IsShortKernelIdFlagSet())
                return false;

            return GetShortKernelId() == other;
        }

        return Equals(other);
    }

    public bool IsReservedForFutureUseFlagSet() => (_Value >> 62) == 1;

    public bool TryGetCurrencyCode(out NumericCurrencyCode? result)
    {
        if (IsKernelIdentifierInProprietaryFormat())
        {
            result = null;

            return false;
        }

        if (!IsDomesticKernel())
        {
            result = null;

            return false;
        }

        result = new NumericCurrencyCode((ushort) (_Value >> 40));

        return true;
    }

    public static bool TryGetDefaultKernelIdentifier(ApplicationDedicatedFileName applicationDedicatedFileName, out KernelIdentifier? kernelIdentifier)
    {
        RegisteredApplicationProviderIndicator rid = applicationDedicatedFileName.GetRegisteredApplicationProviderIndicator();

        if (rid == RegisteredApplicationProviderIndicators.AmericanExpress)
        {
            kernelIdentifier = AmericanExpress;

            return true;
        }

        if (rid == RegisteredApplicationProviderIndicators.Discover)
        {
            kernelIdentifier = Discover;

            return true;
        }

        if (rid == RegisteredApplicationProviderIndicators.Jcb)
        {
            kernelIdentifier = Jcb;

            return true;
        }

        if (rid == RegisteredApplicationProviderIndicators.MasterCard)
        {
            kernelIdentifier = MasterCard;

            return true;
        }

        if (rid == RegisteredApplicationProviderIndicators.VisaInternational)
        {
            kernelIdentifier = VisaInternational;

            return true;
        }

        if (rid == RegisteredApplicationProviderIndicators.ChinaUnionPay)
        {
            kernelIdentifier = ChinaUnionPay;

            return true;
        }

        kernelIdentifier = default;

        return false;
    }

    /// <summary>
    ///     If byte 1, b8 and b7 of the Kernel EncodingId have the value 00b or 01b17, then Requested Kernel ID is
    ///     equal to the value of byte 1 of the Kernel EncodingId(i.e.b8b7||Short Kernel ID)
    /// </summary>
    /// <remarks>Book B Section 3.3.2.5 C</remarks>
    private bool IsShortKernelIdFlagSet() => IsInternationalKernel() || IsReservedForFutureUseFlagSet();

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static KernelIdentifier Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override KernelIdentifier Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static KernelIdentifier Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMinimumLength(value, _MaxByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new KernelIdentifier(result);
    }

    #endregion

    #region Equality

    public bool Equals(KernelIdentifier? x, KernelIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(KernelIdentifier obj) => obj.GetHashCode();

    #endregion
}