using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel2.Databases;

public sealed class KnownObjects : IEquatable<KnownObjects>, IEqualityComparer<KnownObjects>, IComparable<KnownObjects>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<Tag, KnownObjects> _ValueObjectMap;
    private static readonly EmvCodec _Codec = EmvCodec.GetBerCodec();

    #endregion

    #region Instance Values

    private readonly Tag _Tag;
    private readonly Func<ReadOnlyMemory<byte>, PrimitiveValue> _Decoder;
    public int Count => _ValueObjectMap.Count;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static KnownObjects()
    {
        _ValueObjectMap = new Dictionary<Tag, KnownObjects>
        {
            {AccountType.Tag, new KnownObjects(AccountType.Tag, AccountType.Decode)},
            {AcquirerIdentifier.Tag, new KnownObjects(AcquirerIdentifier.Tag, AcquirerIdentifier.Decode)},
            {
                AdditionalTerminalCapabilities.Tag,
                new KnownObjects(AdditionalTerminalCapabilities.Tag, AdditionalTerminalCapabilities.Decode)
            },
            {AmountAuthorizedNumeric.Tag, new KnownObjects(AmountAuthorizedNumeric.Tag, AmountAuthorizedNumeric.Decode)},
            {AmountOtherNumeric.Tag, new KnownObjects(AmountOtherNumeric.Tag, AmountOtherNumeric.Decode)},
            {
                ApplicationCapabilitiesInformation.Tag,
                new KnownObjects(ApplicationCapabilitiesInformation.Tag, ApplicationCapabilitiesInformation.Decode)
            },
            {ApplicationCryptogram.Tag, new KnownObjects(ApplicationCryptogram.Tag, ApplicationCryptogram.Decode)},
            {ApplicationCurrencyCode.Tag, new KnownObjects(ApplicationCurrencyCode.Tag, ApplicationCurrencyCode.Decode)},
            {ApplicationCurrencyExponent.Tag, new KnownObjects(ApplicationCurrencyExponent.Tag, ApplicationCurrencyExponent.Decode)},
            {ApplicationEffectiveDate.Tag, new KnownObjects(ApplicationEffectiveDate.Tag, ApplicationEffectiveDate.Decode)},
            {ApplicationExpirationDate.Tag, new KnownObjects(ApplicationExpirationDate.Tag, ApplicationExpirationDate.Decode)},
            {ApplicationFileLocator.Tag, new KnownObjects(ApplicationFileLocator.Tag, ApplicationFileLocator.Decode)},
            {
                ApplicationInterchangeProfile.Tag,
                new KnownObjects(ApplicationInterchangeProfile.Tag, ApplicationInterchangeProfile.Decode)
            },
            {ApplicationLabel.Tag, new KnownObjects(ApplicationLabel.Tag, ApplicationLabel.Decode)},
            {ApplicationPan.Tag, new KnownObjects(ApplicationPan.Tag, ApplicationPan.Decode)},
            {ApplicationPanSequenceNumber.Tag, new KnownObjects(ApplicationPanSequenceNumber.Tag, ApplicationPanSequenceNumber.Decode)},
            {ApplicationPreferredName.Tag, new KnownObjects(ApplicationPreferredName.Tag, ApplicationPreferredName.Decode)},
            {ApplicationPriorityIndicator.Tag, new KnownObjects(ApplicationPriorityIndicator.Tag, ApplicationPriorityIndicator.Decode)},
            {
                ApplicationTransactionCounter.Tag,
                new KnownObjects(ApplicationTransactionCounter.Tag, ApplicationTransactionCounter.Decode)
            },
            {ApplicationUsageControl.Tag, new KnownObjects(ApplicationUsageControl.Tag, ApplicationUsageControl.Decode)},
            {ApplicationVersionNumberCard.Tag, new KnownObjects(ApplicationVersionNumberCard.Tag, ApplicationVersionNumberCard.Decode)},
            {
                ApplicationVersionNumberReader.Tag,
                new KnownObjects(ApplicationVersionNumberReader.Tag, ApplicationVersionNumberReader.Decode)
            },
            {BalanceReadAfterGenAc.Tag, new KnownObjects(BalanceReadAfterGenAc.Tag, BalanceReadAfterGenAc.Decode)},
            {CardDataInputCapability.Tag, new KnownObjects(CardDataInputCapability.Tag, CardDataInputCapability.Decode)},
            {
                CardRiskManagementDataObjectList1.Tag,
                new KnownObjects(CardRiskManagementDataObjectList1.Tag, CardRiskManagementDataObjectList1.Decode)
            },
            {
                CardRiskManagementDataObjectList1RelatedData.Tag,
                new KnownObjects(CardRiskManagementDataObjectList1RelatedData.Tag, CardRiskManagementDataObjectList1RelatedData.Decode)
            },
            {CryptogramInformationData.Tag, new KnownObjects(CryptogramInformationData.Tag, CryptogramInformationData.Decode)},
            {
                CardholderVerificationCode3Track1.Tag,
                new KnownObjects(CardholderVerificationCode3Track1.Tag, CardholderVerificationCode3Track1.Decode)
            },
            {
                CardholderVerificationCode3Track2.Tag,
                new KnownObjects(CardholderVerificationCode3Track2.Tag, CardholderVerificationCode3Track2.Decode)
            },
            {CvmCapabilityCvmRequired.Tag, new KnownObjects(CvmCapabilityCvmRequired.Tag, CvmCapabilityCvmRequired.Decode)},
            {CvmCapabilityNoCvmRequired.Tag, new KnownObjects(CvmCapabilityNoCvmRequired.Tag, CvmCapabilityNoCvmRequired.Decode)},
            {CvmList.Tag, new KnownObjects(CvmList.Tag, CvmList.Decode)},
            {CvmResults.Tag, new KnownObjects(CvmResults.Tag, CvmResults.Decode)},
            {DataNeeded.Tag, new KnownObjects(DataNeeded.Tag, DataNeeded.Decode)},
            {DataRecord.Tag, new KnownObjects(DataRecord.Tag, DataRecord.Decode)},
            {DataRecoveryDataObjectList.Tag, new KnownObjects(DataRecoveryDataObjectList.Tag, DataRecoveryDataObjectList.Decode)},
            {
                DataRecoveryDataObjectListRelatedData.Tag,
                new KnownObjects(DataRecoveryDataObjectListRelatedData.Tag, DataRecoveryDataObjectListRelatedData.Decode)
            },
            {
                DataStorageApplicationCryptogramType.Tag,
                new KnownObjects(DataStorageApplicationCryptogramType.Tag, DataStorageApplicationCryptogramType.Decode)
            },
            {DataStorageSlotAvailability.Tag, new KnownObjects(DataStorageSlotAvailability.Tag, DataStorageSlotAvailability.Decode)},
            {DataStorageDataObjectList.Tag, new KnownObjects(DataStorageDataObjectList.Tag, DataStorageDataObjectList.Decode)},
            {DataStorageDigestHash.Tag, new KnownObjects(DataStorageDigestHash.Tag, DataStorageDigestHash.Decode)},
            {DataStorageId.Tag, new KnownObjects(DataStorageId.Tag, DataStorageId.Decode)},
            {DataStorageInputCard.Tag, new KnownObjects(DataStorageInputCard.Tag, DataStorageInputCard.Decode)},
            {DataStorageInputTerminal.Tag, new KnownObjects(DataStorageInputTerminal.Tag, DataStorageInputTerminal.Decode)},
            {
                DataStorageOperatorDataSetCard.Tag,
                new KnownObjects(DataStorageOperatorDataSetCard.Tag, DataStorageOperatorDataSetCard.Decode)
            },
            {
                DataStorageOperatorDataSetInfo.Tag,
                new KnownObjects(DataStorageOperatorDataSetInfo.Tag, DataStorageOperatorDataSetInfo.Decode)
            },
            {
                DataStorageOperatorDataSetInfoForReader.Tag,
                new KnownObjects(DataStorageOperatorDataSetInfoForReader.Tag, DataStorageOperatorDataSetInfoForReader.Decode)
            },
            {
                DataStorageOperatorDataSetTerminal.Tag,
                new KnownObjects(DataStorageOperatorDataSetTerminal.Tag, DataStorageOperatorDataSetTerminal.Decode)
            },
            {
                DataStorageRequestedOperatorId.Tag,
                new KnownObjects(DataStorageRequestedOperatorId.Tag, DataStorageRequestedOperatorId.Decode)
            },
            {
                DataStorageSlotManagementControl.Tag,
                new KnownObjects(DataStorageSlotManagementControl.Tag, DataStorageSlotManagementControl.Decode)
            },
            {DataStorageSummary1.Tag, new KnownObjects(DataStorageSummary1.Tag, DataStorageSummary1.Decode)},
            {DataStorageSummary2.Tag, new KnownObjects(DataStorageSummary2.Tag, DataStorageSummary2.Decode)},
            {DataStorageSummary3.Tag, new KnownObjects(DataStorageSummary3.Tag, DataStorageSummary3.Decode)},
            {DataStorageSummaryStatus.Tag, new KnownObjects(DataStorageSummaryStatus.Tag, DataStorageSummaryStatus.Decode)},
            {
                DataStorageUnpredictableNumber.Tag,
                new KnownObjects(DataStorageUnpredictableNumber.Tag, DataStorageUnpredictableNumber.Decode)
            },
            {
                DataStorageVersionNumberTerminal.Tag,
                new KnownObjects(DataStorageVersionNumberTerminal.Tag, DataStorageVersionNumberTerminal.Decode)
            },
            {DataToSend.Tag, new KnownObjects(DataToSend.Tag, DataToSend.Decode)},
            {DiscretionaryDataCardTrack1.Tag, new KnownObjects(DiscretionaryDataCardTrack1.Tag, DiscretionaryDataCardTrack1.Decode)},
            {DiscretionaryDataCardTrack2.Tag, new KnownObjects(DiscretionaryDataCardTrack2.Tag, DiscretionaryDataCardTrack2.Decode)},
            {DedicatedFileName.Tag, new KnownObjects(DedicatedFileName.Tag, DecodeDedicatedFileName)},
            {
                DefaultUnpredictableNumberDataObjectList.Tag,
                new KnownObjects(DefaultUnpredictableNumberDataObjectList.Tag, DefaultUnpredictableNumberDataObjectList.Decode)
            },
            {
                DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag,
                new KnownObjects(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag,
                                 DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode)
            },
            {DeviceRelayResistanceEntropy.Tag, new KnownObjects(DeviceRelayResistanceEntropy.Tag, DeviceRelayResistanceEntropy.Decode)},
            {DiscretionaryData.Tag, new KnownObjects(DiscretionaryData.Tag, DiscretionaryData.Decode)},
            {ErrorIndication.Tag, new KnownObjects(ErrorIndication.Tag, ErrorIndication.Decode)},
            {HoldTimeValue.Tag, new KnownObjects(HoldTimeValue.Tag, HoldTimeValue.Decode)},
            {IccDynamicNumber.Tag, new KnownObjects(IccDynamicNumber.Tag, IccDynamicNumber.Decode)},
            {IccPublicKeyCertificate.Tag, new KnownObjects(IccPublicKeyCertificate.Tag, IccPublicKeyCertificate.Decode)},
            {IccPublicKeyExponent.Tag, new KnownObjects(IccPublicKeyExponent.Tag, IccPublicKeyExponent.Decode)},
            {IccPublicKeyRemainder.Tag, new KnownObjects(IccPublicKeyRemainder.Tag, IccPublicKeyRemainder.Decode)},
            {IntegratedDataStorageStatus.Tag, new KnownObjects(IntegratedDataStorageStatus.Tag, IntegratedDataStorageStatus.Decode)},
            {InterfaceDeviceSerialNumber.Tag, new KnownObjects(InterfaceDeviceSerialNumber.Tag, InterfaceDeviceSerialNumber.Decode)},
            {IssuerActionCodeDefault.Tag, new KnownObjects(IssuerActionCodeDefault.Tag, IssuerActionCodeDefault.Decode)},
            {IssuerActionCodeDenial.Tag, new KnownObjects(IssuerActionCodeDenial.Tag, IssuerActionCodeDenial.Decode)},
            {IssuerActionCodeOnline.Tag, new KnownObjects(IssuerActionCodeOnline.Tag, IssuerActionCodeOnline.Decode)},
            {IssuerApplicationData.Tag, new KnownObjects(IssuerApplicationData.Tag, IssuerApplicationData.Decode)},
            {IssuerCodeTableIndex.Tag, new KnownObjects(IssuerCodeTableIndex.Tag, IssuerCodeTableIndex.Decode)},
            {IssuerCountryCode.Tag, new KnownObjects(IssuerCountryCode.Tag, IssuerCountryCode.Decode)},
            {IssuerPublicKeyCertificate.Tag, new KnownObjects(IssuerPublicKeyCertificate.Tag, IssuerPublicKeyCertificate.Decode)},
            {IssuerPublicKeyExponent.Tag, new KnownObjects(IssuerPublicKeyExponent.Tag, IssuerPublicKeyExponent.Decode)},
            {IssuerPublicKeyRemainder.Tag, new KnownObjects(IssuerPublicKeyRemainder.Tag, IssuerPublicKeyRemainder.Decode)},
            {KernelConfiguration.Tag, new KnownObjects(KernelConfiguration.Tag, KernelConfiguration.Decode)},
            {KernelId.Tag, new KnownObjects(KernelId.Tag, KernelId.Decode)},
            {LanguagePreference.Tag, new KnownObjects(LanguagePreference.Tag, LanguagePreference.Decode)},
            {LogEntry.Tag, new KnownObjects(LogEntry.Tag, LogEntry.Decode)},
            {
                MagstripeApplicationVersionNumberReader.Tag,
                new KnownObjects(MagstripeApplicationVersionNumberReader.Tag, MagstripeApplicationVersionNumberReader.Decode)
            },
            {
                MagstripeCvmCapabilityCvmRequired.Tag,
                new KnownObjects(MagstripeCvmCapabilityCvmRequired.Tag, MagstripeCvmCapabilityCvmRequired.Decode)
            },
            {
                MagstripeCvmCapabilityNoCvmRequired.Tag,
                new KnownObjects(MagstripeCvmCapabilityNoCvmRequired.Tag, MagstripeCvmCapabilityNoCvmRequired.Decode)
            },
            {
                MaximumRelayResistanceGracePeriod.Tag,
                new KnownObjects(MaximumRelayResistanceGracePeriod.Tag, MaximumRelayResistanceGracePeriod.Decode)
            },
            {
                MaxLifetimeOfTornTransactionLogRecords.Tag,
                new KnownObjects(MaxLifetimeOfTornTransactionLogRecords.Tag, MaxLifetimeOfTornTransactionLogRecords.Decode)
            },
            {
                MaxNumberOfTornTransactionLogRecords.Tag,
                new KnownObjects(MaxNumberOfTornTransactionLogRecords.Tag, MaxNumberOfTornTransactionLogRecords.Decode)
            },
            {
                MaxTimeForProcessingRelayResistanceApdu.Tag,
                new KnownObjects(MaxTimeForProcessingRelayResistanceApdu.Tag, MaxTimeForProcessingRelayResistanceApdu.Decode)
            },
            {
                MeasuredRelayResistanceProcessingTime.Tag,
                new KnownObjects(MeasuredRelayResistanceProcessingTime.Tag, MeasuredRelayResistanceProcessingTime.Decode)
            },
            {MerchantCategoryCode.Tag, new KnownObjects(MerchantCategoryCode.Tag, MerchantCategoryCode.Decode)},
            {MerchantCustomData.Tag, new KnownObjects(MerchantCustomData.Tag, MerchantCustomData.Decode)},
            {MerchantIdentifier.Tag, new KnownObjects(MerchantIdentifier.Tag, MerchantIdentifier.Decode)},
            {MerchantNameAndLocation.Tag, new KnownObjects(MerchantNameAndLocation.Tag, MerchantNameAndLocation.Decode)},
            {MessageHoldTime.Tag, new KnownObjects(MessageHoldTime.Tag, MessageHoldTime.Decode)},
            {
                MinimumRelayResistanceGracePeriod.Tag,
                new KnownObjects(MinimumRelayResistanceGracePeriod.Tag, MinimumRelayResistanceGracePeriod.Decode)
            },
            {
                MinTimeForProcessingRelayResistanceApdu.Tag,
                new KnownObjects(MinTimeForProcessingRelayResistanceApdu.Tag, MinTimeForProcessingRelayResistanceApdu.Decode)
            },
            {MobileSupportIndicator.Tag, new KnownObjects(MobileSupportIndicator.Tag, MobileSupportIndicator.Decode)},
            {
                NumericApplicationTransactionCounterTrack1.Tag,
                new KnownObjects(NumericApplicationTransactionCounterTrack1.Tag, NumericApplicationTransactionCounterTrack1.Decode)
            },
            {
                NumericApplicationTransactionCounterTrack2.Tag,
                new KnownObjects(NumericApplicationTransactionCounterTrack2.Tag, NumericApplicationTransactionCounterTrack2.Decode)
            },
            {OfflineAccumulatorBalance.Tag, new KnownObjects(OfflineAccumulatorBalance.Tag, OfflineAccumulatorBalance.Decode)},
            {OutcomeParameterSet.Tag, new KnownObjects(OutcomeParameterSet.Tag, OutcomeParameterSet.Decode)},
            {PaymentAccountReference.Tag, new KnownObjects(PaymentAccountReference.Tag, PaymentAccountReference.Decode)},
            {PhoneMessageTable.Tag, new KnownObjects(PhoneMessageTable.Tag, PhoneMessageTable.Decode)},
            {
                PosCardholderInteractionInformation.Tag,
                new KnownObjects(PosCardholderInteractionInformation.Tag, PosCardholderInteractionInformation.Decode)
            },
            {
                PositionOfCardVerificationCode3Track1.Tag,
                new KnownObjects(PositionOfCardVerificationCode3Track1.Tag, PositionOfCardVerificationCode3Track1.Decode)
            },
            {
                PositionOfCardVerificationCode3Track2.Tag,
                new KnownObjects(PositionOfCardVerificationCode3Track2.Tag, PositionOfCardVerificationCode3Track2.Decode)
            },
            {PunatcTrack1.Tag, new KnownObjects(PunatcTrack1.Tag, PunatcTrack1.Decode)},
            {PunatcTrack2.Tag, new KnownObjects(PunatcTrack2.Tag, PunatcTrack2.Decode)},
            {PreGenAcPutDataStatus.Tag, new KnownObjects(PreGenAcPutDataStatus.Tag, PreGenAcPutDataStatus.Decode)},
            {PostGenAcPutDataStatus.Tag, new KnownObjects(PostGenAcPutDataStatus.Tag, PostGenAcPutDataStatus.Decode)},
            {ProceedToFirstWriteFlag.Tag, new KnownObjects(ProceedToFirstWriteFlag.Tag, ProceedToFirstWriteFlag.Decode)},
            {
                ProcessingOptionsDataObjectList.Tag,
                new KnownObjects(ProcessingOptionsDataObjectList.Tag, ProcessingOptionsDataObjectList.Decode)
            },
            {
                ProcessingOptionsDataObjectListRelatedData.Tag,
                new KnownObjects(ProcessingOptionsDataObjectListRelatedData.Tag, ProcessingOptionsDataObjectListRelatedData.Decode)
            },
            {ProtectedDataEnvelope1.Tag, new KnownObjects(ProtectedDataEnvelope1.Tag, ProtectedDataEnvelope1.Decode)},
            {ProtectedDataEnvelope2.Tag, new KnownObjects(ProtectedDataEnvelope2.Tag, ProtectedDataEnvelope2.Decode)},
            {ProtectedDataEnvelope3.Tag, new KnownObjects(ProtectedDataEnvelope3.Tag, ProtectedDataEnvelope3.Decode)},
            {ProtectedDataEnvelope4.Tag, new KnownObjects(ProtectedDataEnvelope4.Tag, ProtectedDataEnvelope4.Decode)},
            {ProtectedDataEnvelope5.Tag, new KnownObjects(ProtectedDataEnvelope5.Tag, ProtectedDataEnvelope5.Decode)},
            {ReaderContactlessFloorLimit.Tag, new KnownObjects(ReaderContactlessFloorLimit.Tag, ReaderContactlessFloorLimit.Decode)},
            {
                ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag,
                new KnownObjects(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag,
                                 ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Decode)
            },
            {
                ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag,
                new KnownObjects(ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag,
                                 ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Decode)
            },
            {ReaderCvmRequiredLimit.Tag, new KnownObjects(ReaderCvmRequiredLimit.Tag, ReaderCvmRequiredLimit.Decode)},
            {ReferenceControlParameter.Tag, new KnownObjects(ReferenceControlParameter.Tag, ReferenceControlParameter.Decode)},
            {
                RelayResistanceAccuracyThreshold.Tag,
                new KnownObjects(RelayResistanceAccuracyThreshold.Tag, RelayResistanceAccuracyThreshold.Decode)
            },
            {
                RelayResistanceProtocolCounter.Tag,
                new KnownObjects(RelayResistanceProtocolCounter.Tag, RelayResistanceProtocolCounter.Decode)
            },
            {
                RelayResistanceTransmissionTimeMismatchThreshold.Tag,
                new KnownObjects(RelayResistanceTransmissionTimeMismatchThreshold.Tag,
                                 RelayResistanceTransmissionTimeMismatchThreshold.Decode)
            },
            {
                ResponseMessageTemplateFormat1.Tag,
                new KnownObjects(ResponseMessageTemplateFormat1.Tag, ResponseMessageTemplateFormat1.Decode)
            },
            {SecurityCapability.Tag, new KnownObjects(SecurityCapability.Tag, SecurityCapability.Decode)},
            {ServiceCode.Tag, new KnownObjects(ServiceCode.Tag, ServiceCode.Decode)},
            {SignedDynamicApplicationData.Tag, new KnownObjects(SignedDynamicApplicationData.Tag, SignedDynamicApplicationData.Decode)},
            {
                StaticDataAuthenticationTagList.Tag,
                new KnownObjects(StaticDataAuthenticationTagList.Tag, StaticDataAuthenticationTagList.Decode)
            },
            {TagsToRead.Tag, new KnownObjects(TagsToRead.Tag, TagsToRead.Decode)},
            {TagsToWriteAfterGenAc.Tag, new KnownObjects(TagsToWriteAfterGenAc.Tag, TagsToWriteAfterGenAc.Decode)},
            {TagsToWriteBeforeGenAc.Tag, new KnownObjects(TagsToWriteBeforeGenAc.Tag, TagsToWriteBeforeGenAc.Decode)},
            {TerminalActionCodeDefault.Tag, new KnownObjects(TerminalActionCodeDefault.Tag, TerminalActionCodeDefault.Decode)},
            {TerminalActionCodeDenial.Tag, new KnownObjects(TerminalActionCodeDenial.Tag, TerminalActionCodeDenial.Decode)},
            {TerminalActionCodeOnline.Tag, new KnownObjects(TerminalActionCodeOnline.Tag, TerminalActionCodeOnline.Decode)},
            {TerminalCapabilities.Tag, new KnownObjects(TerminalCapabilities.Tag, TerminalCapabilities.Decode)},
            {TerminalCountryCode.Tag, new KnownObjects(TerminalCountryCode.Tag, TerminalCountryCode.Decode)},
            {
                TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag,
                new KnownObjects(TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag,
                                 TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode)
            },
            {
                TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag,
                new KnownObjects(TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag,
                                 TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode)
            },
            {TerminalIdentification.Tag, new KnownObjects(TerminalIdentification.Tag, TerminalIdentification.Decode)},
            {
                TerminalRelayResistanceEntropy.Tag,
                new KnownObjects(TerminalRelayResistanceEntropy.Tag, TerminalRelayResistanceEntropy.Decode)
            },
            {TerminalRiskManagementData.Tag, new KnownObjects(TerminalRiskManagementData.Tag, TerminalRiskManagementData.Decode)},
            {TerminalType.Tag, new KnownObjects(TerminalType.Tag, TerminalType.Decode)},
            {TerminalVerificationResults.Tag, new KnownObjects(TerminalVerificationResults.Tag, TerminalVerificationResults.Decode)},
            {ThirdPartyData.Tag, new KnownObjects(ThirdPartyData.Tag, ThirdPartyData.Decode)},
            {TimeoutValue.Tag, new KnownObjects(TimeoutValue.Tag, TimeoutValue.Decode)},
            {TornRecord.Tag, new KnownObjects(TornRecord.Tag, TornRecord.Decode)},
            {Track1Data.Tag, new KnownObjects(Track1Data.Tag, Track1Data.Decode)},
            {Track1DiscretionaryData.Tag, new KnownObjects(Track1DiscretionaryData.Tag, Track1DiscretionaryData.Decode)},
            {Track2Data.Tag, new KnownObjects(Track2Data.Tag, Track2Data.Decode)},
            {Track2DiscretionaryData.Tag, new KnownObjects(Track2DiscretionaryData.Tag, Track2DiscretionaryData.Decode)},
            {Track2EquivalentData.Tag, new KnownObjects(Track2EquivalentData.Tag, Track2EquivalentData.Decode)},
            {TransactionCategoryCode.Tag, new KnownObjects(TransactionCategoryCode.Tag, TransactionCategoryCode.Decode)},
            {TransactionCurrencyCode.Tag, new KnownObjects(TransactionCurrencyCode.Tag, TransactionCurrencyCode.Decode)},
            {TransactionCurrencyExponent.Tag, new KnownObjects(TransactionCurrencyExponent.Tag, TransactionCurrencyExponent.Decode)},
            {TransactionDate.Tag, new KnownObjects(TransactionDate.Tag, TransactionDate.Decode)},
            {TransactionTime.Tag, new KnownObjects(TransactionTime.Tag, TransactionTime.Decode)},
            {TransactionType.Tag, new KnownObjects(TransactionType.Tag, TransactionType.Decode)},
            {UnpredictableNumber.Tag, new KnownObjects(UnpredictableNumber.Tag, UnpredictableNumber.Decode)},
            {
                UnpredictableNumberDataObjectList.Tag,
                new KnownObjects(UnpredictableNumberDataObjectList.Tag, UnpredictableNumberDataObjectList.Decode)
            },
            {UnprotectedDataEnvelope1.Tag, new KnownObjects(UnprotectedDataEnvelope1.Tag, UnprotectedDataEnvelope1.Decode)},
            {UnprotectedDataEnvelope2.Tag, new KnownObjects(UnprotectedDataEnvelope2.Tag, UnprotectedDataEnvelope2.Decode)},
            {UnprotectedDataEnvelope3.Tag, new KnownObjects(UnprotectedDataEnvelope3.Tag, UnprotectedDataEnvelope3.Decode)},
            {UnprotectedDataEnvelope4.Tag, new KnownObjects(UnprotectedDataEnvelope4.Tag, UnprotectedDataEnvelope4.Decode)},
            {UnprotectedDataEnvelope5.Tag, new KnownObjects(UnprotectedDataEnvelope5.Tag, UnprotectedDataEnvelope5.Decode)},
            {UserInterfaceRequestData.Tag, new KnownObjects(UserInterfaceRequestData.Tag, UserInterfaceRequestData.Decode)}
        }.ToImmutableSortedDictionary();
    }

    private KnownObjects(Tag tag, Func<ReadOnlyMemory<byte>, PrimitiveValue> decoder)
    {
        _Tag = tag;
        _Decoder = decoder;
    }

    #endregion

    #region Instance Members

    private static DedicatedFileName DecodeDedicatedFileName(ReadOnlyMemory<byte> value) => DedicatedFileName.Decode(value, _Codec);

    public int CompareTo(KnownObjects? other)
    {
        if (other is null)
            return 1;

        return other._Tag.CompareTo(other._Tag);
    }

    public static bool Exists(Tag value) => _ValueObjectMap.ContainsKey(value);

    public static IEnumerator<Tag> GetEnumerator()
    {
        return _ValueObjectMap.Values.Select(a => (Tag) a).GetEnumerator();
    }

    public static bool TryGet(Tag value, out KnownObjects result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(KnownObjects? other) => (other != null) && (_Tag == other._Tag);

    public bool Equals(KnownObjects? x, KnownObjects? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode() => _Tag.GetHashCode();
    public int GetHashCode(KnownObjects obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides
    
    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    public static PrimitiveValue DecodeFirstPrimitiveAtRuntime(ReadOnlyMemory<byte> value)
    {
        TagLength tagLength = _Codec.DecodeTagLength(value.Span); 
        return _ValueObjectMap[tagLength.GetTag()]._Decoder(value[tagLength.GetValueOffset()..]);
    }

    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    public static IEnumerable<PrimitiveValue> DecodePrimitiveSiblingsAtRuntime(ReadOnlyMemory<byte> value)
    {
        EncodedTlvSiblings siblings = _Codec.DecodeChildren(value);
        uint[] tags = siblings.GetTags();

        for (int i = 0; i < siblings.SiblingCount(); i++)
        {
            if (!_ValueObjectMap.ContainsKey(tags[i]))
                continue;

            siblings.TryGetValueOctetsOfChild(tags[i], out ReadOnlyMemory<byte> childContentOctets);

            yield return _ValueObjectMap[tags[i]]._Decoder(childContentOctets); 
        } 
    }


    public static implicit operator Tag(KnownObjects value) => value._Tag;

    public static explicit operator KnownObjects(Tag value)
    {
        if (!TryGet(value, out KnownObjects result))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"The {nameof(KnownObjects)} could not be found from the number supplied to the argument: {value}");
        }

        return result;
    }

    #endregion
}