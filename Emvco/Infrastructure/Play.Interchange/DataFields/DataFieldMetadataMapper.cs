using System.Collections.Immutable;

using Play.Interchange.DataFields;
using Play.Interchange.DataFields._Temp;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.Codecs;

public class DataFieldMetadataMapper
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<DataFieldId, DataFieldMapper> _Values;

    #endregion

    #region Constructor

    static DataFieldMetadataMapper()
    {
        _Values = new Dictionary<DataFieldId, DataFieldMapper>
        {
            {BitmapMapper.DataFieldId, new BitmapMapper()},
            {PrimaryAccountNumberPanMapper.DataFieldId, new PrimaryAccountNumberPanMapper()},
            {ProcessingCodeMapper.DataFieldId, new ProcessingCodeMapper()},
            {AmountTransactionMapper.DataFieldId, new AmountTransactionMapper()},
            {AmountSettlementMapper.DataFieldId, new AmountSettlementMapper()},
            {AmountCardholderBillingMapper.DataFieldId, new AmountCardholderBillingMapper()},
            {TransmissionDateTimeMapper.DataFieldId, new TransmissionDateTimeMapper()},
            {AmountCardholderBillingFeeMapper.DataFieldId, new AmountCardholderBillingFeeMapper()},
            {ConversionRateSettlementMapper.DataFieldId, new ConversionRateSettlementMapper()},
            {ConversionRateCardholderBillingMapper.DataFieldId, new ConversionRateCardholderBillingMapper()},
            {SystemTraceAuditNumberStanMapper.DataFieldId, new SystemTraceAuditNumberStanMapper()},
            {LocalTransactionTimeHhmmssMapper.DataFieldId, new LocalTransactionTimeHhmmssMapper()},
            {LocalTransactionDateMmddMapper.DataFieldId, new LocalTransactionDateMmddMapper()},
            {ExpirationDateMapper.DataFieldId, new ExpirationDateMapper()},
            {SettlementDateMapper.DataFieldId, new SettlementDateMapper()},
            {CurrencyConversionDateMapper.DataFieldId, new CurrencyConversionDateMapper()},
            {CaptureDateMapper.DataFieldId, new CaptureDateMapper()},
            {MerchantTypeOrMerchantCategoryCodeMapper.DataFieldId, new MerchantTypeOrMerchantCategoryCodeMapper()},
            {AcquiringInstitutionCountryCodeMapper.DataFieldId, new AcquiringInstitutionCountryCodeMapper()},
            {PanExtendedCountryCodeMapper.DataFieldId, new PanExtendedCountryCodeMapper()},
            {ForwardingInstitutionCountryCodeMapper.DataFieldId, new ForwardingInstitutionCountryCodeMapper()},
            {PointOfServiceEntryModeMapper.DataFieldId, new PointOfServiceEntryModeMapper()},
            {ApplicationPanSequenceNumberMapper.DataFieldId, new ApplicationPanSequenceNumberMapper()},
            {FunctionCodeMapper.DataFieldId, new FunctionCodeMapper()},
            {PointOfServiceConditionCodeMapper.DataFieldId, new PointOfServiceConditionCodeMapper()},
            {PointOfServiceCaptureCodeMapper.DataFieldId, new PointOfServiceCaptureCodeMapper()},
            {AuthorizingIdentificationResponseLengthMapper.DataFieldId, new AuthorizingIdentificationResponseLengthMapper()},
            {AmountTransactionFeeMapper.DataFieldId, new AmountTransactionFeeMapper()},
            {AmountSettlementFeeMapper.DataFieldId, new AmountSettlementFeeMapper()},
            {AmountTransactionProcessingFeeMapper.DataFieldId, new AmountTransactionProcessingFeeMapper()},
            {AmountSettlementProcessingFeeMapper.DataFieldId, new AmountSettlementProcessingFeeMapper()},
            {AcquiringInstitutionIdentificationCodeMapper.DataFieldId, new AcquiringInstitutionIdentificationCodeMapper()},
            {ForwardingInstitutionIdentificationCodeMapper.DataFieldId, new ForwardingInstitutionIdentificationCodeMapper()},
            {PrimaryAccountNumberExtendedMapper.DataFieldId, new PrimaryAccountNumberExtendedMapper()},
            {Track3DataMapper.DataFieldId, new Track3DataMapper()},
            {RetrievalReferenceNumberMapper.DataFieldId, new RetrievalReferenceNumberMapper()},
            {AuthorizationIdentificationResponseMapper.DataFieldId, new AuthorizationIdentificationResponseMapper()},
            {ResponseCodeMapper.DataFieldId, new ResponseCodeMapper()},
            {ServiceRestrictionCodeMapper.DataFieldId, new ServiceRestrictionCodeMapper()},
            {CardAcceptorTerminalIdentificationMapper.DataFieldId, new CardAcceptorTerminalIdentificationMapper()},
            {CardAcceptorIdentificationCodeMapper.DataFieldId, new CardAcceptorIdentificationCodeMapper()},
            {CardAcceptorNameLocationMapper.DataFieldId, new CardAcceptorNameLocationMapper()},
            {AdditionalResponseDataMapper.DataFieldId, new AdditionalResponseDataMapper()},
            {Track1DataMapper.DataFieldId, new Track1DataMapper()},
            {AdditionalDataIsoMapper.DataFieldId, new AdditionalDataIsoMapper()},
            {AdditionalDataNationalMapper.DataFieldId, new AdditionalDataNationalMapper()},
            {AdditionalDataPrivateMapper.DataFieldId, new AdditionalDataPrivateMapper()},
            {CurrencyCodeTransactionMapper.DataFieldId, new CurrencyCodeTransactionMapper()},
            {CurrencyCodeSettlementMapper.DataFieldId, new CurrencyCodeSettlementMapper()},
            {CurrencyCodeCardholderBillingMapper.DataFieldId, new CurrencyCodeCardholderBillingMapper()},
            {PersonalIdentificationNumberDataMapper.DataFieldId, new PersonalIdentificationNumberDataMapper()},
            {SecurityRelatedControlInformationMapper.DataFieldId, new SecurityRelatedControlInformationMapper()},
            {AdditionalAmountsMapper.DataFieldId, new AdditionalAmountsMapper()},
            {IccDataEmvHavingMultipleTagsMapper.DataFieldId, new IccDataEmvHavingMultipleTagsMapper()},
            {Reserved56.DataFieldId, new Reserved56()},
            {Reserved57.DataFieldId, new Reserved57()},
            {Reserved58.DataFieldId, new Reserved58()},
            {Reserved59.DataFieldId, new Reserved59()},
            {Reserved60.DataFieldId, new Reserved60()},
            {Reserved61.DataFieldId, new Reserved61()},
            {Reserved62.DataFieldId, new Reserved62()},
            {Reserved63.DataFieldId, new Reserved63()},
            {MessageAuthenticationCodeMacMapper.DataFieldId, new MessageAuthenticationCodeMacMapper()},
            {ExtendedBitmapIndicatorMapper.DataFieldId, new ExtendedBitmapIndicatorMapper()},
            {SettlementCodeMapper.DataFieldId, new SettlementCodeMapper()},
            {ExtendedPaymentCodeMapper.DataFieldId, new ExtendedPaymentCodeMapper()},
            {ReceivingInstitutionCountryCodeMapper.DataFieldId, new ReceivingInstitutionCountryCodeMapper()},
            {SettlementInstitutionCountryCodeMapper.DataFieldId, new SettlementInstitutionCountryCodeMapper()},
            {NetworkManagementInformationCodeMapper.DataFieldId, new NetworkManagementInformationCodeMapper()},
            {MessageNumberMapper.DataFieldId, new MessageNumberMapper()},
            {LastMessagesNumberMapper.DataFieldId, new LastMessagesNumberMapper()},
            {ActionDateYymmddMapper.DataFieldId, new ActionDateYymmddMapper()},
            {NumberOfCreditsMapper.DataFieldId, new NumberOfCreditsMapper()},
            {CreditsReversalNumberMapper.DataFieldId, new CreditsReversalNumberMapper()},
            {NumberOfDebitsMapper.DataFieldId, new NumberOfDebitsMapper()},
            {DebitsReversalNumberMapper.DataFieldId, new DebitsReversalNumberMapper()},
            {TransferNumberMapper.DataFieldId, new TransferNumberMapper()},
            {TransferReversalNumberMapper.DataFieldId, new TransferReversalNumberMapper()},
            {NumberOfInquiriesMapper.DataFieldId, new NumberOfInquiriesMapper()},
            {NumberOfAuthorizationsMapper.DataFieldId, new NumberOfAuthorizationsMapper()},
            {CreditsProcessingFeeAmountMapper.DataFieldId, new CreditsProcessingFeeAmountMapper()},
            {CreditsTransactionFeeAmountMapper.DataFieldId, new CreditsTransactionFeeAmountMapper()},
            {DebitsProcessingFeeAmountMapper.DataFieldId, new DebitsProcessingFeeAmountMapper()},
            {DebitsTransactionFeeAmountMapper.DataFieldId, new DebitsTransactionFeeAmountMapper()},
            {TotalAmountOfCreditsMapper.DataFieldId, new TotalAmountOfCreditsMapper()},
            {CreditsReversalAmountMapper.DataFieldId, new CreditsReversalAmountMapper()},
            {TotalAmountOfDebitsMapper.DataFieldId, new TotalAmountOfDebitsMapper()},
            {DebitsReversalAmountMapper.DataFieldId, new DebitsReversalAmountMapper()},
            {OriginalDataElementsMapper.DataFieldId, new OriginalDataElementsMapper()},
            {FileUpdateCodeMapper.DataFieldId, new FileUpdateCodeMapper()},
            {FileSecurityCodeMapper.DataFieldId, new FileSecurityCodeMapper()},
            {ResponseIndicatorMapper.DataFieldId, new ResponseIndicatorMapper()},
            {ServiceIndicatorMapper.DataFieldId, new ServiceIndicatorMapper()},
            {ReplacementAmountsMapper.DataFieldId, new ReplacementAmountsMapper()},
            {MessageSecurityCodeMapper.DataFieldId, new MessageSecurityCodeMapper()},
            {NetSettlementAmountMapper.DataFieldId, new NetSettlementAmountMapper()},
            {PayeeMapper.DataFieldId, new PayeeMapper()},
            {SettlementInstitutionIdentificationCodeMapper.DataFieldId, new SettlementInstitutionIdentificationCodeMapper()},
            {ReceivingInstitutionIdentificationCodeMapper.DataFieldId, new ReceivingInstitutionIdentificationCodeMapper()},
            {FileNameMapper.DataFieldId, new FileNameMapper()},
            {AccountIdentification1Mapper.DataFieldId, new AccountIdentification1Mapper()},
            {AccountIdentification2Mapper.DataFieldId, new AccountIdentification2Mapper()},
            {TransactionDescriptionMapper.DataFieldId, new TransactionDescriptionMapper()}
        }.ToImmutableSortedDictionary();
    }

    #endregion
}