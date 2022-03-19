using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel2.Databases;

public sealed class KnownObjects : IEquatable<KnownObjects>, IEqualityComparer<KnownObjects>, IComparable<KnownObjects>
{
    private readonly Tag _Tag;
    private readonly Func<ReadOnlyMemory<byte>, PrimitiveValue> _Decoder;
    







    #region Static Metadata

    private static readonly ImmutableSortedDictionary<Tag, KnownObjects> _ValueObjectMap;
    private static readonly KnownObjects _AccountType;
    private static readonly KnownObjects _AcquirerIdentifier;
    private static readonly KnownObjects _AdditionalTerminalCapabilities;
    private static readonly KnownObjects _AmountAuthorizedNumeric;
    private static readonly KnownObjects _AmountOther;
    private static readonly KnownObjects _ApplicationCapabilitiesInformation;
    private static readonly KnownObjects _ApplicationCryptogram;
    private static readonly KnownObjects _ApplicationCurrencyCode;
    private static readonly KnownObjects _ApplicationCurrencyExponent;
    private static readonly KnownObjects _ApplicationEffectiveDate;
    private static readonly KnownObjects _ApplicationExpirationDate;
    private static readonly KnownObjects _ApplicationFileLocator;
    private static readonly KnownObjects _ApplicationInterchangeProfile;
    private static readonly KnownObjects _ApplicationLabel;
    private static readonly KnownObjects _ApplicationPAN;
    private static readonly KnownObjects _ApplicationPANSequenceNumber;
    private static readonly KnownObjects _ApplicationPreferredName;
    private static readonly KnownObjects _ApplicationPriorityIndicator;
    private static readonly KnownObjects _ApplicationTransactionCounter;
    private static readonly KnownObjects _ApplicationUsageControl;
    private static readonly KnownObjects _ApplicationVersionNumberCard;
    private static readonly KnownObjects _ApplicationVersionNumberReader;
    private static readonly KnownObjects _BalanceReadAfterGenAC;
    private static readonly KnownObjects _CardDataInputCapability;
    private static readonly KnownObjects _CardRiskManagementDataObjectsList1;
    private static readonly KnownObjects _CardRiskManagementDataObjectsList1RelatedData;
    private static readonly KnownObjects _CryptogramInformationData;
    private static readonly KnownObjects _Cvc3Track1;
    private static readonly KnownObjects _Cvc3Track2;
    private static readonly KnownObjects _CvmCapabilityCvmRequired;
    private static readonly KnownObjects _CvmCapabilityNoCvmRequired;
    private static readonly KnownObjects _CvmList;
    private static readonly KnownObjects _CvmResults;
    private static readonly KnownObjects _DataNeeded;
    private static readonly KnownObjects _DataRecord;
    private static readonly KnownObjects _DataRecoveryDataObjectsList;
    private static readonly KnownObjects _DataRecoveryDataObjectsListRelatedData;
    private static readonly KnownObjects _DataStorageApplicationCryptogramType;
    private static readonly KnownObjects _DataStorageAvailability;
    private static readonly KnownObjects _DataStorageDataObjectsList;
    private static readonly KnownObjects _DataStorageDigestH;
    private static readonly KnownObjects _DataStorageId;
    private static readonly KnownObjects _DataStorageInputCard;
    private static readonly KnownObjects _DataStorageInputTerm;
    private static readonly KnownObjects _DataStorageOperatorDataSetCard;
    private static readonly KnownObjects _DataStorageOperatorDataSetInfo;
    private static readonly KnownObjects _DataStorageOperatorDataSetInfoForReader;
    private static readonly KnownObjects _DataStorageOperatorDataSetTerm;
    private static readonly KnownObjects _DataStorageRequestedOperatorId;
    private static readonly KnownObjects _DataStorageSlotManagementControl;
    private static readonly KnownObjects _DataStorageSummary1;
    private static readonly KnownObjects _DataStorageSummary2;
    private static readonly KnownObjects _DataStorageSummary3;
    private static readonly KnownObjects _DataStorageSummaryStatus;
    private static readonly KnownObjects _DataStorageUnpredictableNumber;
    private static readonly KnownObjects _DataStorageVnTerm;
    private static readonly KnownObjects _DataToSend;
    private static readonly KnownObjects _DdCardTrack1;
    private static readonly KnownObjects _DdCardTrack2;
    private static readonly KnownObjects _DedicatedFileName;
    private static readonly KnownObjects _DefaultUdol;
    private static readonly KnownObjects _DeviceEstimatedTransmissionTimeForRelayResistanceRAPDU;
    private static readonly KnownObjects _DeviceRelayResistanceEntropy;
    private static readonly KnownObjects _DiscretionaryData;
    private static readonly KnownObjects _ErrorIndication;
    private static readonly KnownObjects _FileControlInformationIssuerDiscretionaryData;
    private static readonly KnownObjects _FileControlInformationProprietaryTemplate;
    private static readonly KnownObjects _FileControlInformationTemplate;
    private static readonly KnownObjects _HoldTimeValue;
    private static readonly KnownObjects _IccDynamicNumber;
    private static readonly KnownObjects _IccPublicKeyCertificate;
    private static readonly KnownObjects _IccPublicKeyExponent;
    private static readonly KnownObjects _IccPublicKeyRemainder;
    private static readonly KnownObjects _IntegratedDataStorageStatus;
    private static readonly KnownObjects _InterfaceDeviceSerialNumber;
    private static readonly KnownObjects _IssuerActionCodeDefault;
    private static readonly KnownObjects _IssuerActionCodeDenial;
    private static readonly KnownObjects _IssuerActionCodeOnline;
    private static readonly KnownObjects _IssuerApplicationData;
    private static readonly KnownObjects _IssuerCodeTableIndex;
    private static readonly KnownObjects _IssuerCountryCode;
    private static readonly KnownObjects _IssuerPublicKeyCertificate;
    private static readonly KnownObjects _IssuerPublicKeyExponent;
    private static readonly KnownObjects _IssuerPublicKeyRemainder;
    private static readonly KnownObjects _KernelConfiguration;
    private static readonly KnownObjects _KernelId;
    private static readonly KnownObjects _LanguagePreference;
    private static readonly KnownObjects _LogEntry;
    private static readonly KnownObjects _MagstripeApplicationVersionNumberReader;
    private static readonly KnownObjects _MagstripeCvmCapabilityCvmRequired;
    private static readonly KnownObjects _MagstripeCvmCapabilityNoCvmRequired;
    private static readonly KnownObjects _MaximumRelayResistanceGracePeriod;
    private static readonly KnownObjects _MaxLifetimeOfTornTransactionLogRecord;
    private static readonly KnownObjects _MaxNumberOfTornTransactionLogRecords;
    private static readonly KnownObjects _MaxTimeForProcessingRelayResistanceApdu;
    private static readonly KnownObjects _MeasuredRelayResistanceProcessingTime;
    private static readonly KnownObjects _MerchantCategoryCode;
    private static readonly KnownObjects _MerchantCustomData;
    private static readonly KnownObjects _MerchantIdentifier;
    private static readonly KnownObjects _MerchantNameandLocation;
    private static readonly KnownObjects _MessageHoldTime;
    private static readonly KnownObjects _MinimumRelayResistanceGracePeriod;
    private static readonly KnownObjects _MinTimeForProcessingRelayResistanceApdu;
    private static readonly KnownObjects _MobileSupportIndicator;
    private static readonly KnownObjects _NApplicationTransactionCounterTrack1;
    private static readonly KnownObjects _NApplicationTransactionCounterTrack2;
    private static readonly KnownObjects _OfflineAccumulatorBalance;
    private static readonly KnownObjects _OutcomeParameterSet;
    private static readonly KnownObjects _PaymentAccountReference;
    private static readonly KnownObjects _PhoneMessageTable;
    private static readonly KnownObjects _PosCardholderInteractionInformation;
    private static readonly KnownObjects _PositionOfCardVerificationCode3Track1;
    private static readonly KnownObjects _PositionOfCardVerificationCode3Track2;
    private static readonly KnownObjects _PunatcTrack1;
    private static readonly KnownObjects _PunatcTrack2;
    private static readonly KnownObjects _PostGenApplicationCryptogramPutDataStatus;
    private static readonly KnownObjects _PreGenApplicationCryptogramPutDataStatus;
    private static readonly KnownObjects _ProceedToFirstWriteFlag;
    private static readonly KnownObjects _ProcessingDataObjectsList;
    private static readonly KnownObjects _ProcessingDataObjectsListRelatedData;
    private static readonly KnownObjects _ProtectedDataEnvelope1;
    private static readonly KnownObjects _ProtectedDataEnvelope2;
    private static readonly KnownObjects _ProtectedDataEnvelope3;
    private static readonly KnownObjects _ProtectedDataEnvelope4;
    private static readonly KnownObjects _ProtectedDataEnvelope5;
    private static readonly KnownObjects _ReaderContactlessFloorLimit;
    private static readonly KnownObjects _ReaderContactlessTransactionLimitNoOnDeviceCvm;
    private static readonly KnownObjects _ReaderContactlessTransactionLimitOnDeviceCvm;
    private static readonly KnownObjects _ReaderCvmRequiredLimit;
    private static readonly KnownObjects _ReadRecordResponseMessageTemplate;
    private static readonly KnownObjects _ReferenceControlParameter;
    private static readonly KnownObjects _RelayResistanceAccuracyThreshold;
    private static readonly KnownObjects _RelayResistanceProtocolCounter;
    private static readonly KnownObjects _RelayResistanceTransmissionTimeMismatchThreshold;
    private static readonly KnownObjects _ResponseMessageTemplateFormat1;
    private static readonly KnownObjects _ResponseMessageTemplateFormat2;
    private static readonly KnownObjects _SecurityCapability;
    private static readonly KnownObjects _ServiceCode;
    private static readonly KnownObjects _SignedDynamicApplicationData;
    private static readonly KnownObjects _StaticDataAuthenticationTagList;
    private static readonly KnownObjects _TagsToRead;
    private static readonly KnownObjects _TagsToWriteAfterGenApplicationCryptogram;
    private static readonly KnownObjects _TagsToWriteBeforeGenApplicationCryptogram;
    private static readonly KnownObjects _TerminalActionCodeDefault;
    private static readonly KnownObjects _TerminalActionCodeDenial;
    private static readonly KnownObjects _TerminalActionCodeOnline;
    private static readonly KnownObjects _TerminalCapabilities;
    private static readonly KnownObjects _TerminalCountryCode;
    private static readonly KnownObjects _TerminalExpectedTransmissionTimeForRelayResistanceCApdu;
    private static readonly KnownObjects _TerminalExpectedTransmissionTimeForRelayResistanceRApdu;
    private static readonly KnownObjects _TerminalIdentification;
    private static readonly KnownObjects _TerminalRelayResistanceEntropy;
    private static readonly KnownObjects _TerminalRiskManagementData;
    private static readonly KnownObjects _TerminalType;
    private static readonly KnownObjects _TerminalVerificationResults;
    private static readonly KnownObjects _ThirdPartyData;
    private static readonly KnownObjects _TimeOutValue;
    private static readonly KnownObjects _TornRecord;
    private static readonly KnownObjects _Track1Data;
    private static readonly KnownObjects _Track1DiscretionaryData;
    private static readonly KnownObjects _Track2Data;
    private static readonly KnownObjects _Track2DiscretionaryData;
    private static readonly KnownObjects _Track2EquivalentData;
    private static readonly KnownObjects _TransactionCategoryCode;
    private static readonly KnownObjects _TransactionCurrencyCode;
    private static readonly KnownObjects _TransactionCurrencyExponent;
    private static readonly KnownObjects _TransactionDate;
    private static readonly KnownObjects _TransactionTime;
    private static readonly KnownObjects _TransactionType;
    private static readonly KnownObjects _UnpredictableNumber;
    private static readonly KnownObjects _UnpredictableNumberDataObjectsList;
    private static readonly KnownObjects _UnprotectedDataEnvelope1;
    private static readonly KnownObjects _UnprotectedDataEnvelope2;
    private static readonly KnownObjects _UnprotectedDataEnvelope3;
    private static readonly KnownObjects _UnprotectedDataEnvelope4;
    private static readonly KnownObjects _UnprotectedDataEnvelope5;
    private static readonly KnownObjects _UserInterfaceRequestData;

    #endregion

    #region Instance Values

    public int Count => _ValueObjectMap.Count;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static KnownObjects()
    {


        _AccountType = new KnownObjects(AccountType.Tag, AccountType.Decode);
        _AcquirerIdentifier = new KnownObjects(AcquirerIdentifier.Tag, AcquirerIdentifier.Decode);
        _AdditionalTerminalCapabilities = new KnownObjects(AdditionalTerminalCapabilities.Tag, AdditionalTerminalCapabilities.Decode);
        _AmountAuthorizedNumeric = new KnownObjects(AmountAuthorizedNumeric.Tag, AmountAuthorizedNumeric.Decode);
        _AmountOther = new KnownObjects(AmountOtherNumeric.Tag, AmountOtherNumeric.Decode);
        _ApplicationCapabilitiesInformation = new KnownObjects(ApplicationCapabilitiesInformation.Tag, ApplicationCapabilitiesInformation.Decode);
        _ApplicationCryptogram = new KnownObjects(ApplicationCryptogram.Tag, ApplicationCryptogram.Decode);
        _ApplicationCurrencyCode = new KnownObjects(ApplicationCurrencyCode.Tag, ApplicationCurrencyCode.Decode);
        _ApplicationCurrencyExponent = new KnownObjects(Ber.DataElements.ApplicationCurrencyExponent.Tag, Ber.DataElements.ApplicationCurrencyExponent.Decode);
        _ApplicationEffectiveDate = new KnownObjects(ApplicationEffectiveDate.Tag, ApplicationEffectiveDate.Decode);
        _ApplicationExpirationDate = new KnownObjects(ApplicationExpirationDate.Tag, ApplicationExpirationDate.Decode);
        _ApplicationFileLocator = new KnownObjects(ApplicationFileLocator.Tag, ApplicationFileLocator.Decode);
        _ApplicationInterchangeProfile = new KnownObjects(ApplicationInterchangeProfile.Tag, ApplicationInterchangeProfile.Decode);
        _ApplicationLabel = new KnownObjects(ApplicationLabel.Tag, ApplicationLabel.Decode);
        _ApplicationPAN = new KnownObjects(ApplicationPan.Tag, ApplicationPan.Decode);
        _ApplicationPANSequenceNumber = new KnownObjects(ApplicationPanSequenceNumber.Tag, ApplicationPanSequenceNumber.Decode);
        _ApplicationPreferredName = new KnownObjects(ApplicationPreferredName.Tag, ApplicationPreferredName.Decode);
        _ApplicationPriorityIndicator = new KnownObjects(ApplicationPriorityIndicator.Tag, ApplicationPriorityIndicator.Decode);
        _ApplicationTransactionCounter = new KnownObjects(ApplicationTransactionCounter.Tag, ApplicationTransactionCounter.Decode);
        _ApplicationUsageControl = new KnownObjects(ApplicationUsageControl.Tag, ApplicationUsageControl.Decode);
        _ApplicationVersionNumberCard = new KnownObjects(ApplicationVersionNumberCard.Tag, ApplicationVersionNumberCard.Decode);
        _ApplicationVersionNumberReader = new KnownObjects(ApplicationVersionNumberReader.Tag, ApplicationVersionNumberReader.Decode);
        _BalanceReadAfterGenAC = new KnownObjects(BalanceReadAfterGenAc.Tag, BalanceReadAfterGenAc.Decode);
        _CardDataInputCapability = new KnownObjects(CardDataInputCapability.Tag, CardDataInputCapability.Decode);
        _CardRiskManagementDataObjectsList1 = new KnownObjects(CardRiskManagementDataObjectList1.Tag, CardRiskManagementDataObjectList1.Decode);
        _CardRiskManagementDataObjectsList1RelatedData = new KnownObjects(CardRiskManagementDataObjectList1RelatedData.Tag, CardRiskManagementDataObjectList1RelatedData.Decode);
        _CryptogramInformationData = new KnownObjects(CryptogramInformationData.Tag, CryptogramInformationData.Decode);
        _Cvc3Track1 = new KnownObjects(CardholderVerificationCode3Track1.Tag, CardholderVerificationCode3Track1.Decode);
        _Cvc3Track2 = new KnownObjects(CardholderVerificationCode3Track2.Tag, CardholderVerificationCode3Track2.Decode);
        _CvmCapabilityCvmRequired = new KnownObjects(CvmCapabilityCvmRequired.Tag, CvmCapabilityCvmRequired.Decode);
        _CvmCapabilityNoCvmRequired = new KnownObjects(CvmCapabilityNoCvmRequired.Tag, CvmCapabilityNoCvmRequired.Decode);
        _CvmList = new KnownObjects(CvmList.Tag, CvmList.Decode);
        _CvmResults = new KnownObjects(CvmResults.Tag, CvmResults.Decode);
        _DataNeeded = new KnownObjects(DataNeeded.Tag, DataNeeded.Decode);
        _DataRecord = new KnownObjects(DataRecord.Tag, DataRecord.Decode);
        _DataRecoveryDataObjectsList = new KnownObjects(Ber.DataElements.DataRecoveryDataObjectList.Tag, Ber.DataElements.DataRecoveryDataObjectList.Decode);
        _DataRecoveryDataObjectsListRelatedData = new KnownObjects(Ber.DataElements.DataRecoveryDataObjectListRelatedData.Tag, Ber.DataElements.DataRecoveryDataObjectListRelatedData.Decode);
        _DataStorageApplicationCryptogramType = new KnownObjects(DataStorageApplicationCryptogramType.Tag, DataStorageApplicationCryptogramType.Decode);
        _DataStorageAvailability = new KnownObjects(DataStorageSlotAvailability.Tag, DataStorageSlotAvailability.Decode);
        _DataStorageDataObjectsList = new KnownObjects(DataStorageDataObjectList.Tag, DataStorageDataObjectList.Decode);
        _DataStorageDigestH = new KnownObjects(DataStorageDigestHash.Tag, DataStorageDigestHash.Decode);
        _DataStorageId = new KnownObjects(DataStorageId.Tag, DataStorageId.Decode);
        _DataStorageInputCard = new KnownObjects(DataStorageInputCard.Tag, DataStorageInputCard.Decode);
        _DataStorageInputTerm = new KnownObjects(DataStorageInputTerminal.Tag, DataStorageInputTerminal.Decode);
        _DataStorageOperatorDataSetCard = new KnownObjects(DataStorageOperatorDataSetCard.Tag, DataStorageOperatorDataSetCard.Decode);
        _DataStorageOperatorDataSetInfo = new KnownObjects(DataStorageOperatorDataSetInfo.Tag, DataStorageOperatorDataSetInfo.Decode);
        _DataStorageOperatorDataSetInfoForReader = new KnownObjects(DataStorageOperatorDataSetInfoForReader.Tag, DataStorageOperatorDataSetInfoForReader.Decode);
        _DataStorageOperatorDataSetTerm = new KnownObjects(DataStorageOperatorDataSetTerminal.Tag, DataStorageOperatorDataSetTerminal.Decode);
        _DataStorageRequestedOperatorId = new KnownObjects(DataStorageRequestedOperatorId.Tag, DataStorageRequestedOperatorId.Decode);
        _DataStorageSlotManagementControl = new KnownObjects(DataStorageSlotManagementControl.Tag, DataStorageSlotManagementControl.Decode);
        _DataStorageSummary1 = new KnownObjects(DataStorageSummary1.Tag, DataStorageSummary1.Decode);
        _DataStorageSummary2 = new KnownObjects(DataStorageSummary2.Tag, DataStorageSummary2.Decode);
        _DataStorageSummary3 = new KnownObjects(DataStorageSummary3.Tag, DataStorageSummary3.Decode);
        _DataStorageSummaryStatus = new KnownObjects(DataStorageSummaryStatus.Tag, DataStorageSummaryStatus.Decode);
        _DataStorageUnpredictableNumber = new KnownObjects(DataStorageUnpredictableNumber.Tag, DataStorageUnpredictableNumber.Decode);
        _DataStorageVnTerm = new KnownObjects(DataStorageVersionNumberTerminal.Tag, DataStorageVersionNumberTerminal.Decode);
        _DataToSend = new KnownObjects(DataToSend.Tag, DataToSend.Decode);
        _DdCardTrack1 = new KnownObjects(Ber.DataElements.DiscretionaryDataCardTrack1.Tag, Ber.DataElements.DiscretionaryDataCardTrack1.Decode);
        _DdCardTrack2 = new KnownObjects(Ber.DataElements.DiscretionaryDataCardTrack2.Tag, Ber.DataElements.DiscretionaryDataCardTrack2.Decode);
        _DedicatedFileName = new KnownObjects(Play.Icc.FileSystem.DedicatedFiles.DedicatedFileName.Tag, DecodeDedicatedFileName);
        _DefaultUdol = new KnownObjects(Ber.DataElements.DefaultUnpredictableNumberDataObjectList.Tag, Ber.DataElements.DefaultUnpredictableNumberDataObjectList.Decode);
        _DeviceEstimatedTransmissionTimeForRelayResistanceRAPDU = new KnownObjects(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag, DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode);
        _DeviceRelayResistanceEntropy = new KnownObjects(DeviceRelayResistanceEntropy.Tag, DeviceRelayResistanceEntropy.Decode);
        _DiscretionaryData = new KnownObjects(DiscretionaryData.Tag, DiscretionaryData.Decode);
        _ErrorIndication = new KnownObjects(ErrorIndication.Tag, ErrorIndication.Decode); 
        _HoldTimeValue = new KnownObjects(HoldTimeValue.Tag, HoldTimeValue.Decode);
        _IccDynamicNumber = new KnownObjects(IccDynamicNumber.Tag, IccDynamicNumber.Decode);
        _IccPublicKeyCertificate = new KnownObjects(IccPublicKeyCertificate.Tag, IccPublicKeyCertificate.Decode);
        _IccPublicKeyExponent = new KnownObjects(IccPublicKeyExponent.Tag, IccPublicKeyExponent.Decode);
        _IccPublicKeyRemainder = new KnownObjects(IccPublicKeyRemainder.Tag, IccPublicKeyRemainder.Decode);
        _IntegratedDataStorageStatus = new KnownObjects(IntegratedDataStorageStatus.Tag, IntegratedDataStorageStatus.Decode);
        _InterfaceDeviceSerialNumber = new KnownObjects(InterfaceDeviceSerialNumber.Tag, InterfaceDeviceSerialNumber.Decode);
        _IssuerActionCodeDefault = new KnownObjects(IssuerActionCodeDefault.Tag, IssuerActionCodeDefault.Decode);
        _IssuerActionCodeDenial = new KnownObjects(IssuerActionCodeDenial.Tag, IssuerActionCodeDenial.Decode);
        _IssuerActionCodeOnline = new KnownObjects(IssuerActionCodeOnline.Tag, IssuerActionCodeOnline.Decode);
        _IssuerApplicationData = new KnownObjects(IssuerApplicationData.Tag, IssuerApplicationData.Decode);
        _IssuerCodeTableIndex = new KnownObjects(IssuerCodeTableIndex.Tag, IssuerCodeTableIndex.Decode);
        _IssuerCountryCode = new KnownObjects(IssuerCountryCode.Tag, IssuerCountryCode.Decode);
        _IssuerPublicKeyCertificate = new KnownObjects(IssuerPublicKeyCertificate.Tag, IssuerPublicKeyCertificate.Decode);
        _IssuerPublicKeyExponent = new KnownObjects(IssuerPublicKeyExponent.Tag, IssuerPublicKeyExponent.Decode);
        _IssuerPublicKeyRemainder = new KnownObjects(IssuerPublicKeyRemainder.Tag, IssuerPublicKeyRemainder.Decode);
        _KernelConfiguration = new KnownObjects(KernelConfiguration.Tag, KernelConfiguration.Decode);
        _KernelId = new KnownObjects(KernelId.Tag, KernelId.Decode);
        _LanguagePreference = new KnownObjects(LanguagePreference.Tag, LanguagePreference.Decode);
        _LogEntry = new KnownObjects(LogEntry.Tag, LogEntry.Decode);
        _MagstripeApplicationVersionNumberReader = new KnownObjects(MagstripeApplicationVersionNumberReader.Tag, MagstripeApplicationVersionNumberReader.Decode);
        _MagstripeCvmCapabilityCvmRequired = new KnownObjects(MagstripeCvmCapabilityCvmRequired.Tag, MagstripeCvmCapabilityCvmRequired.Decode);
        _MagstripeCvmCapabilityNoCvmRequired = new KnownObjects(MagstripeCvmCapabilityNoCvmRequired.Tag, MagstripeCvmCapabilityNoCvmRequired.Decode);
        _MaximumRelayResistanceGracePeriod = new KnownObjects(MaximumRelayResistanceGracePeriod.Tag, MaximumRelayResistanceGracePeriod.Decode);
        _MaxLifetimeOfTornTransactionLogRecord = new KnownObjects(MaxLifetimeOfTornTransactionLogRecords.Tag, MaxLifetimeOfTornTransactionLogRecords.Decode);
        _MaxNumberOfTornTransactionLogRecords = new KnownObjects(MaxNumberOfTornTransactionLogRecords.Tag, MaxNumberOfTornTransactionLogRecords.Decode);
        _MaxTimeForProcessingRelayResistanceApdu = new KnownObjects(MaxTimeForProcessingRelayResistanceApdu.Tag, MaxTimeForProcessingRelayResistanceApdu.Decode);
        _MeasuredRelayResistanceProcessingTime = new KnownObjects(MeasuredRelayResistanceProcessingTime.Tag, MeasuredRelayResistanceProcessingTime.Decode);
        _MerchantCategoryCode = new KnownObjects(MerchantCategoryCode.Tag, MerchantCategoryCode.Decode);
        _MerchantCustomData = new KnownObjects(Ber.DataElements.MerchantCustomData.Tag, Ber.DataElements.MerchantCustomData.Decode);
        _MerchantIdentifier = new KnownObjects(MerchantIdentifier.Tag, MerchantIdentifier.Decode);
        _MerchantNameandLocation = new KnownObjects(MerchantNameAndLocation.Tag, MerchantNameAndLocation.Decode);
        _MessageHoldTime = new KnownObjects(MessageHoldTime.Tag, MessageHoldTime.Decode);
        _MinimumRelayResistanceGracePeriod = new KnownObjects(MinimumRelayResistanceGracePeriod.Tag, MinimumRelayResistanceGracePeriod.Decode);
        _MinTimeForProcessingRelayResistanceApdu = new KnownObjects(MinTimeForProcessingRelayResistanceApdu.Tag, MinTimeForProcessingRelayResistanceApdu.Decode);
        _MobileSupportIndicator = new KnownObjects(Ber.DataElements.MobileSupportIndicator.Tag, Ber.DataElements.MobileSupportIndicator.Decode);
        _NApplicationTransactionCounterTrack1 = new KnownObjects(Ber.DataElements.NumericApplicationTransactionCounterTrack1.Tag, Ber.DataElements.NumericApplicationTransactionCounterTrack1.Decode);
        _NApplicationTransactionCounterTrack2 = new KnownObjects(Ber.DataElements.NumericApplicationTransactionCounterTrack2.Tag, Ber.DataElements.NumericApplicationTransactionCounterTrack2.Decode);
        _OfflineAccumulatorBalance = new KnownObjects(OfflineAccumulatorBalance.Tag, OfflineAccumulatorBalance.Decode);
        _OutcomeParameterSet = new KnownObjects(OutcomeParameterSet.Tag, OutcomeParameterSet.Decode);
        _PaymentAccountReference = new KnownObjects(Ber.DataElements.PaymentAccountReference.Tag, Ber.DataElements.PaymentAccountReference.Decode);
        _PhoneMessageTable = new KnownObjects(Ber.DataElements.PhoneMessageTable.Tag, Ber.DataElements.PhoneMessageTable.Decode);
        _PosCardholderInteractionInformation = new KnownObjects(Ber.DataElements.PosCardholderInteractionInformation.Tag, Ber.DataElements.PosCardholderInteractionInformation.Decode);
        _PositionOfCardVerificationCode3Track1 = new KnownObjects(Ber.DataElements.PositionOfCardVerificationCode3Track1.Tag, Ber.DataElements.PositionOfCardVerificationCode3Track1.Decode);
        _PositionOfCardVerificationCode3Track2 = new KnownObjects(Ber.DataElements.PositionOfCardVerificationCode3Track2.Tag, Ber.DataElements.PositionOfCardVerificationCode3Track2.Decode);
        _PunatcTrack1 = new KnownObjects(PunatcTrack1.Tag, PunatcTrack1.Decode);
        _PunatcTrack2 = new KnownObjects(PunatcTrack2.Tag, PunatcTrack2.Decode);
        _PostGenApplicationCryptogramPutDataStatus = new KnownObjects(Ber.DataElements.PostGenAcPutDataStatus.Tag, Ber.DataElements.PostGenAcPutDataStatus.Decode);
        _PreGenApplicationCryptogramPutDataStatus = new KnownObjects(Ber.DataElements.PreGenAcPutDataStatus.Tag, Ber.DataElements.PreGenAcPutDataStatus.Decode);
        _ProceedToFirstWriteFlag = new KnownObjects(Ber.DataElements.ProceedToFirstWriteFlag.Tag, Ber.DataElements.ProceedToFirstWriteFlag.Decode);
        _ProcessingDataObjectsList = new KnownObjects(Ber.DataElements.ProcessingOptionsDataObjectList.Tag, Ber.DataElements.ProcessingOptionsDataObjectList.Decode);
        _ProcessingDataObjectsListRelatedData = new KnownObjects(Ber.DataElements.ProcessingOptionsDataObjectListRelatedData.Tag, Ber.DataElements.ProcessingOptionsDataObjectListRelatedData.Decode);
        _ProtectedDataEnvelope1 = new KnownObjects(ProtectedDataEnvelope1.Tag, ProtectedDataEnvelope1.Decode);
        _ProtectedDataEnvelope2 = new KnownObjects(ProtectedDataEnvelope2.Tag, ProtectedDataEnvelope2.Decode);
        _ProtectedDataEnvelope3 = new KnownObjects(ProtectedDataEnvelope3.Tag, ProtectedDataEnvelope3.Decode);
        _ProtectedDataEnvelope4 = new KnownObjects(ProtectedDataEnvelope4.Tag, ProtectedDataEnvelope4.Decode);
        _ProtectedDataEnvelope5 = new KnownObjects(ProtectedDataEnvelope5.Tag, ProtectedDataEnvelope5.Decode);
        _ReaderContactlessFloorLimit = new KnownObjects(ReaderContactlessFloorLimit.Tag, ReaderContactlessFloorLimit.Decode);
        _ReaderContactlessTransactionLimitNoOnDeviceCvm = new KnownObjects(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag, ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Decode);
        _ReaderContactlessTransactionLimitOnDeviceCvm = new KnownObjects(ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag, ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Decode);
        _ReaderCvmRequiredLimit = new KnownObjects(ReaderCvmRequiredLimit.Tag, ReaderCvmRequiredLimit.Decode); 
        _ReferenceControlParameter = new KnownObjects(Ber.DataElements.ReferenceControlParameter.Tag, Ber.DataElements.ReferenceControlParameter.Decode);
        _RelayResistanceAccuracyThreshold = new KnownObjects(RelayResistanceAccuracyThreshold.Tag, RelayResistanceAccuracyThreshold.Decode);
        _RelayResistanceProtocolCounter = new KnownObjects(Ber.DataElements.RelayResistanceProtocolCounter.Tag, Ber.DataElements.RelayResistanceProtocolCounter.Decode);
        _RelayResistanceTransmissionTimeMismatchThreshold = new KnownObjects(RelayResistanceTransmissionTimeMismatchThreshold.Tag, RelayResistanceTransmissionTimeMismatchThreshold.Decode);
        _ResponseMessageTemplateFormat1 = new KnownObjects(ResponseMessageTemplateFormat1.Tag, ResponseMessageTemplateFormat1.Decode);
         _SecurityCapability = new KnownObjects(SecurityCapability.Tag, SecurityCapability.Decode);
        _ServiceCode = new KnownObjects(Ber.DataElements.ServiceCode.Tag, Ber.DataElements.ServiceCode.Decode);
        _SignedDynamicApplicationData = new KnownObjects(Ber.DataElements.SignedDynamicApplicationData.Tag, Ber.DataElements.SignedDynamicApplicationData.Decode);
        _StaticDataAuthenticationTagList = new KnownObjects(StaticDataAuthenticationTagList.Tag, StaticDataAuthenticationTagList.Decode);
        _TagsToRead = new KnownObjects(TagsToRead.Tag, TagsToRead.Decode);
        _TagsToWriteAfterGenApplicationCryptogram = new KnownObjects(Ber.DataElements.TagsToWriteAfterGenAc.Tag, Ber.DataElements.TagsToWriteAfterGenAc.Decode);
        _TagsToWriteBeforeGenApplicationCryptogram = new KnownObjects(Ber.DataElements.TagsToWriteBeforeGenAc.Tag, Ber.DataElements.TagsToWriteBeforeGenAc.Decode);
        _TerminalActionCodeDefault = new KnownObjects(TerminalActionCodeDefault.Tag, TerminalActionCodeDefault.Decode);
        _TerminalActionCodeDenial = new KnownObjects(TerminalActionCodeDenial.Tag, TerminalActionCodeDenial.Decode);
        _TerminalActionCodeOnline = new KnownObjects(TerminalActionCodeOnline.Tag, TerminalActionCodeOnline.Decode);
        _TerminalCapabilities = new KnownObjects(TerminalCapabilities.Tag, TerminalCapabilities.Decode);
        _TerminalCountryCode = new KnownObjects(TerminalCountryCode.Tag, TerminalCountryCode.Decode);
        _TerminalExpectedTransmissionTimeForRelayResistanceCApdu = new KnownObjects(Ber.DataElements.TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag, Ber.DataElements.TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode);
        _TerminalExpectedTransmissionTimeForRelayResistanceRApdu = new KnownObjects(Ber.DataElements.TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag, Ber.DataElements.TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Decode);
        _TerminalIdentification = new KnownObjects(TerminalIdentification.Tag, TerminalIdentification.Decode);
        _TerminalRelayResistanceEntropy = new KnownObjects(TerminalRelayResistanceEntropy.Tag, TerminalRelayResistanceEntropy.Decode);
        _TerminalRiskManagementData = new KnownObjects(TerminalRiskManagementData.Tag, TerminalRiskManagementData.Decode);
        _TerminalType = new KnownObjects(TerminalType.Tag, TerminalType.Decode);
        _TerminalVerificationResults = new KnownObjects(TerminalVerificationResults.Tag, TerminalVerificationResults.Decode);
        _ThirdPartyData = new KnownObjects(ThirdPartyData.Tag, ThirdPartyData.Decode);
        _TimeOutValue = new KnownObjects(Ber.DataElements.TimeoutValue.Tag, Ber.DataElements.TimeoutValue.Decode);
        _TornRecord = new KnownObjects(TornRecord.Tag, TornRecord.Decode);
        _Track1Data = new KnownObjects(Track1Data.Tag, Track1Data.Decode);
        _Track1DiscretionaryData = new KnownObjects(Track1DiscretionaryData.Tag, Track1DiscretionaryData.Decode);
        _Track2Data = new KnownObjects(Track2Data.Tag, Track2Data.Decode);
        _Track2DiscretionaryData = new KnownObjects(Track2DiscretionaryData.Tag, Track2DiscretionaryData.Decode);
        _Track2EquivalentData = new KnownObjects(Track2EquivalentData.Tag, Track2EquivalentData.Decode);
        _TransactionCategoryCode = new KnownObjects(Ber.DataElements.TransactionCategoryCode.Tag, Ber.DataElements.TransactionCategoryCode.Decode);
        _TransactionCurrencyCode = new KnownObjects(TransactionCurrencyCode.Tag, TransactionCurrencyCode.Decode);
        _TransactionCurrencyExponent = new KnownObjects(TransactionCurrencyExponent.Tag, TransactionCurrencyExponent.Decode);
        _TransactionDate = new KnownObjects(TransactionDate.Tag, TransactionDate.Decode);
        _TransactionTime = new KnownObjects(TransactionTime.Tag, TransactionTime.Decode);
        _TransactionType = new KnownObjects(TransactionType.Tag, TransactionType.Decode);
        _UnpredictableNumber = new KnownObjects(UnpredictableNumber.Tag, UnpredictableNumber.Decode);
        _UnpredictableNumberDataObjectsList = new KnownObjects(UnpredictableNumberDataObjectList.Tag, UnpredictableNumberDataObjectList.Decode);
        _UnprotectedDataEnvelope1 = new KnownObjects(UnprotectedDataEnvelope1.Tag, UnprotectedDataEnvelope1.Decode);
        _UnprotectedDataEnvelope2 = new KnownObjects(UnprotectedDataEnvelope2.Tag, UnprotectedDataEnvelope2.Decode);
        _UnprotectedDataEnvelope3 = new KnownObjects(UnprotectedDataEnvelope3.Tag, UnprotectedDataEnvelope3.Decode);
        _UnprotectedDataEnvelope4 = new KnownObjects(UnprotectedDataEnvelope4.Tag, UnprotectedDataEnvelope4.Decode);
        _UnprotectedDataEnvelope5 = new KnownObjects(UnprotectedDataEnvelope5.Tag, UnprotectedDataEnvelope5.Decode);
        _UserInterfaceRequestData = new KnownObjects(UserInterfaceRequestData.Tag, UserInterfaceRequestData.Decode);

        _ValueObjectMap = new Dictionary<Tag, KnownObjects>
        {
           { _AccountType, _AccountType },
{ _AcquirerIdentifier, _AcquirerIdentifier },
{ _AdditionalTerminalCapabilities, _AdditionalTerminalCapabilities },
{ _AmountAuthorizedNumeric, _AmountAuthorizedNumeric },
{ _AmountOther, _AmountOther },
{ _ApplicationCapabilitiesInformation, _ApplicationCapabilitiesInformation },
{ _ApplicationCryptogram, _ApplicationCryptogram },
{ _ApplicationCurrencyCode, _ApplicationCurrencyCode },
{ _ApplicationCurrencyExponent, _ApplicationCurrencyExponent },
{ _ApplicationEffectiveDate, _ApplicationEffectiveDate },
{ _ApplicationExpirationDate, _ApplicationExpirationDate },
{ _ApplicationFileLocator, _ApplicationFileLocator },
{ _ApplicationInterchangeProfile, _ApplicationInterchangeProfile },
{ _ApplicationLabel, _ApplicationLabel },
{ _ApplicationPAN, _ApplicationPAN },
{ _ApplicationPANSequenceNumber, _ApplicationPANSequenceNumber },
{ _ApplicationPreferredName, _ApplicationPreferredName },
{ _ApplicationPriorityIndicator, _ApplicationPriorityIndicator },
{ _ApplicationTransactionCounter, _ApplicationTransactionCounter },
{ _ApplicationUsageControl, _ApplicationUsageControl },
{ _ApplicationVersionNumberCard, _ApplicationVersionNumberCard },
{ _ApplicationVersionNumberReader, _ApplicationVersionNumberReader },
{ _BalanceReadAfterGenAC, _BalanceReadAfterGenAC },
{ _CardDataInputCapability, _CardDataInputCapability },
{ _CardRiskManagementDataObjectsList1, _CardRiskManagementDataObjectsList1 },
{ _CardRiskManagementDataObjectsList1RelatedData, _CardRiskManagementDataObjectsList1RelatedData },
{ _CryptogramInformationData, _CryptogramInformationData },
{ _Cvc3Track1, _Cvc3Track1 },
{ _Cvc3Track2, _Cvc3Track2 },
{ _CvmCapabilityCvmRequired, _CvmCapabilityCvmRequired },
{ _CvmCapabilityNoCvmRequired, _CvmCapabilityNoCvmRequired },
{ _CvmList, _CvmList },
{ _CvmResults, _CvmResults },
{ _DataNeeded, _DataNeeded },
{ _DataRecord, _DataRecord },
{ _DataRecoveryDataObjectsList, _DataRecoveryDataObjectsList },
{ _DataRecoveryDataObjectsListRelatedData, _DataRecoveryDataObjectsListRelatedData },
{ _DataStorageApplicationCryptogramType, _DataStorageApplicationCryptogramType },
{ _DataStorageAvailability, _DataStorageAvailability },
{ _DataStorageDataObjectsList, _DataStorageDataObjectsList },
{ _DataStorageDigestH, _DataStorageDigestH },
{ _DataStorageId, _DataStorageId },
{ _DataStorageInputCard, _DataStorageInputCard },
{ _DataStorageInputTerm, _DataStorageInputTerm },
{ _DataStorageOperatorDataSetCard, _DataStorageOperatorDataSetCard },
{ _DataStorageOperatorDataSetInfo, _DataStorageOperatorDataSetInfo },
{ _DataStorageOperatorDataSetInfoForReader, _DataStorageOperatorDataSetInfoForReader },
{ _DataStorageOperatorDataSetTerm, _DataStorageOperatorDataSetTerm },
{ _DataStorageRequestedOperatorId, _DataStorageRequestedOperatorId },
{ _DataStorageSlotManagementControl, _DataStorageSlotManagementControl },
{ _DataStorageSummary1, _DataStorageSummary1 },
{ _DataStorageSummary2, _DataStorageSummary2 },
{ _DataStorageSummary3, _DataStorageSummary3 },
{ _DataStorageSummaryStatus, _DataStorageSummaryStatus },
{ _DataStorageUnpredictableNumber, _DataStorageUnpredictableNumber },
{ _DataStorageVnTerm, _DataStorageVnTerm },
{ _DataToSend, _DataToSend },
{ _DdCardTrack1, _DdCardTrack1 },
{ _DdCardTrack2, _DdCardTrack2 },
{ _DedicatedFileName, _DedicatedFileName },
{ _DefaultUdol, _DefaultUdol },
{ _DeviceEstimatedTransmissionTimeForRelayResistanceRAPDU, _DeviceEstimatedTransmissionTimeForRelayResistanceRAPDU },
{ _DeviceRelayResistanceEntropy, _DeviceRelayResistanceEntropy },
{ _DiscretionaryData, _DiscretionaryData },
{ _ErrorIndication, _ErrorIndication },
{ _FileControlInformationIssuerDiscretionaryData, _FileControlInformationIssuerDiscretionaryData },
{ _FileControlInformationProprietaryTemplate, _FileControlInformationProprietaryTemplate },
{ _FileControlInformationTemplate, _FileControlInformationTemplate },
{ _HoldTimeValue, _HoldTimeValue },
{ _IccDynamicNumber, _IccDynamicNumber },
{ _IccPublicKeyCertificate, _IccPublicKeyCertificate },
{ _IccPublicKeyExponent, _IccPublicKeyExponent },
{ _IccPublicKeyRemainder, _IccPublicKeyRemainder },
{ _IntegratedDataStorageStatus, _IntegratedDataStorageStatus },
{ _InterfaceDeviceSerialNumber, _InterfaceDeviceSerialNumber },
{ _IssuerActionCodeDefault, _IssuerActionCodeDefault },
{ _IssuerActionCodeDenial, _IssuerActionCodeDenial },
{ _IssuerActionCodeOnline, _IssuerActionCodeOnline },
{ _IssuerApplicationData, _IssuerApplicationData },
{ _IssuerCodeTableIndex, _IssuerCodeTableIndex },
{ _IssuerCountryCode, _IssuerCountryCode },
{ _IssuerPublicKeyCertificate, _IssuerPublicKeyCertificate },
{ _IssuerPublicKeyExponent, _IssuerPublicKeyExponent },
{ _IssuerPublicKeyRemainder, _IssuerPublicKeyRemainder },
{ _KernelConfiguration, _KernelConfiguration },
{ _KernelId, _KernelId },
{ _LanguagePreference, _LanguagePreference },
{ _LogEntry, _LogEntry },
{ _MagstripeApplicationVersionNumberReader, _MagstripeApplicationVersionNumberReader },
{ _MagstripeCvmCapabilityCvmRequired, _MagstripeCvmCapabilityCvmRequired },
{ _MagstripeCvmCapabilityNoCvmRequired, _MagstripeCvmCapabilityNoCvmRequired },
{ _MaximumRelayResistanceGracePeriod, _MaximumRelayResistanceGracePeriod },
{ _MaxLifetimeOfTornTransactionLogRecord, _MaxLifetimeOfTornTransactionLogRecord },
{ _MaxNumberOfTornTransactionLogRecords, _MaxNumberOfTornTransactionLogRecords },
{ _MaxTimeForProcessingRelayResistanceApdu, _MaxTimeForProcessingRelayResistanceApdu },
{ _MeasuredRelayResistanceProcessingTime, _MeasuredRelayResistanceProcessingTime },
{ _MerchantCategoryCode, _MerchantCategoryCode },
{ _MerchantCustomData, _MerchantCustomData },
{ _MerchantIdentifier, _MerchantIdentifier },
{ _MerchantNameandLocation, _MerchantNameandLocation },
{ _MessageHoldTime, _MessageHoldTime },
{ _MinimumRelayResistanceGracePeriod, _MinimumRelayResistanceGracePeriod },
{ _MinTimeForProcessingRelayResistanceApdu, _MinTimeForProcessingRelayResistanceApdu },
{ _MobileSupportIndicator, _MobileSupportIndicator },
{ _NApplicationTransactionCounterTrack1, _NApplicationTransactionCounterTrack1 },
{ _NApplicationTransactionCounterTrack2, _NApplicationTransactionCounterTrack2 },
{ _OfflineAccumulatorBalance, _OfflineAccumulatorBalance },
{ _OutcomeParameterSet, _OutcomeParameterSet },
{ _PaymentAccountReference, _PaymentAccountReference },
{ _PhoneMessageTable, _PhoneMessageTable },
{ _PosCardholderInteractionInformation, _PosCardholderInteractionInformation },
{ _PositionOfCardVerificationCode3Track1, _PositionOfCardVerificationCode3Track1 },
{ _PositionOfCardVerificationCode3Track2, _PositionOfCardVerificationCode3Track2 },
{ _PunatcTrack1, _PunatcTrack1 },
{ _PunatcTrack2, _PunatcTrack2 },
{ _PostGenApplicationCryptogramPutDataStatus, _PostGenApplicationCryptogramPutDataStatus },
{ _PreGenApplicationCryptogramPutDataStatus, _PreGenApplicationCryptogramPutDataStatus },
{ _ProceedToFirstWriteFlag, _ProceedToFirstWriteFlag },
{ _ProcessingDataObjectsList, _ProcessingDataObjectsList },
{ _ProcessingDataObjectsListRelatedData, _ProcessingDataObjectsListRelatedData },
{ _ProtectedDataEnvelope1, _ProtectedDataEnvelope1 },
{ _ProtectedDataEnvelope2, _ProtectedDataEnvelope2 },
{ _ProtectedDataEnvelope3, _ProtectedDataEnvelope3 },
{ _ProtectedDataEnvelope4, _ProtectedDataEnvelope4 },
{ _ProtectedDataEnvelope5, _ProtectedDataEnvelope5 },
{ _ReaderContactlessFloorLimit, _ReaderContactlessFloorLimit },
{ _ReaderContactlessTransactionLimitNoOnDeviceCvm, _ReaderContactlessTransactionLimitNoOnDeviceCvm },
{ _ReaderContactlessTransactionLimitOnDeviceCvm, _ReaderContactlessTransactionLimitOnDeviceCvm },
{ _ReaderCvmRequiredLimit, _ReaderCvmRequiredLimit },
{ _ReadRecordResponseMessageTemplate, _ReadRecordResponseMessageTemplate },
{ _ReferenceControlParameter, _ReferenceControlParameter },
{ _RelayResistanceAccuracyThreshold, _RelayResistanceAccuracyThreshold },
{ _RelayResistanceProtocolCounter, _RelayResistanceProtocolCounter },
{ _RelayResistanceTransmissionTimeMismatchThreshold, _RelayResistanceTransmissionTimeMismatchThreshold },
{ _ResponseMessageTemplateFormat1, _ResponseMessageTemplateFormat1 },
{ _ResponseMessageTemplateFormat2, _ResponseMessageTemplateFormat2 },
{ _SecurityCapability, _SecurityCapability },
{ _ServiceCode, _ServiceCode },
{ _SignedDynamicApplicationData, _SignedDynamicApplicationData },
{ _StaticDataAuthenticationTagList, _StaticDataAuthenticationTagList },
{ _TagsToRead, _TagsToRead },
{ _TagsToWriteAfterGenApplicationCryptogram, _TagsToWriteAfterGenApplicationCryptogram },
{ _TagsToWriteBeforeGenApplicationCryptogram, _TagsToWriteBeforeGenApplicationCryptogram },
{ _TerminalActionCodeDefault, _TerminalActionCodeDefault },
{ _TerminalActionCodeDenial, _TerminalActionCodeDenial },
{ _TerminalActionCodeOnline, _TerminalActionCodeOnline },
{ _TerminalCapabilities, _TerminalCapabilities },
{ _TerminalCountryCode, _TerminalCountryCode },
{ _TerminalExpectedTransmissionTimeForRelayResistanceCApdu, _TerminalExpectedTransmissionTimeForRelayResistanceCApdu },
{ _TerminalExpectedTransmissionTimeForRelayResistanceRApdu, _TerminalExpectedTransmissionTimeForRelayResistanceRApdu },
{ _TerminalIdentification, _TerminalIdentification },
{ _TerminalRelayResistanceEntropy, _TerminalRelayResistanceEntropy },
{ _TerminalRiskManagementData, _TerminalRiskManagementData },
{ _TerminalType, _TerminalType },
{ _TerminalVerificationResults, _TerminalVerificationResults },
{ _ThirdPartyData, _ThirdPartyData },
{ _TimeOutValue, _TimeOutValue },
{ _TornRecord, _TornRecord },
{ _Track1Data, _Track1Data },
{ _Track1DiscretionaryData, _Track1DiscretionaryData },
{ _Track2Data, _Track2Data },
{ _Track2DiscretionaryData, _Track2DiscretionaryData },
{ _Track2EquivalentData, _Track2EquivalentData },
{ _TransactionCategoryCode, _TransactionCategoryCode },
{ _TransactionCurrencyCode, _TransactionCurrencyCode },
{ _TransactionCurrencyExponent, _TransactionCurrencyExponent },
{ _TransactionDate, _TransactionDate },
{ _TransactionTime, _TransactionTime },
{ _TransactionType, _TransactionType },
{ _UnpredictableNumber, _UnpredictableNumber },
{ _UnpredictableNumberDataObjectsList, _UnpredictableNumberDataObjectsList },
{ _UnprotectedDataEnvelope1, _UnprotectedDataEnvelope1 },
{ _UnprotectedDataEnvelope2, _UnprotectedDataEnvelope2 },
{ _UnprotectedDataEnvelope3, _UnprotectedDataEnvelope3 },
{ _UnprotectedDataEnvelope4, _UnprotectedDataEnvelope4 },
{ _UnprotectedDataEnvelope5, _UnprotectedDataEnvelope5 },
{ _UserInterfaceRequestData, _UserInterfaceRequestData },
        }.ToImmutableSortedDictionary();
    }

    private KnownObjects(Tag tag, Func<ReadOnlyMemory<byte>, PrimitiveValue> decoder)
    {
        _Tag = tag;
        _Decoder = decoder;
    }

    #endregion

    #region Instance Members

    private static DedicatedFileName DecodeDedicatedFileName(ReadOnlyMemory<byte> value)
    {
        return DedicatedFileName.Decode(value, _Codec);
    }
    private static readonly EmvCodec _Codec = EmvCodec.GetBerCodec();
    public int CompareTo(KnownObjects other) => other._Tag.CompareTo(other._Tag);
    public static bool Exists(Tag value) => _ValueObjectMap.ContainsKey(value);

    public static IEnumerator<Tag> GetEnumerator()
    {
        return _ValueObjectMap.Values.Select(a => (Tag) a).GetEnumerator();
    }

    public static bool TryGet(Tag value, out KnownObjects result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(KnownObjects? other) => other != null && (_Tag == other._Tag);

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

    public static implicit operator Tag(KnownObjects value) =>
        value._Tag;

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