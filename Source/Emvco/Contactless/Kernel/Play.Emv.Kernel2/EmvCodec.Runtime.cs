using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Templates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Ber;

public class EmvRuntimeCodec : EmvCodec, IResolveKnownObjectsAtRuntime
{
    #region Static Metadata

    private static readonly EmvRuntimeCodec _Default = new();
    private static readonly ImmutableDictionary<Tag, Func<ReadOnlyMemory<byte>, PrimitiveValue>> _DecodeHandlers;

    #endregion

    #region Constructor

    /// <exception cref="System.Security.SecurityException"></exception>
    /// <exception cref="ReflectionTypeLoadException"></exception>
    static EmvRuntimeCodec()
    {
        _DecodeHandlers = new Dictionary<Tag, Func<ReadOnlyMemory<byte>, PrimitiveValue>>
        {
            {AccountType.Tag, AccountType.Decode},
            {AcquirerIdentifier.Tag, AcquirerIdentifier.Decode},
            {AdditionalTerminalCapabilities.Tag, AdditionalTerminalCapabilities.Decode},
            {AmountAuthorizedNumeric.Tag, AmountAuthorizedNumeric.Decode},
            {AmountOtherNumeric.Tag, AmountOtherNumeric.Decode},
            {ApplicationCapabilitiesInformation.Tag, ApplicationCapabilitiesInformation.Decode},
            {ApplicationCryptogram.Tag, ApplicationCryptogram.Decode},
            {ApplicationCurrencyCode.Tag, ApplicationCurrencyCode.Decode},
            {ApplicationCurrencyExponent.Tag, ApplicationCurrencyExponent.Decode},
            {ApplicationEffectiveDate.Tag, ApplicationEffectiveDate.Decode},
            {ApplicationExpirationDate.Tag, ApplicationExpirationDate.Decode},
            {ApplicationFileLocator.Tag, ApplicationFileLocator.Decode},
            {ApplicationInterchangeProfile.Tag, ApplicationInterchangeProfile.Decode},
            {ApplicationLabel.Tag, ApplicationLabel.Decode},
            {ApplicationPan.Tag, ApplicationPan.Decode},
            {ApplicationPanSequenceNumber.Tag, ApplicationPanSequenceNumber.Decode},
            {ApplicationPreferredName.Tag, ApplicationPreferredName.Decode},
            {ApplicationPriorityIndicator.Tag, ApplicationPriorityIndicator.Decode},
            {ApplicationTransactionCounter.Tag, ApplicationTransactionCounter.Decode},
            {ApplicationUsageControl.Tag, ApplicationUsageControl.Decode},
            {ApplicationVersionNumberCard.Tag, ApplicationVersionNumberCard.Decode},
            {ApplicationVersionNumberReader.Tag, ApplicationVersionNumberReader.Decode},
            {BalanceReadAfterGenAc.Tag, BalanceReadAfterGenAc.Decode},
            {CardDataInputCapability.Tag, CardDataInputCapability.Decode},
            {CardRiskManagementDataObjectList1.Tag, CardRiskManagementDataObjectList1.Decode},
            {CardRiskManagementDataObjectList1RelatedData.Tag, CardRiskManagementDataObjectList1RelatedData.Decode},
            {CryptogramInformationData.Tag, CryptogramInformationData.Decode},
            {CardholderVerificationCode3Track1.Tag, CardholderVerificationCode3Track1.Decode},
            {CardholderVerificationCode3Track2.Tag, CardholderVerificationCode3Track2.Decode},
            {CvmCapabilityCvmRequired.Tag, CvmCapabilityCvmRequired.Decode},
            {CvmCapabilityNoCvmRequired.Tag, CvmCapabilityNoCvmRequired.Decode},
            {CvmList.Tag, CvmList.Decode},
            {CvmResults.Tag, CvmResults.Decode},
            {DataNeeded.Tag, DataNeeded.Decode},
            {DataRecord.Tag, DataRecord.Decode},
            {DataRecoveryDataObjectList.Tag, DataRecoveryDataObjectList.Decode},
            {DataRecoveryDataObjectListRelatedData.Tag, DataRecoveryDataObjectListRelatedData.Decode},
            {DataStorageApplicationCryptogramType.Tag, DataStorageApplicationCryptogramType.Decode},
            {DataStorageSlotAvailability.Tag, DataStorageSlotAvailability.Decode},
            {DataStorageDataObjectList.Tag, DataStorageDataObjectList.Decode},
            {DataStorageDigestHash.Tag, DataStorageDigestHash.Decode},
            {DataStorageId.Tag, DataStorageId.Decode},
            {DataStorageInputCard.Tag, DataStorageInputCard.Decode},
            {DataStorageInputTerminal.Tag, DataStorageInputTerminal.Decode},
            {DataStorageOperatorDataSetCard.Tag, DataStorageOperatorDataSetCard.Decode},
            {DataStorageOperatorDataSetInfo.Tag, DataStorageOperatorDataSetInfo.Decode},
            {DataStorageOperatorDataSetInfoForReader.Tag, DataStorageOperatorDataSetInfoForReader.Decode},
            {DataStorageOperatorDataSetTerminal.Tag, DataStorageOperatorDataSetTerminal.Decode},
            {DataStorageRequestedOperatorId.Tag, DataStorageRequestedOperatorId.Decode},
            {DataStorageSlotManagementControl.Tag, DataStorageSlotManagementControl.Decode},
            {DataStorageSummary1.Tag, DataStorageSummary1.Decode},
            {DataStorageSummary2.Tag, DataStorageSummary2.Decode},
            {DataStorageSummary3.Tag, DataStorageSummary3.Decode},
            {DataStorageSummaryStatus.Tag, DataStorageSummaryStatus.Decode},
            {DataStorageUnpredictableNumber.Tag, DataStorageUnpredictableNumber.Decode},
            {DataStorageVersionNumberTerminal.Tag, DataStorageVersionNumberTerminal.Decode},
            {DataToSend.Tag, (i) => DataToSend.Decode(_Default, i)},
            {DedicatedFileName.Tag, i => DedicatedFileName.Decode(i, _Default)},
            {Track1DiscretionaryData.Tag, Track1DiscretionaryData.Decode},
            {Track2DiscretionaryData.Tag, Track2DiscretionaryData.Decode},
            {DefaultUnpredictableNumberDataObjectList.Tag, DefaultUnpredictableNumberDataObjectList.Decode},
            {DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag, DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode},
            {DeviceRelayResistanceEntropy.Tag, DeviceRelayResistanceEntropy.Decode},
            {DiscretionaryData.Tag, DiscretionaryData.Decode},
            {ErrorIndication.Tag, ErrorIndication.Decode},
            {HoldTimeValue.Tag, HoldTimeValue.Decode},
            {IccDynamicNumber.Tag, IccDynamicNumber.Decode},
            {IccPublicKeyCertificate.Tag, IccPublicKeyCertificate.Decode},
            {IccPublicKeyExponent.Tag, IccPublicKeyExponent.Decode},
            {IccPublicKeyRemainder.Tag, IccPublicKeyRemainder.Decode},
            {IntegratedDataStorageStatus.Tag, IntegratedDataStorageStatus.Decode},
            {InterfaceDeviceSerialNumber.Tag, InterfaceDeviceSerialNumber.Decode},
            {IssuerActionCodeDefault.Tag, IssuerActionCodeDefault.Decode},
            {IssuerActionCodeDenial.Tag, IssuerActionCodeDenial.Decode},
            {IssuerActionCodeOnline.Tag, IssuerActionCodeOnline.Decode},
            {IssuerApplicationData.Tag, IssuerApplicationData.Decode},
            {IssuerCodeTableIndex.Tag, IssuerCodeTableIndex.Decode},
            {IssuerCountryCode.Tag, IssuerCountryCode.Decode},
            {IssuerPublicKeyCertificate.Tag, IssuerPublicKeyCertificate.Decode},
            {IssuerPublicKeyExponent.Tag, IssuerPublicKeyExponent.Decode},
            {IssuerPublicKeyRemainder.Tag, IssuerPublicKeyRemainder.Decode},
            {KernelConfiguration.Tag, KernelConfiguration.Decode},
            {KernelId.Tag, KernelId.Decode},
            {LanguagePreference.Tag, LanguagePreference.Decode},
            {LogEntry.Tag, LogEntry.Decode},
            {MagstripeApplicationVersionNumberReader.Tag, MagstripeApplicationVersionNumberReader.Decode},
            {MagstripeCvmCapabilityCvmRequired.Tag, MagstripeCvmCapabilityCvmRequired.Decode},
            {MagstripeCvmCapabilityNoCvmRequired.Tag, MagstripeCvmCapabilityNoCvmRequired.Decode},
            {MaximumRelayResistanceGracePeriod.Tag, MaximumRelayResistanceGracePeriod.Decode},
            {MaxLifetimeOfTornTransactionLogRecords.Tag, MaxLifetimeOfTornTransactionLogRecords.Decode},
            {MaxNumberOfTornTransactionLogRecords.Tag, MaxNumberOfTornTransactionLogRecords.Decode},
            {MaxTimeForProcessingRelayResistanceApdu.Tag, MaxTimeForProcessingRelayResistanceApdu.Decode},
            {MeasuredRelayResistanceProcessingTime.Tag, MeasuredRelayResistanceProcessingTime.Decode},
            {MerchantCategoryCode.Tag, MerchantCategoryCode.Decode},
            {MerchantCustomData.Tag, MerchantCustomData.Decode},
            {MerchantIdentifier.Tag, MerchantIdentifier.Decode},
            {MerchantNameAndLocation.Tag, MerchantNameAndLocation.Decode},
            {MessageHoldTime.Tag, MessageHoldTime.Decode},
            {MinimumRelayResistanceGracePeriod.Tag, MinimumRelayResistanceGracePeriod.Decode},
            {MinTimeForProcessingRelayResistanceApdu.Tag, MinTimeForProcessingRelayResistanceApdu.Decode},
            {MobileSupportIndicator.Tag, MobileSupportIndicator.Decode},
            {NumericApplicationTransactionCounterTrack1.Tag, NumericApplicationTransactionCounterTrack1.Decode},
            {NumericApplicationTransactionCounterTrack2.Tag, NumericApplicationTransactionCounterTrack2.Decode},
            {OfflineAccumulatorBalance.Tag, OfflineAccumulatorBalance.Decode},
            {OutcomeParameterSet.Tag, OutcomeParameterSet.Decode},
            {PaymentAccountReference.Tag, PaymentAccountReference.Decode},
            {PhoneMessageTable.Tag, PhoneMessageTable.Decode},
            {PosCardholderInteractionInformation.Tag, PosCardholderInteractionInformation.Decode},
            {PositionOfCardVerificationCode3Track1.Tag, PositionOfCardVerificationCode3Track1.Decode},
            {PositionOfCardVerificationCode3Track2.Tag, PositionOfCardVerificationCode3Track2.Decode},
            {PunatcTrack1.Tag, PunatcTrack1.Decode},
            {PunatcTrack2.Tag, PunatcTrack2.Decode},
            {PreGenAcPutDataStatus.Tag, PreGenAcPutDataStatus.Decode},
            {PostGenAcPutDataStatus.Tag, PostGenAcPutDataStatus.Decode},
            {ProceedToFirstWriteFlag.Tag, ProceedToFirstWriteFlag.Decode},
            {ProcessingOptionsDataObjectList.Tag, ProcessingOptionsDataObjectList.Decode},
            {ProcessingOptionsDataObjectListRelatedData.Tag, ProcessingOptionsDataObjectListRelatedData.Decode},
            {ProtectedDataEnvelope1.Tag, ProtectedDataEnvelope1.Decode},
            {ProtectedDataEnvelope2.Tag, ProtectedDataEnvelope2.Decode},
            {ProtectedDataEnvelope3.Tag, ProtectedDataEnvelope3.Decode},
            {ProtectedDataEnvelope4.Tag, ProtectedDataEnvelope4.Decode},
            {ProtectedDataEnvelope5.Tag, ProtectedDataEnvelope5.Decode},
            {ReaderContactlessFloorLimit.Tag, ReaderContactlessFloorLimit.Decode},
            {ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag, ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Decode},
            {ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag, ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Decode},
            {ReaderCvmRequiredLimit.Tag, ReaderCvmRequiredLimit.Decode},
            {ReferenceControlParameter.Tag, ReferenceControlParameter.Decode},
            {RelayResistanceAccuracyThreshold.Tag, RelayResistanceAccuracyThreshold.Decode},
            {RelayResistanceProtocolCounter.Tag, RelayResistanceProtocolCounter.Decode},
            {RelayResistanceTransmissionTimeMismatchThreshold.Tag, RelayResistanceTransmissionTimeMismatchThreshold.Decode},
            {ResponseMessageTemplateFormat1.Tag, ResponseMessageTemplateFormat1.Decode},
            {SecurityCapability.Tag, SecurityCapability.Decode},
            {ServiceCode.Tag, ServiceCode.Decode},
            {SignedDynamicApplicationData.Tag, SignedDynamicApplicationData.Decode},
            {StaticDataAuthenticationTagList.Tag, StaticDataAuthenticationTagList.Decode},
            {TagsToRead.Tag, TagsToRead.Decode},
            {TagsToWriteAfterGenAc.Tag, TagsToWriteAfterGenAc.Decode},
            {TagsToWriteBeforeGenAc.Tag, TagsToWriteBeforeGenAc.Decode},
            {TerminalActionCodeDefault.Tag, TerminalActionCodeDefault.Decode},
            {TerminalActionCodeDenial.Tag, TerminalActionCodeDenial.Decode},
            {TerminalActionCodeOnline.Tag, TerminalActionCodeOnline.Decode},
            {TerminalCapabilities.Tag, TerminalCapabilities.Decode},
            {TerminalCountryCode.Tag, TerminalCountryCode.Decode},
            {TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag, TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode},
            {TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag, TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode},
            {TerminalIdentification.Tag, TerminalIdentification.Decode},
            {TerminalRelayResistanceEntropy.Tag, TerminalRelayResistanceEntropy.Decode},
            {TerminalRiskManagementData.Tag, TerminalRiskManagementData.Decode},
            {TerminalType.Tag, TerminalType.Decode},
            {TerminalVerificationResults.Tag, TerminalVerificationResults.Decode},
            {ThirdPartyData.Tag, ThirdPartyData.Decode},
            {TimeoutValue.Tag, TimeoutValue.Decode},
            {TornRecord.Tag, TornRecord.Decode},
            {Track1Data.Tag, Track1Data.Decode},
            {Track2Data.Tag, Track2Data.Decode},
            {Track2EquivalentData.Tag, Track2EquivalentData.Decode},
            {TransactionCategoryCode.Tag, TransactionCategoryCode.Decode},
            {TransactionCurrencyCode.Tag, TransactionCurrencyCode.Decode},
            {TransactionCurrencyExponent.Tag, TransactionCurrencyExponent.Decode},
            {TransactionDate.Tag, TransactionDate.Decode},
            {TransactionTime.Tag, TransactionTime.Decode},
            {TransactionType.Tag, TransactionType.Decode},
            {UnpredictableNumber.Tag, UnpredictableNumber.Decode},
            {UnpredictableNumberDataObjectList.Tag, UnpredictableNumberDataObjectList.Decode},
            {UnprotectedDataEnvelope1.Tag, UnprotectedDataEnvelope1.Decode},
            {UnprotectedDataEnvelope2.Tag, UnprotectedDataEnvelope2.Decode},
            {UnprotectedDataEnvelope3.Tag, UnprotectedDataEnvelope3.Decode},
            {UnprotectedDataEnvelope4.Tag, UnprotectedDataEnvelope4.Decode},
            {UnprotectedDataEnvelope5.Tag, UnprotectedDataEnvelope5.Decode},
            {UserInterfaceRequestData.Tag, UserInterfaceRequestData.Decode}
        }.ToImmutableDictionary();
    }

    #endregion

    #region Instance Members

    public new static EmvRuntimeCodec GetCodec() => _Default;

    public bool TryDecodingAtRuntime(Tag tag, ReadOnlyMemory<byte> value, out PrimitiveValue? primitiveValue)
    {
        if (!_DecodeHandlers.TryGetValue(tag, out Func<ReadOnlyMemory<byte>, PrimitiveValue>? handler))
        {
            primitiveValue = null;

            return false;
        }

        primitiveValue = handler(value);

        return true;
    }

    #endregion
}