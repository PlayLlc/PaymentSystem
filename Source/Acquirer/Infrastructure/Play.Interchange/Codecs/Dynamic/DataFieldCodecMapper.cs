using System.Collections.Immutable;

using Play.Interchange.Codecs.Dynamic.Fields;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic;

internal class DataFieldCodecMapper
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<DataFieldId, DataFieldCodec> _Values;

    #endregion

    #region Constructor

    static DataFieldCodecMapper()
    {
        _Values = new Dictionary<DataFieldId, DataFieldCodec>
        {
            {BitmapCodec.DataFieldId, new BitmapCodec()},
            {PrimaryAccountNumberPanCodec.DataFieldId, new PrimaryAccountNumberPanCodec()},
            {ProcessingCodeCodec.DataFieldId, new ProcessingCodeCodec()},
            {AmountTransactionCodec.DataFieldId, new AmountTransactionCodec()},
            {AmountSettlementCodec.DataFieldId, new AmountSettlementCodec()},
            {AmountCardholderBillingCodec.DataFieldId, new AmountCardholderBillingCodec()},
            {TransmissionDateTimeCodec.DataFieldId, new TransmissionDateTimeCodec()},
            {AmountCardholderBillingFeeCodec.DataFieldId, new AmountCardholderBillingFeeCodec()},
            {ConversionRateSettlementCodec.DataFieldId, new ConversionRateSettlementCodec()},
            {ConversionRateCardholderBillingCodec.DataFieldId, new ConversionRateCardholderBillingCodec()},
            {SystemTraceAuditNumberStanCodec.DataFieldId, new SystemTraceAuditNumberStanCodec()},
            {LocalTransactionTimeHhmmssCodec.DataFieldId, new LocalTransactionTimeHhmmssCodec()},
            {LocalTransactionDateMmddCodec.DataFieldId, new LocalTransactionDateMmddCodec()},
            {ExpirationDateCodec.DataFieldId, new ExpirationDateCodec()},
            {SettlementDateCodec.DataFieldId, new SettlementDateCodec()},
            {CurrencyConversionDateCodec.DataFieldId, new CurrencyConversionDateCodec()},
            {CaptureDateCodec.DataFieldId, new CaptureDateCodec()},
            {MerchantTypeOrMerchantCategoryCodeCodec.DataFieldId, new MerchantTypeOrMerchantCategoryCodeCodec()},
            {AcquiringInstitutionCountryCodeCodec.DataFieldId, new AcquiringInstitutionCountryCodeCodec()},
            {PanExtendedCountryCodeCodec.DataFieldId, new PanExtendedCountryCodeCodec()},
            {ForwardingInstitutionCountryCodeCodec.DataFieldId, new ForwardingInstitutionCountryCodeCodec()},
            {PointOfServiceEntryModeCodec.DataFieldId, new PointOfServiceEntryModeCodec()},
            {ApplicationPanSequenceNumberCodec.DataFieldId, new ApplicationPanSequenceNumberCodec()},
            {FunctionCodeCodec.DataFieldId, new FunctionCodeCodec()},
            {PointOfServiceConditionCodeCodec.DataFieldId, new PointOfServiceConditionCodeCodec()},
            {PointOfServiceCaptureCodeCodec.DataFieldId, new PointOfServiceCaptureCodeCodec()},
            {AuthorizingIdentificationResponseLengthCodec.DataFieldId, new AuthorizingIdentificationResponseLengthCodec()},
            {AmountTransactionFeeCodec.DataFieldId, new AmountTransactionFeeCodec()},
            {AmountSettlementFeeCodec.DataFieldId, new AmountSettlementFeeCodec()},
            {AmountTransactionProcessingFeeCodec.DataFieldId, new AmountTransactionProcessingFeeCodec()},
            {AmountSettlementProcessingFeeCodec.DataFieldId, new AmountSettlementProcessingFeeCodec()},
            {AcquiringInstitutionIdentificationCodeCodec.DataFieldId, new AcquiringInstitutionIdentificationCodeCodec()},
            {ForwardingInstitutionIdentificationCodeCodec.DataFieldId, new ForwardingInstitutionIdentificationCodeCodec()},
            {PrimaryAccountNumberExtendedCodec.DataFieldId, new PrimaryAccountNumberExtendedCodec()},
            {Track3DataCodec.DataFieldId, new Track3DataCodec()},
            {RetrievalReferenceNumberCodec.DataFieldId, new RetrievalReferenceNumberCodec()},
            {AuthorizationIdentificationResponseCodec.DataFieldId, new AuthorizationIdentificationResponseCodec()},
            {ResponseCodeCodec.DataFieldId, new ResponseCodeCodec()},
            {ServiceRestrictionCodeCodec.DataFieldId, new ServiceRestrictionCodeCodec()},
            {CardAcceptorTerminalIdentificationCodec.DataFieldId, new CardAcceptorTerminalIdentificationCodec()},
            {CardAcceptorIdentificationCodeCodec.DataFieldId, new CardAcceptorIdentificationCodeCodec()},
            {CardAcceptorNameLocationCodec.DataFieldId, new CardAcceptorNameLocationCodec()},
            {AdditionalResponseDataCodec.DataFieldId, new AdditionalResponseDataCodec()},
            {Track1DataCodec.DataFieldId, new Track1DataCodec()},
            {AdditionalDataIsoCodec.DataFieldId, new AdditionalDataIsoCodec()},
            {AdditionalDataNationalCodec.DataFieldId, new AdditionalDataNationalCodec()},
            {AdditionalDataPrivateCodec.DataFieldId, new AdditionalDataPrivateCodec()},
            {CurrencyCodeTransactionCodec.DataFieldId, new CurrencyCodeTransactionCodec()},
            {CurrencyCodeSettlementCodec.DataFieldId, new CurrencyCodeSettlementCodec()},
            {CurrencyCodeCardholderBillingCodec.DataFieldId, new CurrencyCodeCardholderBillingCodec()},
            {PersonalIdentificationNumberDataCodec.DataFieldId, new PersonalIdentificationNumberDataCodec()},
            {SecurityRelatedControlInformationCodec.DataFieldId, new SecurityRelatedControlInformationCodec()},
            {AdditionalAmountsCodec.DataFieldId, new AdditionalAmountsCodec()},
            {IccDataEmvHavingMultipleTagsCodec.DataFieldId, new IccDataEmvHavingMultipleTagsCodec()},
            {Reserved56.DataFieldId, new Reserved56()},
            {Reserved57.DataFieldId, new Reserved57()},
            {Reserved58.DataFieldId, new Reserved58()},
            {Reserved59.DataFieldId, new Reserved59()},
            {Reserved60.DataFieldId, new Reserved60()},
            {Reserved61.DataFieldId, new Reserved61()},
            {Reserved62.DataFieldId, new Reserved62()},
            {Reserved63.DataFieldId, new Reserved63()},
            {MessageAuthenticationCodeMacCodec.DataFieldId, new MessageAuthenticationCodeMacCodec()},
            {ExtendedBitmapIndicatorCodec.DataFieldId, new ExtendedBitmapIndicatorCodec()},
            {SettlementCodeCodec.DataFieldId, new SettlementCodeCodec()},
            {ExtendedPaymentCodeCodec.DataFieldId, new ExtendedPaymentCodeCodec()},
            {ReceivingInstitutionCountryCodeCodec.DataFieldId, new ReceivingInstitutionCountryCodeCodec()},
            {SettlementInstitutionCountryCodeCodec.DataFieldId, new SettlementInstitutionCountryCodeCodec()},
            {NetworkManagementInformationCodeCodec.DataFieldId, new NetworkManagementInformationCodeCodec()},
            {MessageNumberCodec.DataFieldId, new MessageNumberCodec()},
            {LastMessagesNumberCodec.DataFieldId, new LastMessagesNumberCodec()},
            {ActionDateYymmddCodec.DataFieldId, new ActionDateYymmddCodec()},
            {NumberOfCreditsCodec.DataFieldId, new NumberOfCreditsCodec()},
            {CreditsReversalNumberCodec.DataFieldId, new CreditsReversalNumberCodec()},
            {NumberOfDebitsCodec.DataFieldId, new NumberOfDebitsCodec()},
            {DebitsReversalNumberCodec.DataFieldId, new DebitsReversalNumberCodec()},
            {TransferNumberCodec.DataFieldId, new TransferNumberCodec()},
            {TransferReversalNumberCodec.DataFieldId, new TransferReversalNumberCodec()},
            {NumberOfInquiriesCodec.DataFieldId, new NumberOfInquiriesCodec()},
            {NumberOfAuthorizationsCodec.DataFieldId, new NumberOfAuthorizationsCodec()},
            {CreditsProcessingFeeAmountCodec.DataFieldId, new CreditsProcessingFeeAmountCodec()},
            {CreditsTransactionFeeAmountCodec.DataFieldId, new CreditsTransactionFeeAmountCodec()},
            {DebitsProcessingFeeAmountCodec.DataFieldId, new DebitsProcessingFeeAmountCodec()},
            {DebitsTransactionFeeAmountCodec.DataFieldId, new DebitsTransactionFeeAmountCodec()},
            {TotalAmountOfCreditsCodec.DataFieldId, new TotalAmountOfCreditsCodec()},
            {CreditsReversalAmountCodec.DataFieldId, new CreditsReversalAmountCodec()},
            {TotalAmountOfDebitsCodec.DataFieldId, new TotalAmountOfDebitsCodec()},
            {DebitsReversalAmountCodec.DataFieldId, new DebitsReversalAmountCodec()},
            {OriginalDataElementsCodec.DataFieldId, new OriginalDataElementsCodec()},
            {FileUpdateCodeCodec.DataFieldId, new FileUpdateCodeCodec()},
            {FileSecurityCodeCodec.DataFieldId, new FileSecurityCodeCodec()},
            {ResponseIndicatorCodec.DataFieldId, new ResponseIndicatorCodec()},
            {ServiceIndicatorCodec.DataFieldId, new ServiceIndicatorCodec()},
            {ReplacementAmountsCodec.DataFieldId, new ReplacementAmountsCodec()},
            {MessageSecurityCodeCodec.DataFieldId, new MessageSecurityCodeCodec()},
            {NetSettlementAmountCodec.DataFieldId, new NetSettlementAmountCodec()},
            {PayeeCodec.DataFieldId, new PayeeCodec()},
            {SettlementInstitutionIdentificationCodeCodec.DataFieldId, new SettlementInstitutionIdentificationCodeCodec()},
            {ReceivingInstitutionIdentificationCodeCodec.DataFieldId, new ReceivingInstitutionIdentificationCodeCodec()},
            {FileNameCodec.DataFieldId, new FileNameCodec()},
            {AccountIdentification1Codec.DataFieldId, new AccountIdentification1Codec()},
            {AccountIdentification2Codec.DataFieldId, new AccountIdentification2Codec()},
            {TransactionDescriptionCodec.DataFieldId, new TransactionDescriptionCodec()}
        }.ToImmutableSortedDictionary();
    }

    #endregion
}