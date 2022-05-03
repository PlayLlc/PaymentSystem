using AutoFixture;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization.Country;
using Play.Globalization.Currency;

namespace Play.Testing.Emv._TestTlv;

public class ProcessingOptionsDataObjectListTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x9F, 0x66, 0x04, // PUNATC(Track2)
        0x9F, 0x02, 0x06, // Amount Authorized (Numeric)
        0x9F, 0x03, 0x06, // Amount Other (Numeric)
        0x9F, 0x1A, 0x02, // Terminal Country Code
        0x95, 0x05, // Terminal Verification Results

        0x5F, 0x2A, 0x02, // Transaction Currency Code
        0x9A, 0x03, // Transaction Date
        0x9C, 0x01, // Transaction Type

        0x9F, 0x37, 0x04, // Unpredictable Number
        0x9F, 0x4E, 0x14 // Merchant Name and Location
    };

    private static readonly Dictionary<Tag, PrimitiveValue> _TerminalValues = new()
    {
        {PunatcTrack2.Tag, new PunatcTrack2(0)},
        {AmountAuthorizedNumeric.Tag, new AmountAuthorizedNumeric(222)},
        {AmountOtherNumeric.Tag, new AmountOtherNumeric(222)},
        {TerminalCountryCode.Tag, new TerminalCountryCode(new NumericCountryCode(840))},
        {TerminalVerificationResults.Tag, new TerminalVerificationResults(new TerminalVerificationResult())},
        {TransactionCurrencyCode.Tag, new TransactionCurrencyCode(new NumericCurrencyCode(840))},
        {TransactionDate.Tag, TransactionDate.Decode(new byte[] {0x15, 0x06, 0x17}.AsMemory())},
        {TransactionType.Tag, new TransactionType(TransactionTypes.GoodsAndServicesDebit)},
        {UnpredictableNumber.Tag, new UnpredictableNumber(3321)},
        {MerchantNameAndLocation.Tag, new MerchantNameAndLocation("Sam Smith, Texas")}
    };

    #endregion

    #region Constructor

    public ProcessingOptionsDataObjectListTestTlv() : base(_DefaultContentOctets)
    { }

    public ProcessingOptionsDataObjectListTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int GetValueByteCount() => _DefaultContentOctets.Length;
    public int GetTagLengthValueByteCount() => GetValueByteCount() + 3;

    public TagLengthValue[] AsTagLengthValues()
    {
        List<TagLengthValue> result = new();
        foreach (PrimitiveValue? value in _TerminalValues.Values)
            result.Add(new TagLengthValue(value.GetTag(), value.EncodeValue(EmvCodec.GetCodec())));

        return result.ToArray();
    }

    public static void RegisterDefault(IFixture fixture)
    {
        EmvCodec codec = EmvCodec.GetCodec();

        fixture.Register(() => new ProcessingOptionsDataObjectList(codec.DecodeTagLengths(new byte[]
        {
            0x9F, 0x66, 0x04, 0x9F, 0x02, 0x06, 0x9F, 0x03, 0x06, 0x9F,
            0x1A, 0x02, 0x95, 0x05, 0x5F, 0x2A, 0x02, 0x9A, 0x03, 0x9C,
            0x01, 0x9F, 0x37, 0x04, 0x9F, 0x4E, 0x14
        })));
    }

    public override Tag GetTag() => ProcessingOptionsDataObjectList.Tag;
    public PrimitiveValue[] GetTerminalValues() => _TerminalValues.Values.ToArray();

    public TagLength[] GetRequestedItems()
    {
        return new TagLength[]
        {
            new(0x9F66, 0x04), new(AmountAuthorizedNumeric.Tag, 0x06), new(AmountOtherNumeric.Tag, 0x06), new(TerminalCountryCode.Tag, 0x02),
            new(TerminalVerificationResults.Tag, 0x05), new(TransactionCurrencyCode.Tag, 0x02), new(TransactionDate.Tag, 0x03),
            new(TransactionType.Tag, 0x01), new(UnpredictableNumber.Tag, 0x04), new(MerchantNameAndLocation.Tag, 0x14)
        };
    }

    public Tag[] GetRequestedTags()
    {
        return new Tag[]
        {
            0x9F66, AmountAuthorizedNumeric.Tag, AmountOtherNumeric.Tag, TerminalCountryCode.Tag, TerminalVerificationResults.Tag,
            TransactionCurrencyCode.Tag, TransactionDate.Tag, TransactionType.Tag, UnpredictableNumber.Tag, MerchantNameAndLocation.Tag
        };
    }

    #endregion
}