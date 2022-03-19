using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Globalization.Country;
using Play.Globalization.Currency;

namespace Play.Emv.TestData.Ber.Primitive;

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

    private static readonly Dictionary<Tag, TagLengthValue> _TerminalValues = new()
    {
        {0x9F66, new UnknownPrimitiveValue(new TagLength(0x9F66, 0x04)).AsTagLengthValue()},
        {AmountAuthorizedNumeric.Tag, new AmountAuthorizedNumeric(222).AsTagLengthValue()},
        {AmountOtherNumeric.Tag, new AmountOtherNumeric(222).AsTagLengthValue()},
        {TerminalCountryCode.Tag, new TerminalCountryCode(new NumericCountryCode(840)).AsTagLengthValue()},
        {TerminalVerificationResults.Tag, new TerminalVerificationResults(new TerminalVerificationResult()).AsTagLengthValue()},
        {TransactionCurrencyCode.Tag, new TransactionCurrencyCode(new NumericCurrencyCode(840)).AsTagLengthValue()},
        {TransactionDate.Tag, TransactionDate.Decode(new byte[] {0x15, 0x06, 0x17}.AsMemory()).AsTagLengthValue()},
        {TransactionType.Tag, new TransactionType(TransactionTypes.GoodsAndServicesDebit).AsTagLengthValue()},
        {UnpredictableNumber.Tag, new UnpredictableNumber(3321).AsTagLengthValue()},
        {MerchantNameAndLocation.Tag, new MerchantNameAndLocation("Sam Smith, Texas").AsTagLengthValue()}
    };

    #endregion

    #region Constructor

    public ProcessingOptionsDataObjectListTestTlv() : base(_DefaultContentOctets)
    { }

    public ProcessingOptionsDataObjectListTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ProcessingOptionsDataObjectList.Tag;
    public TagLengthValue[] GetTerminalValues() => _TerminalValues.Values.ToArray();

    public TagLength[] GetRequestedItems()
    {
        return new TagLength[]
        {
            new(0x9F66, 0x04), new(AmountAuthorizedNumeric.Tag, 0x06), new(AmountOtherNumeric.Tag, 0x06),
            new(TerminalCountryCode.Tag, 0x02), new(TerminalVerificationResults.Tag, 0x05), new(TransactionCurrencyCode.Tag, 0x02),
            new(TransactionDate.Tag, 0x03), new(TransactionType.Tag, 0x01), new(UnpredictableNumber.Tag, 0x04),
            new(MerchantNameAndLocation.Tag, 0x14)
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