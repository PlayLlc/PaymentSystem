using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Templates;
using Play.Emv.Kernel.Databases;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel2.Databases;

public sealed record Kernel2KnownObjects : KnownObjects
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<Tag, Kernel2KnownObjects> _ValueObjectMap;

    #endregion

    #region Instance Values

    public static int Count => _ValueObjectMap.Count;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static Kernel2KnownObjects()
    {
        _ValueObjectMap = new Dictionary<Tag, Kernel2KnownObjects>
        {
            {AccountType.Tag, new(AccountType.Tag)},
            {AcquirerIdentifier.Tag, new(AcquirerIdentifier.Tag)},
            {AdditionalTerminalCapabilities.Tag, new(AdditionalTerminalCapabilities.Tag)},
            {AmountAuthorizedNumeric.Tag, new(AmountAuthorizedNumeric.Tag)},
            {AmountOtherNumeric.Tag, new(AmountOtherNumeric.Tag)},
            {ApplicationCapabilitiesInformation.Tag, new(ApplicationCapabilitiesInformation.Tag)},
            {ApplicationCryptogram.Tag, new(ApplicationCryptogram.Tag)},
            {ApplicationCurrencyCode.Tag, new(ApplicationCurrencyCode.Tag)},
            {ApplicationCurrencyExponent.Tag, new(ApplicationCurrencyExponent.Tag)},
            {ApplicationEffectiveDate.Tag, new(ApplicationEffectiveDate.Tag)},
            {ApplicationExpirationDate.Tag, new(ApplicationExpirationDate.Tag)},
            {ApplicationFileLocator.Tag, new(ApplicationFileLocator.Tag)},
            {ApplicationInterchangeProfile.Tag, new(ApplicationInterchangeProfile.Tag)},
            {ApplicationLabel.Tag, new(ApplicationLabel.Tag)},
            {ApplicationPan.Tag, new(ApplicationPan.Tag)},
            {ApplicationPanSequenceNumber.Tag, new(ApplicationPanSequenceNumber.Tag)},
            {ApplicationPreferredName.Tag, new(ApplicationPreferredName.Tag)},
            {ApplicationPriorityIndicator.Tag, new(ApplicationPriorityIndicator.Tag)},
            {ApplicationTransactionCounter.Tag, new(ApplicationTransactionCounter.Tag)},
            {ApplicationUsageControl.Tag, new(ApplicationUsageControl.Tag)},
            {ApplicationVersionNumberCard.Tag, new(ApplicationVersionNumberCard.Tag)},
            {ApplicationVersionNumberReader.Tag, new(ApplicationVersionNumberReader.Tag)},
            {BalanceReadAfterGenAc.Tag, new(BalanceReadAfterGenAc.Tag)},
            {CardDataInputCapability.Tag, new(CardDataInputCapability.Tag)},
            {CardRiskManagementDataObjectList1.Tag, new(CardRiskManagementDataObjectList1.Tag)},
            {CardRiskManagementDataObjectList1RelatedData.Tag, new(CardRiskManagementDataObjectList1RelatedData.Tag)},
            {CryptogramInformationData.Tag, new(CryptogramInformationData.Tag)},
            {CardholderVerificationCode3Track1.Tag, new(CardholderVerificationCode3Track1.Tag)},
            {CardholderVerificationCode3Track2.Tag, new(CardholderVerificationCode3Track2.Tag)},
            {CvmCapabilityCvmRequired.Tag, new(CvmCapabilityCvmRequired.Tag)},
            {CvmCapabilityNoCvmRequired.Tag, new(CvmCapabilityNoCvmRequired.Tag)},
            {CvmList.Tag, new(CvmList.Tag)},
            {CvmResults.Tag, new(CvmResults.Tag)},
            {DataNeeded.Tag, new(DataNeeded.Tag)},
            {DataRecord.Tag, new(DataRecord.Tag)},
            {DataRecoveryDataObjectList.Tag, new(DataRecoveryDataObjectList.Tag)},
            {DataRecoveryDataObjectListRelatedData.Tag, new(DataRecoveryDataObjectListRelatedData.Tag)},
            {DataStorageApplicationCryptogramType.Tag, new(DataStorageApplicationCryptogramType.Tag)},
            {DataStorageSlotAvailability.Tag, new(DataStorageSlotAvailability.Tag)},
            {DataStorageDataObjectList.Tag, new(DataStorageDataObjectList.Tag)},
            {DataStorageDigestHash.Tag, new(DataStorageDigestHash.Tag)},
            {DataStorageId.Tag, new(DataStorageId.Tag)},
            {DataStorageInputCard.Tag, new(DataStorageInputCard.Tag)},
            {DataStorageInputTerminal.Tag, new(DataStorageInputTerminal.Tag)},
            {DataStorageOperatorDataSetCard.Tag, new(DataStorageOperatorDataSetCard.Tag)},
            {DataStorageOperatorDataSetInfo.Tag, new(DataStorageOperatorDataSetInfo.Tag)},
            {DataStorageOperatorDataSetInfoForReader.Tag, new(DataStorageOperatorDataSetInfoForReader.Tag)},
            {DataStorageOperatorDataSetTerminal.Tag, new(DataStorageOperatorDataSetTerminal.Tag)},
            {DataStorageRequestedOperatorId.Tag, new(DataStorageRequestedOperatorId.Tag)},
            {DataStorageSlotManagementControl.Tag, new(DataStorageSlotManagementControl.Tag)},
            {DataStorageSummary1.Tag, new(DataStorageSummary1.Tag)},
            {DataStorageSummary2.Tag, new(DataStorageSummary2.Tag)},
            {DataStorageSummary3.Tag, new(DataStorageSummary3.Tag)},
            {DataStorageSummaryStatus.Tag, new(DataStorageSummaryStatus.Tag)},
            {DataStorageUnpredictableNumber.Tag, new(DataStorageUnpredictableNumber.Tag)},
            {DataStorageVersionNumberTerminal.Tag, new(DataStorageVersionNumberTerminal.Tag)},
            {DataToSend.Tag, new(DataToSend.Tag)},
            {Track1DiscretionaryData.Tag, new(Track1DiscretionaryData.Tag)},
            {Track2DiscretionaryData.Tag, new(Track2DiscretionaryData.Tag)},
            {DedicatedFileName.Tag, new(DedicatedFileName.Tag)},
            {DefaultUnpredictableNumberDataObjectList.Tag, new(DefaultUnpredictableNumberDataObjectList.Tag)},
            {
                DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag,
                new(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag)
            },
            {DeviceRelayResistanceEntropy.Tag, new(DeviceRelayResistanceEntropy.Tag)},
            {DiscretionaryData.Tag, new(DiscretionaryData.Tag)},
            {ErrorIndication.Tag, new(ErrorIndication.Tag)},
            {FileControlInformationProprietaryTemplate.Tag, new(FileControlInformationProprietaryTemplate.Tag)},
            {FileControlInformationTemplate.Tag, new(FileControlInformationTemplate.Tag)},
            {HoldTimeValue.Tag, new(HoldTimeValue.Tag)},
            {IccDynamicNumber.Tag, new(IccDynamicNumber.Tag)},
            {IccPublicKeyCertificate.Tag, new(IccPublicKeyCertificate.Tag)},
            {IccPublicKeyExponent.Tag, new(IccPublicKeyExponent.Tag)},
            {IccPublicKeyRemainder.Tag, new(IccPublicKeyRemainder.Tag)},
            {IntegratedDataStorageStatus.Tag, new(IntegratedDataStorageStatus.Tag)},
            {InterfaceDeviceSerialNumber.Tag, new(InterfaceDeviceSerialNumber.Tag)},
            {IssuerActionCodeDefault.Tag, new(IssuerActionCodeDefault.Tag)},
            {IssuerActionCodeDenial.Tag, new(IssuerActionCodeDenial.Tag)},
            {IssuerActionCodeOnline.Tag, new(IssuerActionCodeOnline.Tag)},
            {IssuerApplicationData.Tag, new(IssuerApplicationData.Tag)},
            {IssuerCodeTableIndex.Tag, new(IssuerCodeTableIndex.Tag)},
            {IssuerCountryCode.Tag, new(IssuerCountryCode.Tag)},
            {IssuerPublicKeyCertificate.Tag, new(IssuerPublicKeyCertificate.Tag)},
            {IssuerPublicKeyExponent.Tag, new(IssuerPublicKeyExponent.Tag)},
            {IssuerPublicKeyRemainder.Tag, new(IssuerPublicKeyRemainder.Tag)},
            {KernelConfiguration.Tag, new(KernelConfiguration.Tag)},
            {KernelId.Tag, new(KernelId.Tag)},
            {LanguagePreference.Tag, new(LanguagePreference.Tag)},
            {LogEntry.Tag, new(LogEntry.Tag)},
            {MagstripeApplicationVersionNumberReader.Tag, new(MagstripeApplicationVersionNumberReader.Tag)},
            {MagstripeCvmCapabilityCvmRequired.Tag, new(MagstripeCvmCapabilityCvmRequired.Tag)},
            {MagstripeCvmCapabilityNoCvmRequired.Tag, new(MagstripeCvmCapabilityNoCvmRequired.Tag)},
            {MaximumRelayResistanceGracePeriod.Tag, new(MaximumRelayResistanceGracePeriod.Tag)},
            {MaxLifetimeOfTornTransactionLogRecords.Tag, new(MaxLifetimeOfTornTransactionLogRecords.Tag)},
            {MaxNumberOfTornTransactionLogRecords.Tag, new(MaxNumberOfTornTransactionLogRecords.Tag)},
            {MaxTimeForProcessingRelayResistanceApdu.Tag, new(MaxTimeForProcessingRelayResistanceApdu.Tag)},
            {MeasuredRelayResistanceProcessingTime.Tag, new(MeasuredRelayResistanceProcessingTime.Tag)},
            {MerchantCategoryCode.Tag, new(MerchantCategoryCode.Tag)},
            {MerchantCustomData.Tag, new(MerchantCustomData.Tag)},
            {MerchantIdentifier.Tag, new(MerchantIdentifier.Tag)},
            {MerchantNameAndLocation.Tag, new(MerchantNameAndLocation.Tag)},
            {MessageHoldTime.Tag, new(MessageHoldTime.Tag)},
            {MinimumRelayResistanceGracePeriod.Tag, new(MinimumRelayResistanceGracePeriod.Tag)},
            {MinTimeForProcessingRelayResistanceApdu.Tag, new(MinTimeForProcessingRelayResistanceApdu.Tag)},
            {MobileSupportIndicator.Tag, new(MobileSupportIndicator.Tag)},
            {NumericApplicationTransactionCounterTrack1.Tag, new(NumericApplicationTransactionCounterTrack1.Tag)},
            {NumericApplicationTransactionCounterTrack2.Tag, new(NumericApplicationTransactionCounterTrack2.Tag)},
            {OfflineAccumulatorBalance.Tag, new(OfflineAccumulatorBalance.Tag)},
            {OutcomeParameterSet.Tag, new(OutcomeParameterSet.Tag)},
            {PaymentAccountReference.Tag, new(PaymentAccountReference.Tag)},
            {PhoneMessageTable.Tag, new(PhoneMessageTable.Tag)},
            {PosCardholderInteractionInformation.Tag, new(PosCardholderInteractionInformation.Tag)},
            {PositionOfCardVerificationCode3Track1.Tag, new(PositionOfCardVerificationCode3Track1.Tag)},
            {PositionOfCardVerificationCode3Track2.Tag, new(PositionOfCardVerificationCode3Track2.Tag)},
            {PunatcTrack1.Tag, new(PunatcTrack1.Tag)},
            {PunatcTrack2.Tag, new(PunatcTrack2.Tag)},
            {PreGenAcPutDataStatus.Tag, new(PreGenAcPutDataStatus.Tag)},
            {PostGenAcPutDataStatus.Tag, new(PostGenAcPutDataStatus.Tag)},
            {ProceedToFirstWriteFlag.Tag, new(ProceedToFirstWriteFlag.Tag)},
            {ProcessingOptionsDataObjectList.Tag, new(ProcessingOptionsDataObjectList.Tag)},
            {ProcessingOptionsDataObjectListRelatedData.Tag, new(ProcessingOptionsDataObjectListRelatedData.Tag)},
            {ProtectedDataEnvelope1.Tag, new(ProtectedDataEnvelope1.Tag)},
            {ProtectedDataEnvelope2.Tag, new(ProtectedDataEnvelope2.Tag)},
            {ProtectedDataEnvelope3.Tag, new(ProtectedDataEnvelope3.Tag)},
            {ProtectedDataEnvelope4.Tag, new(ProtectedDataEnvelope4.Tag)},
            {ProtectedDataEnvelope5.Tag, new(ProtectedDataEnvelope5.Tag)},
            {ReaderContactlessFloorLimit.Tag, new(ReaderContactlessFloorLimit.Tag)},
            {ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag, new(ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag)},
            {ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag, new(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag)},
            {ReaderCvmRequiredLimit.Tag, new(ReaderCvmRequiredLimit.Tag)},
            {ReferenceControlParameter.Tag, new(ReferenceControlParameter.Tag)},
            {RelayResistanceAccuracyThreshold.Tag, new(RelayResistanceAccuracyThreshold.Tag)},
            {RelayResistanceProtocolCounter.Tag, new(RelayResistanceProtocolCounter.Tag)},
            {RelayResistanceTransmissionTimeMismatchThreshold.Tag, new(RelayResistanceTransmissionTimeMismatchThreshold.Tag)},
            {ResponseMessageTemplateFormat1.Tag, new(ResponseMessageTemplateFormat1.Tag)},
            {ResponseMessageTemplateFormat2.Tag, new(ResponseMessageTemplateFormat2.Tag)},
            {SecurityCapability.Tag, new(SecurityCapability.Tag)},
            {ServiceCode.Tag, new(ServiceCode.Tag)},
            {SignedDynamicApplicationData.Tag, new(SignedDynamicApplicationData.Tag)},
            {StaticDataAuthenticationTagList.Tag, new(StaticDataAuthenticationTagList.Tag)},
            {TagsToRead.Tag, new(TagsToRead.Tag)},
            {TagsToWriteAfterGenAc.Tag, new(TagsToWriteAfterGenAc.Tag)},
            {TagsToWriteBeforeGenAc.Tag, new(TagsToWriteBeforeGenAc.Tag)},
            {TerminalActionCodeDefault.Tag, new(TerminalActionCodeDefault.Tag)},
            {TerminalActionCodeDenial.Tag, new(TerminalActionCodeDenial.Tag)},
            {TerminalActionCodeOnline.Tag, new(TerminalActionCodeOnline.Tag)},
            {TerminalCapabilities.Tag, new(TerminalCapabilities.Tag)},
            {TerminalCountryCode.Tag, new(TerminalCountryCode.Tag)},
            {
                TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag,
                new(TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag)
            },
            {
                TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag,
                new(TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag)
            },
            {TerminalIdentification.Tag, new(TerminalIdentification.Tag)},
            {TerminalRelayResistanceEntropy.Tag, new(TerminalRelayResistanceEntropy.Tag)},
            {TerminalRiskManagementData.Tag, new(TerminalRiskManagementData.Tag)},
            {TerminalType.Tag, new(TerminalType.Tag)},
            {TerminalVerificationResults.Tag, new(TerminalVerificationResults.Tag)},
            {ThirdPartyData.Tag, new(ThirdPartyData.Tag)},
            {TimeoutValue.Tag, new(TimeoutValue.Tag)},
            {TornRecord.Tag, new(TornRecord.Tag)},
            {Track1Data.Tag, new(Track1Data.Tag)},
            {Track1DiscretionaryData.Tag, new(Track1DiscretionaryData.Tag)},
            {Track2Data.Tag, new(Track2Data.Tag)},
            {Track2DiscretionaryData.Tag, new(Track2DiscretionaryData.Tag)},
            {Track2EquivalentData.Tag, new(Track2EquivalentData.Tag)},
            {TransactionCategoryCode.Tag, new(TransactionCategoryCode.Tag)},
            {TransactionCurrencyCode.Tag, new(TransactionCurrencyCode.Tag)},
            {TransactionCurrencyExponent.Tag, new(TransactionCurrencyExponent.Tag)},
            {TransactionDate.Tag, new(TransactionDate.Tag)},
            {TransactionTime.Tag, new(TransactionTime.Tag)},
            {TransactionType.Tag, new(TransactionType.Tag)},
            {UnpredictableNumber.Tag, new(UnpredictableNumber.Tag)},
            {UnpredictableNumberDataObjectList.Tag, new(UnpredictableNumberDataObjectList.Tag)},
            {UnprotectedDataEnvelope1.Tag, new(UnprotectedDataEnvelope1.Tag)},
            {UnprotectedDataEnvelope2.Tag, new(UnprotectedDataEnvelope2.Tag)},
            {UnprotectedDataEnvelope3.Tag, new(UnprotectedDataEnvelope3.Tag)},
            {UnprotectedDataEnvelope4.Tag, new(UnprotectedDataEnvelope4.Tag)},
            {UnprotectedDataEnvelope5.Tag, new(UnprotectedDataEnvelope5.Tag)},
            {UserInterfaceRequestData.Tag, new(UserInterfaceRequestData.Tag)}
        }.ToImmutableSortedDictionary();
    }

    public Kernel2KnownObjects()
    { }

    private Kernel2KnownObjects(Tag value) : base(value)
    { }

    #endregion

    #region Equality

    public bool Equals(Kernel2KnownObjects? other) => !(other is null) && (_Value == other._Value);

    public static bool Equals(Kernel2KnownObjects x, Kernel2KnownObjects y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + (_Value.GetHashCode() * 3);
    }

    public static int GetHashCode(Kernel2KnownObjects obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(Kernel2KnownObjects registeredApplicationProviderIndicators) =>
        registeredApplicationProviderIndicators._Value;

    public static explicit operator Kernel2KnownObjects(Tag registeredApplicationProviderIndicator)
    {
        if (!TryGet(registeredApplicationProviderIndicator, out Kernel2KnownObjects result))
        {
            throw new ArgumentOutOfRangeException(nameof(registeredApplicationProviderIndicator),
                                                  $"The {nameof(Kernel2KnownObjects)} could not be found from the number supplied to the argument: {registeredApplicationProviderIndicator}");
        }

        return result;
    }

    #endregion

    #region Instance Members

    public int CompareTo(Kernel2KnownObjects other) => _Value._Value.CompareTo(other._Value);
    public override bool Exists(Tag value) => _ValueObjectMap.ContainsKey(value);

    public static IEnumerator<Tag> GetEnumerator()
    {
        return _ValueObjectMap.Values.Select(a => (Tag) a).GetEnumerator();
    }

    public static bool TryGet(Tag value, out Kernel2KnownObjects result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion
}