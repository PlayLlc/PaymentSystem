using Play.Core;

namespace Play.Emv.DataElements;

public record TransactionTypes : EnumObject<byte>
{
    #region Static Metadata

    /// <summary>
    ///     A purchase of goods and services that are debited from the cardholders account
    /// </summary>
    /// <remarks>Hex: 0: Decimal: 0</remarks>
    public static readonly TransactionTypes GoodsAndServicesDebit;

    /// <summary>
    ///     A Cash Advance allows a cardholder to withdrawal cash from a credit card
    /// </summary>
    /// <remarks>Hex: 1: Decimal: 1</remarks>
    public static readonly TransactionTypes CashAdvance;

    /// <remarks>Hex: 2: Decimal: 2</remarks>
    public static readonly TransactionTypes AdjustmentDebit;

    /// <remarks>Hex: 3: Decimal: 3</remarks>
    public static readonly TransactionTypes CheckGuaranteeDebit;

    /// <remarks>Hex: 4: Decimal: 4</remarks>
    public static readonly TransactionTypes CheckVerificationDebit;

    /// <remarks>Hex: 5: Decimal: 5</remarks>
    public static readonly TransactionTypes EuroCheckDebit;

    /// <remarks>Hex: 6: Decimal: 6</remarks>
    public static readonly TransactionTypes TravelersCheckDebit;

    /// <remarks>Hex: 7: Decimal: 7</remarks>
    public static readonly TransactionTypes LetterOfCreditDebit;

    /// <remarks>Hex: 8: Decimal: 8</remarks>
    public static readonly TransactionTypes GiroPostalBankingDebit;

    /// <summary>
    ///     A transaction in which the cardholder receives a cash disbursement, such as a credit or loan that is disbursed and
    ///     debited to their account
    /// </summary>
    /// <remarks>Hex: 9: Decimal: 9</remarks>
    public static readonly TransactionTypes GoodsAndServicesWithCashDisbursementDebit;

    /// <remarks>Hex: 0A: Decimal: 10</remarks>
    public static readonly TransactionTypes PhoneTopUpPrivate;

    /// <remarks>Hex: 0B: Decimal: 11</remarks>
    public static readonly TransactionTypes FeeCollectionDebit;

    /// <remarks>Hex: 10: Decimal: 16</remarks>
    public static readonly TransactionTypes NonCashFinancialInstrumentDebit;

    /// <remarks>Hex: 11: Decimal: 17</remarks>
    public static readonly TransactionTypes QuasiCashAndScripDebit;

    /// <summary>
    ///     A transaction in which the cardholder receives cash from the teller, such as a quick loan, ATM withdrawal using a
    ///     credit card, and so on
    /// </summary>
    /// <remarks>Hex: 17: Decimal: 23</remarks>
    public static readonly TransactionTypes FastCashDebit;

    /// <remarks>Hex: 18: Decimal: 24</remarks>
    public static readonly TransactionTypes PrivateUseDebit;

    /// <remarks>Hex: 20: Decimal: 32</remarks>
    public static readonly TransactionTypes ReturnCredit;

    /// <remarks>Hex: 21: Decimal: 33</remarks>
    public static readonly TransactionTypes DepositCredit;

    /// <remarks>Hex: 22: Decimal: 34</remarks>
    public static readonly TransactionTypes AdjustmentCredit;

    /// <remarks>Hex: 23: Decimal: 35</remarks>
    public static readonly TransactionTypes CheckDepositGuaranteeCredit;

    /// <remarks>Hex: 24: Decimal: 36</remarks>
    public static readonly TransactionTypes CheckDepositCredit;

    /// <remarks>Hex: 28: Decimal: 40</remarks>
    public static readonly TransactionTypes DepositWithCashBackCredit;

    /// <remarks>Hex: 29: Decimal: 41</remarks>
    public static readonly TransactionTypes CheckDepositWithCashBackCredit;

    /// <remarks>Hex: 2A: Decimal: 42</remarks>
    public static readonly TransactionTypes FundsDisbursement;

    /// <remarks>Hex: 2B: Decimal: 43</remarks>
    public static readonly TransactionTypes PrepaidLoadCredit;

    /// <remarks>Hex: 2C: Decimal: 44</remarks>
    public static readonly TransactionTypes OriginalCredits;

    /// <remarks>Hex: 2D: Decimal: 45</remarks>
    public static readonly TransactionTypes CashDepositWithCashBack;

    /// <remarks>Hex: 2E: Decimal: 46</remarks>
    public static readonly TransactionTypes CashDeposit;

    /// <remarks>Hex: 2F: Decimal: 47</remarks>
    public static readonly TransactionTypes SplitDeposit;

    /// <remarks>Hex: 30: Decimal: 48</remarks>
    public static readonly TransactionTypes InquiryAvailableFundsInquiry;

    /// <remarks>Hex: 31: Decimal: 49</remarks>
    public static readonly TransactionTypes InquiryBalanceInquiry;

    /// <remarks>Hex: 38: Decimal: 56</remarks>
    public static readonly TransactionTypes CardVerification;

    /// <remarks>Hex: 39: Decimal: 57</remarks>
    public static readonly TransactionTypes StatementPrintInboundOutbound;

    /// <remarks>Hex: 3A: Decimal: 58</remarks>
    public static readonly TransactionTypes MiniStatementInquiryCheckClearInboundOutbound;

    /// <remarks>Hex: 3B: Decimal: 59</remarks>
    public static readonly TransactionTypes MiniStatementInquiryLastDebitCreditInboundOutbound;

    /// <remarks>Hex: 3C: Decimal: 60</remarks>
    public static readonly TransactionTypes MiniStatementInquiryLastSourceInboundOutbound;

    /// <remarks>Hex: 3D: Decimal: 61</remarks>
    public static readonly TransactionTypes MiniStatementInquiryLastCheckInboundOutbound;

    /// <remarks>Hex: 3E: Decimal: 62</remarks>
    public static readonly TransactionTypes MiniStatementInquiryLastDebitInboundOutbound;

    /// <remarks>Hex: 3F: Decimal: 63</remarks>
    public static readonly TransactionTypes MiniStatementInquiryLastCreditInboundOutbound;

    /// <remarks>Hex: 40: Decimal: 64</remarks>
    public static readonly TransactionTypes TransferCardholderAccountsTransfer;

    /// <remarks>Hex: 48: Decimal: 72</remarks>
    public static readonly TransactionTypes TransferPrivateUse;

    /// <remarks>Hex: 4A: Decimal: 74</remarks>
    public static readonly TransactionTypes TransferFuture;

    /// <remarks>Hex: 4B: Decimal: 75</remarks>
    public static readonly TransactionTypes TransferRecurring;

    /// <remarks>Hex: 50: Decimal: 80</remarks>
    public static readonly TransactionTypes PaymentCanIncludeBothAFromAndToAccountType;

    /// <remarks>Hex: 56: Decimal: 86</remarks>
    public static readonly TransactionTypes PaymentToOnlyAToAccountIsPresent;

    /// <remarks>Hex: 58: Decimal: 88</remarks>
    public static readonly TransactionTypes PaymentEnclosed;

    /// <remarks>Hex: 59: Decimal: 89</remarks>
    public static readonly TransactionTypes PaymentPrivateUse;

    /// <remarks>Hex: 5A: Decimal: 90</remarks>
    public static readonly TransactionTypes PaymentPaymentFuture;

    /// <remarks>Hex: 5B: Decimal: 91</remarks>
    public static readonly TransactionTypes PaymentRecurring;

    /// <remarks>Hex: 5C: Decimal: 92</remarks>
    public static readonly TransactionTypes BulkAuthorization;

    /// <remarks>Hex: 5D: Decimal: 93</remarks>
    public static readonly TransactionTypes ReturnPayment;

    /// <remarks>Hex: 5E: Decimal: 94</remarks>
    public static readonly TransactionTypes SchemeReturnPayment;

    /// <remarks>Hex: 5F: Decimal: 95</remarks>
    public static readonly TransactionTypes CorporateDatedPayment;

    /// <remarks>Hex: 90: Decimal: 144</remarks>
    public static readonly TransactionTypes PinChange;

    /// <remarks>Hex: 91: Decimal: 145</remarks>
    public static readonly TransactionTypes PinVerify;

    #endregion

    #region Constructor

    static TransactionTypes()
    {
        GoodsAndServicesDebit = new TransactionTypes(0x0);
        CashAdvance = new TransactionTypes(0x1);
        AdjustmentDebit = new TransactionTypes(0x2);
        CheckGuaranteeDebit = new TransactionTypes(0x3);
        CheckVerificationDebit = new TransactionTypes(0x4);
        EuroCheckDebit = new TransactionTypes(0x5);
        TravelersCheckDebit = new TransactionTypes(0x6);
        LetterOfCreditDebit = new TransactionTypes(0x7);
        GiroPostalBankingDebit = new TransactionTypes(0x8);
        GoodsAndServicesWithCashDisbursementDebit = new TransactionTypes(0x9);
        PhoneTopUpPrivate = new TransactionTypes(0x0A);
        FeeCollectionDebit = new TransactionTypes(0x0B);
        NonCashFinancialInstrumentDebit = new TransactionTypes(0x10);
        QuasiCashAndScripDebit = new TransactionTypes(0x11);
        FastCashDebit = new TransactionTypes(0x17);
        PrivateUseDebit = new TransactionTypes(0x18);
        ReturnCredit = new TransactionTypes(0x20);
        DepositCredit = new TransactionTypes(0x21);
        AdjustmentCredit = new TransactionTypes(0x22);
        CheckDepositGuaranteeCredit = new TransactionTypes(0x23);
        CheckDepositCredit = new TransactionTypes(0x24);
        DepositWithCashBackCredit = new TransactionTypes(0x28);
        CheckDepositWithCashBackCredit = new TransactionTypes(0x29);
        FundsDisbursement = new TransactionTypes(0x2A);
        PrepaidLoadCredit = new TransactionTypes(0x2B);
        OriginalCredits = new TransactionTypes(0x2C);
        CashDepositWithCashBack = new TransactionTypes(0x2D);
        CashDeposit = new TransactionTypes(0x2E);
        SplitDeposit = new TransactionTypes(0x2F);
        InquiryAvailableFundsInquiry = new TransactionTypes(0x30);
        InquiryBalanceInquiry = new TransactionTypes(0x31);
        CardVerification = new TransactionTypes(0x38);
        StatementPrintInboundOutbound = new TransactionTypes(0x39);
        MiniStatementInquiryCheckClearInboundOutbound = new TransactionTypes(0x3A);
        MiniStatementInquiryLastDebitCreditInboundOutbound = new TransactionTypes(0x3B);
        MiniStatementInquiryLastSourceInboundOutbound = new TransactionTypes(0x3C);
        MiniStatementInquiryLastCheckInboundOutbound = new TransactionTypes(0x3D);
        MiniStatementInquiryLastDebitInboundOutbound = new TransactionTypes(0x3E);
        MiniStatementInquiryLastCreditInboundOutbound = new TransactionTypes(0x3F);
        TransferCardholderAccountsTransfer = new TransactionTypes(0x40);
        TransferPrivateUse = new TransactionTypes(0x48);
        TransferFuture = new TransactionTypes(0x4A);
        TransferRecurring = new TransactionTypes(0x4B);
        PaymentCanIncludeBothAFromAndToAccountType = new TransactionTypes(0x50);
        PaymentToOnlyAToAccountIsPresent = new TransactionTypes(0x56);
        PaymentEnclosed = new TransactionTypes(0x58);
        PaymentPrivateUse = new TransactionTypes(0x59);
        PaymentPaymentFuture = new TransactionTypes(0x5A);
        PaymentRecurring = new TransactionTypes(0x5B);
        BulkAuthorization = new TransactionTypes(0x5C);
        ReturnPayment = new TransactionTypes(0x5D);
        SchemeReturnPayment = new TransactionTypes(0x5E);
        CorporateDatedPayment = new TransactionTypes(0x5F);
        PinChange = new TransactionTypes(0x90);
        PinVerify = new TransactionTypes(0x91);
    }

    private TransactionTypes(byte value) : base(value)
    { }

    #endregion
}