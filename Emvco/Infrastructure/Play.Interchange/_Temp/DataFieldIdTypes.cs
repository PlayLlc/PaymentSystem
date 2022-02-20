using System.Collections.Immutable;

using Play.Core;

namespace Play.Interchange.Messages.DataFields;

public sealed record DataFieldIdTypes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, DataFieldIdTypes> _ValueObjectMap;
    public static readonly DataFieldIdTypes Bitmap;
    public static readonly DataFieldIdTypes PrimaryAccountNumber;
    public static readonly DataFieldIdTypes ProcessingCode;
    public static readonly DataFieldIdTypes AmountTransaction;
    public static readonly DataFieldIdTypes AmountSettlement;
    public static readonly DataFieldIdTypes AmountCardholderBilling;
    public static readonly DataFieldIdTypes TransmissionDateTime;
    public static readonly DataFieldIdTypes AmountCardholderBillingFee;
    public static readonly DataFieldIdTypes ConversionRateSettlement;
    public static readonly DataFieldIdTypes ConversionRateCardholderBilling;
    public static readonly DataFieldIdTypes SystemTraceAuditNumberStan;
    public static readonly DataFieldIdTypes LocalTransactionTimeHhmmss;
    public static readonly DataFieldIdTypes LocalTransactionDateMmdd;
    public static readonly DataFieldIdTypes ExpirationDate;
    public static readonly DataFieldIdTypes SettlementDate;
    public static readonly DataFieldIdTypes CurrencyConversionDate;
    public static readonly DataFieldIdTypes CaptureDate;
    public static readonly DataFieldIdTypes MerchantTypeOrMerchantCategoryCode;
    public static readonly DataFieldIdTypes AcquiringInstitutionCountryCode;
    public static readonly DataFieldIdTypes PanExtendedCountryCode;
    public static readonly DataFieldIdTypes ForwardingInstitutionCountryCode;
    public static readonly DataFieldIdTypes PointOfServiceEntryMode;
    public static readonly DataFieldIdTypes ApplicationPanSequenceNumber;
    public static readonly DataFieldIdTypes FunctionCodeIso85831993OrNetworkInternationalIdentifierNii;
    public static readonly DataFieldIdTypes PointOfServiceConditionCode;
    public static readonly DataFieldIdTypes PointOfServiceCaptureCode;
    public static readonly DataFieldIdTypes AuthorizingIdentificationResponseLength;
    public static readonly DataFieldIdTypes AmountTransactionFee;
    public static readonly DataFieldIdTypes AmountSettlementFee;
    public static readonly DataFieldIdTypes AmountTransactionProcessingFee;
    public static readonly DataFieldIdTypes AmountSettlementProcessingFee;
    public static readonly DataFieldIdTypes AcquiringInstitutionIdentificationCode;
    public static readonly DataFieldIdTypes ForwardingInstitutionIdentificationCode;
    public static readonly DataFieldIdTypes PrimaryAccountNumberExtended;
    public static readonly DataFieldIdTypes Track2Data;
    public static readonly DataFieldIdTypes Track3Data;
    public static readonly DataFieldIdTypes RetrievalReferenceNumber;
    public static readonly DataFieldIdTypes AuthorizationIdentificationResponse;
    public static readonly DataFieldIdTypes ResponseCode;
    public static readonly DataFieldIdTypes ServiceRestrictionCode;
    public static readonly DataFieldIdTypes CardAcceptorTerminalIdentification;
    public static readonly DataFieldIdTypes CardAcceptorIdentificationCode;
    public static readonly DataFieldIdTypes CardAcceptorNameLocation;
    public static readonly DataFieldIdTypes AdditionalResponseData;
    public static readonly DataFieldIdTypes Track1Data;
    public static readonly DataFieldIdTypes AdditionalDataIso;
    public static readonly DataFieldIdTypes AdditionalDataNational;
    public static readonly DataFieldIdTypes AdditionalDataPrivate;
    public static readonly DataFieldIdTypes CurrencyCodeTransaction;
    public static readonly DataFieldIdTypes CurrencyCodeSettlement;
    public static readonly DataFieldIdTypes CurrencyCodeCardholderBilling;
    public static readonly DataFieldIdTypes PersonalIdentificationNumberData;
    public static readonly DataFieldIdTypes SecurityRelatedControlInformation;
    public static readonly DataFieldIdTypes AdditionalAmounts;
    public static readonly DataFieldIdTypes IccDataEmvHavingMultipleTags;
    public static readonly DataFieldIdTypes ReservedIso;
    public static readonly DataFieldIdTypes ReservedNational;

    public static readonly DataFieldIdTypes
        ReservedNationalEgSettlementRequestBatchNumberAdviceTransactionsOriginalTransactionAmountBatchUploadOriginalMtiPlusOriginalRrnPlusOriginalStanEtc;

    public static readonly DataFieldIdTypes ReservedPrivateEgCvv2ServiceCodeTransactions;
    public static readonly DataFieldIdTypes ReservedPrivateEgTransactionsInvoiceNumberKeyExchangeTransactionsTpkKeyEtc;
    public static readonly DataFieldIdTypes ReservedPrivate;
    public static readonly DataFieldIdTypes MessageAuthenticationCodeMac;
    public static readonly DataFieldIdTypes ExtendedBitmapIndicator;
    public static readonly DataFieldIdTypes SettlementCode;
    public static readonly DataFieldIdTypes ExtendedPaymentCode;
    public static readonly DataFieldIdTypes ReceivingInstitutionCountryCode;
    public static readonly DataFieldIdTypes SettlementInstitutionCountryCode;
    public static readonly DataFieldIdTypes NetworkManagementInformationCode;
    public static readonly DataFieldIdTypes MessageNumber;
    public static readonly DataFieldIdTypes LastMessagesNumber;
    public static readonly DataFieldIdTypes ActionDateYymmdd;
    public static readonly DataFieldIdTypes NumberOfCredits;
    public static readonly DataFieldIdTypes CreditsReversalNumber;
    public static readonly DataFieldIdTypes NumberOfDebits;
    public static readonly DataFieldIdTypes DebitsReversalNumber;
    public static readonly DataFieldIdTypes TransferNumber;
    public static readonly DataFieldIdTypes TransferReversalNumber;
    public static readonly DataFieldIdTypes NumberOfInquiries;
    public static readonly DataFieldIdTypes NumberOfAuthorizations;
    public static readonly DataFieldIdTypes CreditsProcessingFeeAmount;
    public static readonly DataFieldIdTypes CreditsTransactionFeeAmount;
    public static readonly DataFieldIdTypes DebitsProcessingFeeAmount;
    public static readonly DataFieldIdTypes DebitsTransactionFeeAmount;
    public static readonly DataFieldIdTypes TotalAmountOfCredits;
    public static readonly DataFieldIdTypes CreditsReversalAmount;
    public static readonly DataFieldIdTypes TotalAmountOfDebits;
    public static readonly DataFieldIdTypes DebitsReversalAmount;
    public static readonly DataFieldIdTypes OriginalDataElements;
    public static readonly DataFieldIdTypes FileUpdateCode;
    public static readonly DataFieldIdTypes FileSecurityCode;
    public static readonly DataFieldIdTypes ResponseIndicator;
    public static readonly DataFieldIdTypes ServiceIndicator;
    public static readonly DataFieldIdTypes ReplacementAmounts;
    public static readonly DataFieldIdTypes MessageSecurityCode;
    public static readonly DataFieldIdTypes NetSettlementAmount;
    public static readonly DataFieldIdTypes Payee;
    public static readonly DataFieldIdTypes SettlementInstitutionIdentificationCode;
    public static readonly DataFieldIdTypes ReceivingInstitutionIdentificationCode;
    public static readonly DataFieldIdTypes FileName;
    public static readonly DataFieldIdTypes AccountIdentification1;
    public static readonly DataFieldIdTypes AccountIdentification2;
    public static readonly DataFieldIdTypes TransactionDescription;

    #endregion

    #region Constructor

    static DataFieldIdTypes()
    {
        Bitmap = new DataFieldIdTypes(1);
        PrimaryAccountNumber = new DataFieldIdTypes(2);
        ProcessingCode = new DataFieldIdTypes(3);
        AmountTransaction = new DataFieldIdTypes(4);
        AmountSettlement = new DataFieldIdTypes(5);
        AmountCardholderBilling = new DataFieldIdTypes(6);
        TransmissionDateTime = new DataFieldIdTypes(7);
        AmountCardholderBillingFee = new DataFieldIdTypes(8);
        ConversionRateSettlement = new DataFieldIdTypes(9);
        ConversionRateCardholderBilling = new DataFieldIdTypes(10);
        SystemTraceAuditNumberStan = new DataFieldIdTypes(11);
        LocalTransactionTimeHhmmss = new DataFieldIdTypes(12);
        LocalTransactionDateMmdd = new DataFieldIdTypes(13);
        ExpirationDate = new DataFieldIdTypes(14);
        SettlementDate = new DataFieldIdTypes(15);
        CurrencyConversionDate = new DataFieldIdTypes(16);
        CaptureDate = new DataFieldIdTypes(17);
        MerchantTypeOrMerchantCategoryCode = new DataFieldIdTypes(18);
        AcquiringInstitutionCountryCode = new DataFieldIdTypes(19);
        PanExtendedCountryCode = new DataFieldIdTypes(20);
        ForwardingInstitutionCountryCode = new DataFieldIdTypes(21);
        PointOfServiceEntryMode = new DataFieldIdTypes(22);
        ApplicationPanSequenceNumber = new DataFieldIdTypes(23);
        FunctionCodeIso85831993OrNetworkInternationalIdentifierNii = new DataFieldIdTypes(24);
        PointOfServiceConditionCode = new DataFieldIdTypes(25);
        PointOfServiceCaptureCode = new DataFieldIdTypes(26);
        AuthorizingIdentificationResponseLength = new DataFieldIdTypes(27);
        AmountTransactionFee = new DataFieldIdTypes(28);
        AmountSettlementFee = new DataFieldIdTypes(29);
        AmountTransactionProcessingFee = new DataFieldIdTypes(30);
        AmountSettlementProcessingFee = new DataFieldIdTypes(31);
        AcquiringInstitutionIdentificationCode = new DataFieldIdTypes(32);
        ForwardingInstitutionIdentificationCode = new DataFieldIdTypes(33);
        PrimaryAccountNumberExtended = new DataFieldIdTypes(34);
        Track2Data = new DataFieldIdTypes(35);
        Track3Data = new DataFieldIdTypes(36);
        RetrievalReferenceNumber = new DataFieldIdTypes(37);
        AuthorizationIdentificationResponse = new DataFieldIdTypes(38);
        ResponseCode = new DataFieldIdTypes(39);
        ServiceRestrictionCode = new DataFieldIdTypes(40);
        CardAcceptorTerminalIdentification = new DataFieldIdTypes(41);
        CardAcceptorIdentificationCode = new DataFieldIdTypes(42);
        CardAcceptorNameLocation = new DataFieldIdTypes(43);
        AdditionalResponseData = new DataFieldIdTypes(44);
        Track1Data = new DataFieldIdTypes(45);
        AdditionalDataIso = new DataFieldIdTypes(46);
        AdditionalDataNational = new DataFieldIdTypes(47);
        AdditionalDataPrivate = new DataFieldIdTypes(48);
        CurrencyCodeTransaction = new DataFieldIdTypes(49);
        CurrencyCodeSettlement = new DataFieldIdTypes(50);
        CurrencyCodeCardholderBilling = new DataFieldIdTypes(51);
        PersonalIdentificationNumberData = new DataFieldIdTypes(52);
        SecurityRelatedControlInformation = new DataFieldIdTypes(53);
        AdditionalAmounts = new DataFieldIdTypes(54);
        IccDataEmvHavingMultipleTags = new DataFieldIdTypes(55);
        ReservedIso = new DataFieldIdTypes(56);
        ReservedNational = new DataFieldIdTypes(57);

        ReservedNationalEgSettlementRequestBatchNumberAdviceTransactionsOriginalTransactionAmountBatchUploadOriginalMtiPlusOriginalRrnPlusOriginalStanEtc =
            new DataFieldIdTypes(60);
        ReservedPrivateEgCvv2ServiceCodeTransactions = new DataFieldIdTypes(61);
        ReservedPrivateEgTransactionsInvoiceNumberKeyExchangeTransactionsTpkKeyEtc = new DataFieldIdTypes(62);
        ReservedPrivate = new DataFieldIdTypes(63);
        MessageAuthenticationCodeMac = new DataFieldIdTypes(64);
        ExtendedBitmapIndicator = new DataFieldIdTypes(65);
        SettlementCode = new DataFieldIdTypes(66);
        ExtendedPaymentCode = new DataFieldIdTypes(67);
        ReceivingInstitutionCountryCode = new DataFieldIdTypes(68);
        SettlementInstitutionCountryCode = new DataFieldIdTypes(69);
        NetworkManagementInformationCode = new DataFieldIdTypes(70);
        MessageNumber = new DataFieldIdTypes(71);
        LastMessagesNumber = new DataFieldIdTypes(72);
        ActionDateYymmdd = new DataFieldIdTypes(73);
        NumberOfCredits = new DataFieldIdTypes(74);
        CreditsReversalNumber = new DataFieldIdTypes(75);
        NumberOfDebits = new DataFieldIdTypes(76);
        DebitsReversalNumber = new DataFieldIdTypes(77);
        TransferNumber = new DataFieldIdTypes(78);
        TransferReversalNumber = new DataFieldIdTypes(79);
        NumberOfInquiries = new DataFieldIdTypes(80);
        NumberOfAuthorizations = new DataFieldIdTypes(81);
        CreditsProcessingFeeAmount = new DataFieldIdTypes(82);
        CreditsTransactionFeeAmount = new DataFieldIdTypes(83);
        DebitsProcessingFeeAmount = new DataFieldIdTypes(84);
        DebitsTransactionFeeAmount = new DataFieldIdTypes(85);
        TotalAmountOfCredits = new DataFieldIdTypes(86);
        CreditsReversalAmount = new DataFieldIdTypes(87);
        TotalAmountOfDebits = new DataFieldIdTypes(88);
        DebitsReversalAmount = new DataFieldIdTypes(89);
        OriginalDataElements = new DataFieldIdTypes(90);
        FileUpdateCode = new DataFieldIdTypes(91);
        FileSecurityCode = new DataFieldIdTypes(92);
        ResponseIndicator = new DataFieldIdTypes(93);
        ServiceIndicator = new DataFieldIdTypes(94);
        ReplacementAmounts = new DataFieldIdTypes(95);
        MessageSecurityCode = new DataFieldIdTypes(96);
        NetSettlementAmount = new DataFieldIdTypes(97);
        Payee = new DataFieldIdTypes(98);
        SettlementInstitutionIdentificationCode = new DataFieldIdTypes(99);
        ReceivingInstitutionIdentificationCode = new DataFieldIdTypes(100);
        FileName = new DataFieldIdTypes(101);
        AccountIdentification1 = new DataFieldIdTypes(102);
        AccountIdentification2 = new DataFieldIdTypes(103);
        TransactionDescription = new DataFieldIdTypes(104);
        _ValueObjectMap = new Dictionary<byte, DataFieldIdTypes>
        {
            {Bitmap, Bitmap},
            {PrimaryAccountNumber, PrimaryAccountNumber},
            {ProcessingCode, ProcessingCode},
            {AmountTransaction, AmountTransaction},
            {AmountSettlement, AmountSettlement},
            {AmountCardholderBilling, AmountCardholderBilling},
            {TransmissionDateTime, TransmissionDateTime},
            {AmountCardholderBillingFee, AmountCardholderBillingFee},
            {ConversionRateSettlement, ConversionRateSettlement},
            {ConversionRateCardholderBilling, ConversionRateCardholderBilling},
            {SystemTraceAuditNumberStan, SystemTraceAuditNumberStan},
            {LocalTransactionTimeHhmmss, LocalTransactionTimeHhmmss},
            {LocalTransactionDateMmdd, LocalTransactionDateMmdd},
            {ExpirationDate, ExpirationDate},
            {SettlementDate, SettlementDate},
            {CurrencyConversionDate, CurrencyConversionDate},
            {CaptureDate, CaptureDate},
            {MerchantTypeOrMerchantCategoryCode, MerchantTypeOrMerchantCategoryCode},
            {AcquiringInstitutionCountryCode, AcquiringInstitutionCountryCode},
            {PanExtendedCountryCode, PanExtendedCountryCode},
            {ForwardingInstitutionCountryCode, ForwardingInstitutionCountryCode},
            {PointOfServiceEntryMode, PointOfServiceEntryMode},
            {ApplicationPanSequenceNumber, ApplicationPanSequenceNumber},
            {FunctionCodeIso85831993OrNetworkInternationalIdentifierNii, FunctionCodeIso85831993OrNetworkInternationalIdentifierNii},
            {PointOfServiceConditionCode, PointOfServiceConditionCode},
            {PointOfServiceCaptureCode, PointOfServiceCaptureCode},
            {AuthorizingIdentificationResponseLength, AuthorizingIdentificationResponseLength},
            {AmountTransactionFee, AmountTransactionFee},
            {AmountSettlementFee, AmountSettlementFee},
            {AmountTransactionProcessingFee, AmountTransactionProcessingFee},
            {AmountSettlementProcessingFee, AmountSettlementProcessingFee},
            {AcquiringInstitutionIdentificationCode, AcquiringInstitutionIdentificationCode},
            {ForwardingInstitutionIdentificationCode, ForwardingInstitutionIdentificationCode},
            {PrimaryAccountNumberExtended, PrimaryAccountNumberExtended},
            {Track2Data, Track2Data},
            {Track3Data, Track3Data},
            {RetrievalReferenceNumber, RetrievalReferenceNumber},
            {AuthorizationIdentificationResponse, AuthorizationIdentificationResponse},
            {ResponseCode, ResponseCode},
            {ServiceRestrictionCode, ServiceRestrictionCode},
            {CardAcceptorTerminalIdentification, CardAcceptorTerminalIdentification},
            {CardAcceptorIdentificationCode, CardAcceptorIdentificationCode},
            {CardAcceptorNameLocation, CardAcceptorNameLocation},
            {AdditionalResponseData, AdditionalResponseData},
            {Track1Data, Track1Data},
            {AdditionalDataIso, AdditionalDataIso},
            {AdditionalDataNational, AdditionalDataNational},
            {AdditionalDataPrivate, AdditionalDataPrivate},
            {CurrencyCodeTransaction, CurrencyCodeTransaction},
            {CurrencyCodeSettlement, CurrencyCodeSettlement},
            {CurrencyCodeCardholderBilling, CurrencyCodeCardholderBilling},
            {PersonalIdentificationNumberData, PersonalIdentificationNumberData},
            {SecurityRelatedControlInformation, SecurityRelatedControlInformation},
            {AdditionalAmounts, AdditionalAmounts},
            {IccDataEmvHavingMultipleTags, IccDataEmvHavingMultipleTags},
            {ReservedIso, ReservedIso},
            {ReservedNational, ReservedNational},
            {
                ReservedNationalEgSettlementRequestBatchNumberAdviceTransactionsOriginalTransactionAmountBatchUploadOriginalMtiPlusOriginalRrnPlusOriginalStanEtc,
                ReservedNationalEgSettlementRequestBatchNumberAdviceTransactionsOriginalTransactionAmountBatchUploadOriginalMtiPlusOriginalRrnPlusOriginalStanEtc
            },
            {ReservedPrivateEgCvv2ServiceCodeTransactions, ReservedPrivateEgCvv2ServiceCodeTransactions},
            {
                ReservedPrivateEgTransactionsInvoiceNumberKeyExchangeTransactionsTpkKeyEtc,
                ReservedPrivateEgTransactionsInvoiceNumberKeyExchangeTransactionsTpkKeyEtc
            },
            {ReservedPrivate, ReservedPrivate},
            {MessageAuthenticationCodeMac, MessageAuthenticationCodeMac},
            {ExtendedBitmapIndicator, ExtendedBitmapIndicator},
            {SettlementCode, SettlementCode},
            {ExtendedPaymentCode, ExtendedPaymentCode},
            {ReceivingInstitutionCountryCode, ReceivingInstitutionCountryCode},
            {SettlementInstitutionCountryCode, SettlementInstitutionCountryCode},
            {NetworkManagementInformationCode, NetworkManagementInformationCode},
            {MessageNumber, MessageNumber},
            {LastMessagesNumber, LastMessagesNumber},
            {ActionDateYymmdd, ActionDateYymmdd},
            {NumberOfCredits, NumberOfCredits},
            {CreditsReversalNumber, CreditsReversalNumber},
            {NumberOfDebits, NumberOfDebits},
            {DebitsReversalNumber, DebitsReversalNumber},
            {TransferNumber, TransferNumber},
            {TransferReversalNumber, TransferReversalNumber},
            {NumberOfInquiries, NumberOfInquiries},
            {NumberOfAuthorizations, NumberOfAuthorizations},
            {CreditsProcessingFeeAmount, CreditsProcessingFeeAmount},
            {CreditsTransactionFeeAmount, CreditsTransactionFeeAmount},
            {DebitsProcessingFeeAmount, DebitsProcessingFeeAmount},
            {DebitsTransactionFeeAmount, DebitsTransactionFeeAmount},
            {TotalAmountOfCredits, TotalAmountOfCredits},
            {CreditsReversalAmount, CreditsReversalAmount},
            {TotalAmountOfDebits, TotalAmountOfDebits},
            {DebitsReversalAmount, DebitsReversalAmount},
            {OriginalDataElements, OriginalDataElements},
            {FileUpdateCode, FileUpdateCode},
            {FileSecurityCode, FileSecurityCode},
            {ResponseIndicator, ResponseIndicator},
            {ServiceIndicator, ServiceIndicator},
            {ReplacementAmounts, ReplacementAmounts},
            {MessageSecurityCode, MessageSecurityCode},
            {NetSettlementAmount, NetSettlementAmount},
            {Payee, Payee},
            {SettlementInstitutionIdentificationCode, SettlementInstitutionIdentificationCode},
            {ReceivingInstitutionIdentificationCode, ReceivingInstitutionIdentificationCode},
            {FileName, FileName},
            {AccountIdentification1, AccountIdentification1},
            {AccountIdentification2, AccountIdentification2},
            {TransactionDescription, TransactionDescription}
        }.ToImmutableSortedDictionary();
    }

    private DataFieldIdTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool TryGet(byte value, out DataFieldIdTypes result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(DataFieldIdTypes x, DataFieldIdTypes y) => x.Equals(y);
    public int GetHashCode(DataFieldIdTypes obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(DataFieldIdTypes registeredApplicationProviderIndicators) =>
        registeredApplicationProviderIndicators._Value;

    #endregion
}