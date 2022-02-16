using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Core;

namespace Play.Emv.Kernel2.Databases;

public sealed record KnownObjects : EnumObject<Tag>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<Tag, KnownObjects> _ValueObjectMap;
    public static readonly KnownObjects AccountType;
    public static readonly KnownObjects AcquirerIdentifier;
    public static readonly KnownObjects AdditionalTerminalCapabilities;
    public static readonly KnownObjects AmountAuthorizedNumeric;
    public static readonly KnownObjects AmountOther;
    public static readonly KnownObjects ApplicationCapabilitiesInformation;
    public static readonly KnownObjects ApplicationCryptogram;
    public static readonly KnownObjects ApplicationCurrencyCode;
    public static readonly KnownObjects ApplicationCurrencyExponent;
    public static readonly KnownObjects ApplicationEffectiveDate;
    public static readonly KnownObjects ApplicationExpirationDate;
    public static readonly KnownObjects ApplicationFileLocator;
    public static readonly KnownObjects ApplicationInterchangeProfile;
    public static readonly KnownObjects ApplicationLabel;
    public static readonly KnownObjects ApplicationPAN;
    public static readonly KnownObjects ApplicationPANSequenceNumber;
    public static readonly KnownObjects ApplicationPreferredName;
    public static readonly KnownObjects ApplicationPriorityIndicator;
    public static readonly KnownObjects ApplicationTransactionCounter;
    public static readonly KnownObjects ApplicationUsageControl;
    public static readonly KnownObjects ApplicationVersionNumberCard;
    public static readonly KnownObjects ApplicationVersionNumberReader;
    public static readonly KnownObjects BalanceReadAfterGenAC;
    public static readonly KnownObjects CardDataInputCapability;
    public static readonly KnownObjects CardRiskManagementDataObjectsList1;
    public static readonly KnownObjects CardRiskManagementDataObjectsList1RelatedData;
    public static readonly KnownObjects CryptogramInformationData;
    public static readonly KnownObjects Cvc3Track1;
    public static readonly KnownObjects Cvc3Track2;
    public static readonly KnownObjects CvmCapabilityCvmRequired;
    public static readonly KnownObjects CvmCapabilityNoCvmRequired;
    public static readonly KnownObjects CvmList;
    public static readonly KnownObjects CvmResults;
    public static readonly KnownObjects DataNeeded;
    public static readonly KnownObjects DataRecord;
    public static readonly KnownObjects DataRecoveryDataObjectsList;
    public static readonly KnownObjects DataRecoveryDataObjectsListRelatedData;
    public static readonly KnownObjects DataStorageApplicationCryptogramType;
    public static readonly KnownObjects DataStorageAvailability;
    public static readonly KnownObjects DataStorageDataObjectsList;
    public static readonly KnownObjects DataStorageDigestH;
    public static readonly KnownObjects DataStorageId;
    public static readonly KnownObjects DataStorageInputCard;
    public static readonly KnownObjects DataStorageInputTerm;
    public static readonly KnownObjects DataStorageOperatorDataSetCard;
    public static readonly KnownObjects DataStorageOperatorDataSetInfo;
    public static readonly KnownObjects DataStorageOperatorDataSetInfoForReader;
    public static readonly KnownObjects DataStorageOperatorDataSetTerm;
    public static readonly KnownObjects DataStorageRequestedOperatorId;
    public static readonly KnownObjects DataStorageSlotManagementControl;
    public static readonly KnownObjects DataStorageSummary1;
    public static readonly KnownObjects DataStorageSummary2;
    public static readonly KnownObjects DataStorageSummary3;
    public static readonly KnownObjects DataStorageSummaryStatus;
    public static readonly KnownObjects DataStorageUnpredictableNumber;
    public static readonly KnownObjects DataStorageVnTerm;
    public static readonly KnownObjects DataToSend;
    public static readonly KnownObjects DdCardTrack1;
    public static readonly KnownObjects DdCardTrack2;
    public static readonly KnownObjects DedicatedFileName;
    public static readonly KnownObjects DefaultUdol;
    public static readonly KnownObjects DeviceEstimatedTransmissionTimeForRelayResistanceRAPDU;
    public static readonly KnownObjects DeviceRelayResistanceEntropy;
    public static readonly KnownObjects DiscretionaryData;
    public static readonly KnownObjects ErrorIndication;
    public static readonly KnownObjects FileControlInformationIssuerDiscretionaryData;
    public static readonly KnownObjects FileControlInformationProprietaryTemplate;
    public static readonly KnownObjects FileControlInformationTemplate;
    public static readonly KnownObjects HoldTimeValue;
    public static readonly KnownObjects IccDynamicNumber;
    public static readonly KnownObjects IccPublicKeyCertificate;
    public static readonly KnownObjects IccPublicKeyExponent;
    public static readonly KnownObjects IccPublicKeyRemainder;
    public static readonly KnownObjects IntegratedDataStorageStatus;
    public static readonly KnownObjects InterfaceDeviceSerialNumber;
    public static readonly KnownObjects IssuerActionCodeDefault;
    public static readonly KnownObjects IssuerActionCodeDenial;
    public static readonly KnownObjects IssuerActionCodeOnline;
    public static readonly KnownObjects IssuerApplicationData;
    public static readonly KnownObjects IssuerCodeTableIndex;
    public static readonly KnownObjects IssuerCountryCode;
    public static readonly KnownObjects IssuerPublicKeyCertificate;
    public static readonly KnownObjects IssuerPublicKeyExponent;
    public static readonly KnownObjects IssuerPublicKeyRemainder;
    public static readonly KnownObjects KernelConfiguration;
    public static readonly KnownObjects KernelId;
    public static readonly KnownObjects LanguagePreference;
    public static readonly KnownObjects LogEntry;
    public static readonly KnownObjects MagstripeApplicationVersionNumberReader;
    public static readonly KnownObjects MagstripeCvmCapabilityCvmRequired;
    public static readonly KnownObjects MagstripeCvmCapabilityNoCvmRequired;
    public static readonly KnownObjects MaximumRelayResistanceGracePeriod;
    public static readonly KnownObjects MaxLifetimeOfTornTransactionLogRecord;
    public static readonly KnownObjects MaxNumberOfTornTransactionLogRecords;
    public static readonly KnownObjects MaxTimeForProcessingRelayResistanceApdu;
    public static readonly KnownObjects MeasuredRelayResistanceProcessingTime;
    public static readonly KnownObjects MerchantCategoryCode;
    public static readonly KnownObjects MerchantCustomData;
    public static readonly KnownObjects MerchantIdentifier;
    public static readonly KnownObjects MerchantNameandLocation;
    public static readonly KnownObjects MessageHoldTime;
    public static readonly KnownObjects MinimumRelayResistanceGracePeriod;
    public static readonly KnownObjects MinTimeForProcessingRelayResistanceApdu;
    public static readonly KnownObjects MobileSupportIndicator;
    public static readonly KnownObjects NApplicationTransactionCounterTrack1;
    public static readonly KnownObjects NApplicationTransactionCounterTrack2;
    public static readonly KnownObjects OfflineAccumulatorBalance;
    public static readonly KnownObjects OutcomeParameterSet;
    public static readonly KnownObjects PaymentAccountReference;
    public static readonly KnownObjects PhoneMessageTable;
    public static readonly KnownObjects PosCardholderInteractionInformation;
    public static readonly KnownObjects PositionOfCardVerificationCode3Track1;
    public static readonly KnownObjects PositionOfCardVerificationCode3Track2;
    public static readonly KnownObjects PunatcTrack1;
    public static readonly KnownObjects PunatcTrack2;
    public static readonly KnownObjects PostGenApplicationCryptogramPutDataStatus;
    public static readonly KnownObjects PreGenApplicationCryptogramPutDataStatus;
    public static readonly KnownObjects ProceedToFirstWriteFlag;
    public static readonly KnownObjects ProcessingDataObjectsList;
    public static readonly KnownObjects ProcessingDataObjectsListRelatedData;
    public static readonly KnownObjects ProtectedDataEnvelope1;
    public static readonly KnownObjects ProtectedDataEnvelope2;
    public static readonly KnownObjects ProtectedDataEnvelope3;
    public static readonly KnownObjects ProtectedDataEnvelope4;
    public static readonly KnownObjects ProtectedDataEnvelope5;
    public static readonly KnownObjects ReaderContactlessFloorLimit;
    public static readonly KnownObjects ReaderContactlessTransactionLimitNoOnDeviceCvm;
    public static readonly KnownObjects ReaderContactlessTransactionLimitOnDeviceCvm;
    public static readonly KnownObjects ReaderCvmRequiredLimit;
    public static readonly KnownObjects ReadRecordResponseMessageTemplate;
    public static readonly KnownObjects ReferenceControlParameter;
    public static readonly KnownObjects RelayResistanceAccuracyThreshold;
    public static readonly KnownObjects RelayResistanceProtocolCounter;
    public static readonly KnownObjects RelayResistanceTransmissionTimeMismatchThreshold;
    public static readonly KnownObjects ResponseMessageTemplateFormat1;
    public static readonly KnownObjects ResponseMessageTemplateFormat2;
    public static readonly KnownObjects SecurityCapability;
    public static readonly KnownObjects ServiceCode;
    public static readonly KnownObjects SignedDynamicApplicationData;
    public static readonly KnownObjects StaticDataAuthenticationTagList;
    public static readonly KnownObjects TagsToRead;
    public static readonly KnownObjects TagsToWriteAfterGenApplicationCryptogram;
    public static readonly KnownObjects TagsToWriteBeforeGenApplicationCryptogram;
    public static readonly KnownObjects TerminalActionCodeDefault;
    public static readonly KnownObjects TerminalActionCodeDenial;
    public static readonly KnownObjects TerminalActionCodeOnline;
    public static readonly KnownObjects TerminalCapabilities;
    public static readonly KnownObjects TerminalCountryCode;
    public static readonly KnownObjects TerminalExpectedTransmissionTimeForRelayResistanceCApdu;
    public static readonly KnownObjects TerminalExpectedTransmissionTimeForRelayResistanceRApdu;
    public static readonly KnownObjects TerminalIdentification;
    public static readonly KnownObjects TerminalRelayResistanceEntropy;
    public static readonly KnownObjects TerminalRiskManagementData;
    public static readonly KnownObjects TerminalType;
    public static readonly KnownObjects TerminalVerificationResults;
    public static readonly KnownObjects ThirdPartyData;
    public static readonly KnownObjects TimeOutValue;
    public static readonly KnownObjects TornRecord;
    public static readonly KnownObjects Track1Data;
    public static readonly KnownObjects Track1DiscretionaryData;
    public static readonly KnownObjects Track2Data;
    public static readonly KnownObjects Track2DiscretionaryData;
    public static readonly KnownObjects Track2EquivalentData;
    public static readonly KnownObjects TransactionCategoryCode;
    public static readonly KnownObjects TransactionCurrencyCode;
    public static readonly KnownObjects TransactionCurrencyExponent;
    public static readonly KnownObjects TransactionDate;
    public static readonly KnownObjects TransactionTime;
    public static readonly KnownObjects TransactionType;
    public static readonly KnownObjects UnpredictableNumber;
    public static readonly KnownObjects UnpredictableNumberDataObjectsList;
    public static readonly KnownObjects UnprotectedDataEnvelope1;
    public static readonly KnownObjects UnprotectedDataEnvelope2;
    public static readonly KnownObjects UnprotectedDataEnvelope3;
    public static readonly KnownObjects UnprotectedDataEnvelope4;
    public static readonly KnownObjects UnprotectedDataEnvelope5;
    public static readonly KnownObjects UserInterfaceRequestData;

    #endregion

    #region Instance Values

    public int Count => _ValueObjectMap.Count;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static KnownObjects()
    {
        const uint accountType = 0x5F57;
        const uint acquirerIdentifier = 0x9F01;
        const uint additionalTerminalCapabilities = 0x9F40;
        const uint amountAuthorizedNumeric = 0x9F02;
        const uint amountOther = 0x9F03;
        const uint applicationCapabilitiesInformation = 0x9F5D;
        const uint applicationCryptogram = 0x9F26;
        const uint applicationCurrencyCode = 0x9F42;
        const uint applicationCurrencyExponent = 0x9F44;
        const uint applicationEffectiveDate = 0x5F25;
        const uint applicationExpirationDate = 0x5F24;
        const uint applicationFileLocator = 0x94;
        const uint applicationInterchangeProfile = 0x82;
        const uint applicationLabel = 0x50;
        const uint applicationPAN = 0x5A;
        const uint applicationPANSequenceNumber = 0x5F34;
        const uint applicationPreferredName = 0x9F12;
        const uint applicationPriorityIndicator = 0x87;
        const uint applicationTransactionCounter = 0x9F36;
        const uint applicationUsageControl = 0x9F07;
        const uint applicationVersionNumberCard = 0x9F08;
        const uint applicationVersionNumberReader = 0x9F09;
        const uint balanceReadAfterGenAC = 0xDF8105;
        const uint cardRiskManagementDataObjectList1 = 0x8C;
        const uint cardRiskManagementDataObjectList1RelatedData = 0xDF8107;
        const uint cryptogramInformationData = 0x9F27;
        const uint cvc3Track1 = 0x9F60;
        const uint cvc3Track2 = 0x9F61;
        const uint cvmCapabilityCvmRequired = 0xDF8118;
        const uint cvmCapabilityNoCvmRequired = 0xDF8119;
        const uint cvmList = 0x8E;
        const uint cvmResults = 0x9F34;
        const uint dataNeeded = 0xDF8106;
        const uint dataRecord = 0xFF8105;
        const uint dataRecoveryDataObjectList = 0x9F51;
        const uint dataRecoveryDataObjectListRelatedData = 0xDF8113;
        const uint dataStorageApplicationCryptogramType = 0xDF8108;
        const uint dataStorageAvailability = 0x9F5F;
        const uint dataStorageDataObjectList = 0x9F5B;
        const uint dataStorageDigestH = 0xDF61;
        const uint dataStorageId = 0x9F5E;
        const uint dataStorageInputCard = 0xDF60;
        const uint dataStorageInputTerm = 0xDF8109;
        const uint dataStorageOperatorDataSetCard = 0x9F54;
        const uint dataStorageOperatorDataSetInfo = 0xDF62;
        const uint dataStorageOperatorDataSetInfoForReader = 0xDF810A;
        const uint dataStorageOperatorDataSetTerm = 0xDF63;
        const uint dataStorageRequestedOperatorId = 0x9F5C;
        const uint dataStorageSlotManagementControl = 0x9F6F;
        const uint dataStorageSummary1 = 0x9F7D;
        const uint dataStorageSummary2 = 0xDF8101;
        const uint dataStorageSummary3 = 0xDF8102;
        const uint dataStorageSummaryStatus = 0xDF810B;
        const uint dataStorageUnpredictableNumber = 0x9F7F;
        const uint dataStorageVnTerm = 0xDF810D;
        const uint dataToSend = 0xFF8104;
        const uint ddCardTrack1 = 0xDF812A;
        const uint ddCardTrack2 = 0xDF812B;
        const uint dedicatedFileName = 0x84;
        const uint defaultUdol = 0xDF811A;
        const uint deviceEstimatedTransmissionTimeForRelayResistanceRAPDU = 0xDF8305;
        const uint deviceRelayResistanceEntropy = 0xDF8302;
        const uint discretionaryData = 0xFF8106;
        const uint errorIndication = 0xDF8115;
        const uint fileControlInformationIssuerDiscretionaryData = 0xBF0C;
        const uint fileControlInformationProprietaryTemplate = 0xA5;
        const uint fileControlInformationTemplate = 0x6F;
        const uint holdTimeValue = 0xDF8130;
        const uint iccDynamicNumber = 0x9F4C;
        const uint iccPublicKeyCertificate = 0x9F46;
        const uint iccPublicKeyExponent = 0x9F47;
        const uint iccPublicKeyRemainder = 0x9F48;
        const uint integratedDataStorageStatus = 0xDF8128;
        const uint interfaceDeviceSerialNumber = 0x9F1E;
        const uint issuerActionCodeDefault = 0x9F0D;
        const uint issuerActionCodeDenial = 0x9F0E;
        const uint issuerActionCodeOnline = 0x9F0F;
        const uint issuerApplicationData = 0x9F10;
        const uint issuerCodeTableIndex = 0x9F11;
        const uint issuerCountryCode = 0x5F28;
        const uint issuerPublicKeyCertificate = 0x90;
        const uint issuerPublicKeyExponent = 0x9F32;
        const uint issuerPublicKeyRemainder = 0x92;
        const uint kernelConfiguration = 0xDF811B;
        const uint kernelID = 0xDF810C;
        const uint languagePreference = 0x5F2D;
        const uint logEntry = 0x9F4D;
        const uint magstripeApplicationVersionNumberReader = 0x9F6D;
        const uint magstripeCvmCapabilityCVMRequired = 0xDF811E;
        const uint magstripeCvmCapabilityNoCVMRequired = 0xDF812C;
        const uint maximumRelayResistanceGracePeriod = 0xDF8133;
        const uint maxLifetimeofTornTransactionLogRecord = 0xDF811C;
        const uint maxNumberofTornTransactionLogRecords = 0xDF811D;
        const uint maxTimeForProcessingRelayResistanceAPDU = 0xDF8304;
        const uint measuredRelayResistanceProcessingTime = 0xDF8306;
        const uint merchantCategoryCode = 0x9F15;
        const uint merchantCustomData = 0x9F7C;
        const uint merchantIdentifier = 0x9F16;
        const uint merchantNameandLocation = 0x9F4E;
        const uint messageHoldTime = 0xDF812D;
        const uint minimumRelayResistanceGracePeriod = 0xDF8132;
        const uint minTimeForProcessingRelayResistanceAPDU = 0xDF8303;
        const uint mobileSupportIndicator = 0x9F7E;
        const uint nApplicationTransactionCounterTrack1 = 0x9F64;
        const uint nApplicationTransactionCounterTrack2 = 0x9F67;
        const uint offlineAccumulatorBalance = 0x9F50;
        const uint outcomeParameterSet = 0xDF8129;
        const uint paymentAccountReference = 0x9F24;
        const uint phoneMessageTable = 0xDF8131;
        const uint posCardholderInteractionInformation = 0xDF4B;
        const uint positionOfCardVerificationCode3Track1 = 0x9F62;
        const uint positionOfCardVerificationCode3Track2 = 0x9F65;
        const uint punatcTrack1 = 0x9F63;
        const uint punatcTrack2 = 0x9F66;
        const uint postGenApplicationCryptogramPutDataStatus = 0xDF810E;
        const uint preGenApplicationCryptogramPutDataStatus = 0xDF810F;
        const uint proceedToFirstWriteFlag = 0xDF8110;
        const uint processingDataObjectList = 0x9F38;
        const uint processingDataObjectListRelatedData = 0xDF8111;
        const uint protectedDataEnvelope1 = 0x9F70;
        const uint protectedDataEnvelope2 = 0x9F71;
        const uint protectedDataEnvelope3 = 0x9F72;
        const uint protectedDataEnvelope4 = 0x9F73;
        const uint protectedDataEnvelope5 = 0x9F74;
        const uint readerContactlessFloorLimit = 0xDF8123;
        const uint readerContactlessTransactionLimitNoOnDeviceCvm = 0xDF8124;
        const uint readerContactlessTransactionLimitOnDeviceCvm = 0xDF8125;
        const uint readerCvmRequiredLimit = 0xDF8126;
        const uint readRecordResponseMessageTemplate = 0x70;
        const uint referenceControlParameter = 0xDF8114;
        const uint relayResistanceAccuracyThreshold = 0xDF8136;
        const uint relayResistanceProtocolCounter = 0xDF8307;
        const uint relayResistanceTransmissionTimeMismatchThreshold = 0xDF8137;
        const uint responseMessageTemplateFormat1 = 0x80;
        const uint responseMessageTemplateFormat2 = 0x77;
        const uint securityCapability = 0xDF811F;
        const uint serviceCode = 0x5F30;
        const uint signedDynamicApplicationData = 0x9F4B;
        const uint staticDataAuthenticationTagList = 0x9F4A;
        const uint tagsToRead = 0xDF8112;
        const uint tagsToWriteAfterGenApplicationCryptogram = 0xFF8103;
        const uint tagsToWriteBeforeGenApplicationCryptogram = 0xFF8102;
        const uint terminalActionCodeDefault = 0xDF8120;
        const uint terminalActionCodeDenial = 0xDF8121;
        const uint terminalActionCodeOnline = 0xDF8122;
        const uint terminalCapabilities = 0x9F33;
        const uint terminalCountryCode = 0x9F1A;
        const uint terminalExpectedTransmissionTimeForRelayResistanceCAPDU = 0xDF8134;
        const uint terminalExpectedTransmissionTimeForRelayResistanceRAPDU = 0xDF8135;
        const uint terminalIdentification = 0x9F1C;
        const uint terminalRelayResistanceEntropy = 0xDF8301;
        const uint terminalRiskManagementData = 0x9F1D;
        const uint terminalVerificationResults = 0x95;
        const uint thirdPartyData = 0x9F6E;
        const uint timeOutValue = 0xDF8127;
        const uint tornRecord = 0xFF8101;
        const uint track1Data = 0x56;
        const uint track1DiscretionaryData = 0x9F1F;
        const uint track2Data = 0x9F6B;
        const uint track2DiscretionaryData = 0x9F20;
        const uint track2EquivalentData = 0x57;
        const uint transactionCategoryCode = 0x9F53;
        const uint transactionCurrencyCode = 0x5F2A;
        const uint transactionCurrencyExponent = 0x5F36;
        const uint transactionDate = 0x9A;
        const uint transactionTime = 0x9F21;
        const uint transactionType = 0x9C;
        const uint unpredictableNumber = 0x9F37;
        const uint unpredictableNumberDataObjectList = 0x9F69;
        const uint unprotectedDataEnvelope1 = 0x9F75;
        const uint unprotectedDataEnvelope2 = 0x9F76;
        const uint unprotectedDataEnvelope3 = 0x9F77;
        const uint unprotectedDataEnvelope4 = 0x9F78;
        const uint unprotectedDataEnvelope5 = 0x9F79;
        const uint userInterfaceRequestData = 0xDF8116;
        const uint cardDataInputCapability = 0xDF8117;

        AccountType = new KnownObjects(0x5F57);
        AcquirerIdentifier = new KnownObjects(0x9F01);
        AdditionalTerminalCapabilities = new KnownObjects(0x9F40);
        AmountAuthorizedNumeric = new KnownObjects(0x9F02);
        AmountOther = new KnownObjects(0x9F03);
        ApplicationCapabilitiesInformation = new KnownObjects(0x9F5D);
        ApplicationCryptogram = new KnownObjects(0x9F26);
        ApplicationCurrencyCode = new KnownObjects(0x9F42);
        ApplicationCurrencyExponent = new KnownObjects(0x9F44);
        ApplicationEffectiveDate = new KnownObjects(0x5F25);
        ApplicationExpirationDate = new KnownObjects(0x5F24);
        ApplicationFileLocator = new KnownObjects(0x94);
        ApplicationInterchangeProfile = new KnownObjects(0x82);
        ApplicationLabel = new KnownObjects(0x50);
        ApplicationPAN = new KnownObjects(0x5A);
        ApplicationPANSequenceNumber = new KnownObjects(0x5F34);
        ApplicationPreferredName = new KnownObjects(0x9F12);
        ApplicationPriorityIndicator = new KnownObjects(0x87);
        ApplicationTransactionCounter = new KnownObjects(0x9F36);
        ApplicationUsageControl = new KnownObjects(0x9F07);
        ApplicationVersionNumberCard = new KnownObjects(0x9F08);
        ApplicationVersionNumberReader = new KnownObjects(0x9F09);
        BalanceReadAfterGenAC = new KnownObjects(0xDF8105);
        CardRiskManagementDataObjectsList1 = new KnownObjects(0x8C);
        CardRiskManagementDataObjectsList1RelatedData = new KnownObjects(0xDF8107);
        CryptogramInformationData = new KnownObjects(0x9F27);
        Cvc3Track1 = new KnownObjects(0x9F60);
        Cvc3Track2 = new KnownObjects(0x9F61);
        CvmCapabilityCvmRequired = new KnownObjects(0xDF8118);
        CvmCapabilityNoCvmRequired = new KnownObjects(0xDF8119);
        TerminalType = new KnownObjects(0x9F35);
        CvmList = new KnownObjects(0x8E);
        CvmResults = new KnownObjects(0x9F34);
        DataNeeded = new KnownObjects(0xDF8106);
        DataRecord = new KnownObjects(0xFF8105);
        DataRecoveryDataObjectsList = new KnownObjects(0x9F51);
        DataRecoveryDataObjectsListRelatedData = new KnownObjects(0xDF8113);
        DataStorageApplicationCryptogramType = new KnownObjects(0xDF8108);
        DataStorageAvailability = new KnownObjects(0x9F5F);
        DataStorageDataObjectsList = new KnownObjects(0x9F5B);
        DataStorageDigestH = new KnownObjects(0xDF61);
        DataStorageId = new KnownObjects(0x9F5E);
        DataStorageInputCard = new KnownObjects(0xDF60);
        DataStorageInputTerm = new KnownObjects(0xDF8109);
        DataStorageOperatorDataSetCard = new KnownObjects(0x9F54);
        DataStorageOperatorDataSetInfo = new KnownObjects(0xDF62);
        DataStorageOperatorDataSetInfoForReader = new KnownObjects(0xDF810A);
        DataStorageOperatorDataSetTerm = new KnownObjects(0xDF63);
        DataStorageRequestedOperatorId = new KnownObjects(0x9F5C);
        DataStorageSlotManagementControl = new KnownObjects(0x9F6F);
        DataStorageSummary1 = new KnownObjects(0x9F7D);
        DataStorageSummary2 = new KnownObjects(0xDF8101);
        DataStorageSummary3 = new KnownObjects(0xDF8102);
        DataStorageSummaryStatus = new KnownObjects(0xDF810B);
        DataStorageUnpredictableNumber = new KnownObjects(0x9F7F);
        DataStorageVnTerm = new KnownObjects(0xDF810D);
        DataToSend = new KnownObjects(0xFF8104);
        DdCardTrack1 = new KnownObjects(0xDF812A);
        DdCardTrack2 = new KnownObjects(0xDF812B);
        DedicatedFileName = new KnownObjects(0x84);
        DefaultUdol = new KnownObjects(0xDF811A);
        DeviceEstimatedTransmissionTimeForRelayResistanceRAPDU = new KnownObjects(0xDF8305);
        DeviceRelayResistanceEntropy = new KnownObjects(0xDF8302);
        DiscretionaryData = new KnownObjects(0xFF8106);
        ErrorIndication = new KnownObjects(0xDF8115);
        FileControlInformationIssuerDiscretionaryData = new KnownObjects(0xBF0C);
        FileControlInformationProprietaryTemplate = new KnownObjects(0xA5);
        FileControlInformationTemplate = new KnownObjects(0x6F);
        HoldTimeValue = new KnownObjects(0xDF8130);
        IccDynamicNumber = new KnownObjects(0x9F4C);
        IccPublicKeyCertificate = new KnownObjects(0x9F46);
        IccPublicKeyExponent = new KnownObjects(0x9F47);
        IccPublicKeyRemainder = new KnownObjects(0x9F48);
        IntegratedDataStorageStatus = new KnownObjects(0xDF8128);
        InterfaceDeviceSerialNumber = new KnownObjects(0x9F1E);
        IssuerActionCodeDefault = new KnownObjects(0x9F0D);
        IssuerActionCodeDenial = new KnownObjects(0x9F0E);
        IssuerActionCodeOnline = new KnownObjects(0x9F0F);
        IssuerApplicationData = new KnownObjects(0x9F10);
        IssuerCodeTableIndex = new KnownObjects(0x9F11);
        IssuerCountryCode = new KnownObjects(0x5F28);
        IssuerPublicKeyCertificate = new KnownObjects(0x90);
        IssuerPublicKeyExponent = new KnownObjects(0x9F32);
        IssuerPublicKeyRemainder = new KnownObjects(0x92);
        KernelConfiguration = new KnownObjects(0xDF811B);
        KernelId = new KnownObjects(0xDF810C);
        LanguagePreference = new KnownObjects(0x5F2D);
        LogEntry = new KnownObjects(0x9F4D);
        MagstripeApplicationVersionNumberReader = new KnownObjects(0x9F6D);
        MagstripeCvmCapabilityCvmRequired = new KnownObjects(0xDF811E);
        MagstripeCvmCapabilityNoCvmRequired = new KnownObjects(0xDF812C);
        MaximumRelayResistanceGracePeriod = new KnownObjects(0xDF8133);
        MaxLifetimeOfTornTransactionLogRecord = new KnownObjects(0xDF811C);
        MaxNumberOfTornTransactionLogRecords = new KnownObjects(0xDF811D);
        MaxTimeForProcessingRelayResistanceApdu = new KnownObjects(0xDF8304);
        MeasuredRelayResistanceProcessingTime = new KnownObjects(0xDF8306);
        MerchantCategoryCode = new KnownObjects(0x9F15);
        MerchantCustomData = new KnownObjects(0x9F7C);
        MerchantIdentifier = new KnownObjects(0x9F16);
        MerchantNameandLocation = new KnownObjects(0x9F4E);
        MessageHoldTime = new KnownObjects(0xDF812D);
        MinimumRelayResistanceGracePeriod = new KnownObjects(0xDF8132);
        MinTimeForProcessingRelayResistanceApdu = new KnownObjects(0xDF8303);
        MobileSupportIndicator = new KnownObjects(0x9F7E);
        NApplicationTransactionCounterTrack1 = new KnownObjects(0x9F64);
        NApplicationTransactionCounterTrack2 = new KnownObjects(0x9F67);
        OfflineAccumulatorBalance = new KnownObjects(0x9F50);
        OutcomeParameterSet = new KnownObjects(0xDF8129);
        PaymentAccountReference = new KnownObjects(0x9F24);
        PhoneMessageTable = new KnownObjects(0xDF8131);
        PosCardholderInteractionInformation = new KnownObjects(0xDF4B);
        PositionOfCardVerificationCode3Track1 = new KnownObjects(0x9F62);
        PositionOfCardVerificationCode3Track2 = new KnownObjects(0x9F65);
        PunatcTrack1 = new KnownObjects(0x9F63);
        PunatcTrack2 = new KnownObjects(0x9F66);
        PostGenApplicationCryptogramPutDataStatus = new KnownObjects(0xDF810E);
        PreGenApplicationCryptogramPutDataStatus = new KnownObjects(0xDF810F);
        ProceedToFirstWriteFlag = new KnownObjects(0xDF8110);
        ProcessingDataObjectsList = new KnownObjects(0x9F38);
        ProcessingDataObjectsListRelatedData = new KnownObjects(0xDF8111);
        ProtectedDataEnvelope1 = new KnownObjects(0x9F70);
        ProtectedDataEnvelope2 = new KnownObjects(0x9F71);
        ProtectedDataEnvelope3 = new KnownObjects(0x9F72);
        ProtectedDataEnvelope4 = new KnownObjects(0x9F73);
        ProtectedDataEnvelope5 = new KnownObjects(0x9F74);
        ReaderContactlessFloorLimit = new KnownObjects(0xDF8123);
        ReaderContactlessTransactionLimitNoOnDeviceCvm = new KnownObjects(0xDF8124);
        ReaderContactlessTransactionLimitOnDeviceCvm = new KnownObjects(0xDF8125);
        ReaderCvmRequiredLimit = new KnownObjects(0xDF8126);
        ReadRecordResponseMessageTemplate = new KnownObjects(0x70);
        ReferenceControlParameter = new KnownObjects(0xDF8114);
        RelayResistanceAccuracyThreshold = new KnownObjects(0xDF8136);
        RelayResistanceProtocolCounter = new KnownObjects(0xDF8307);
        RelayResistanceTransmissionTimeMismatchThreshold = new KnownObjects(0xDF8137);
        ResponseMessageTemplateFormat1 = new KnownObjects(0x80);
        ResponseMessageTemplateFormat2 = new KnownObjects(0x77);
        SecurityCapability = new KnownObjects(0xDF811F);
        ServiceCode = new KnownObjects(0x5F30);
        SignedDynamicApplicationData = new KnownObjects(0x9F4B);
        StaticDataAuthenticationTagList = new KnownObjects(0x9F4A);
        TagsToRead = new KnownObjects(0xDF8112);
        TagsToWriteAfterGenApplicationCryptogram = new KnownObjects(0xFF8103);
        TagsToWriteBeforeGenApplicationCryptogram = new KnownObjects(0xFF8102);
        TerminalActionCodeDefault = new KnownObjects(0xDF8120);
        TerminalActionCodeDenial = new KnownObjects(0xDF8121);
        TerminalActionCodeOnline = new KnownObjects(0xDF8122);
        TerminalCapabilities = new KnownObjects(0x9F33);
        TerminalCountryCode = new KnownObjects(0x9F1A);
        TerminalExpectedTransmissionTimeForRelayResistanceCApdu = new KnownObjects(0xDF8134);
        TerminalExpectedTransmissionTimeForRelayResistanceRApdu = new KnownObjects(0xDF8135);
        TerminalIdentification = new KnownObjects(0x9F1C);
        TerminalRelayResistanceEntropy = new KnownObjects(0xDF8301);
        TerminalRiskManagementData = new KnownObjects(0x9F1D);
        TerminalVerificationResults = new KnownObjects(0x95);
        ThirdPartyData = new KnownObjects(0x9F6E);
        TimeOutValue = new KnownObjects(0xDF8127);
        TornRecord = new KnownObjects(0xFF8101);
        Track1Data = new KnownObjects(0x56);
        Track1DiscretionaryData = new KnownObjects(0x9F1F);
        Track2Data = new KnownObjects(0x9F6B);
        Track2DiscretionaryData = new KnownObjects(0x9F20);
        Track2EquivalentData = new KnownObjects(0x57);
        TransactionCategoryCode = new KnownObjects(0x9F53);
        TransactionCurrencyCode = new KnownObjects(0x5F2A);
        TransactionCurrencyExponent = new KnownObjects(0x5F36);
        TransactionDate = new KnownObjects(0x9A);
        TransactionTime = new KnownObjects(0x9F21);
        TransactionType = new KnownObjects(0x9C);
        UnpredictableNumber = new KnownObjects(0x9F37);
        UnpredictableNumber = new KnownObjects(0x9F6A);
        UnpredictableNumberDataObjectsList = new KnownObjects(0x9F69);
        UnprotectedDataEnvelope1 = new KnownObjects(0x9F75);
        UnprotectedDataEnvelope2 = new KnownObjects(0x9F76);
        UnprotectedDataEnvelope3 = new KnownObjects(0x9F77);
        UnprotectedDataEnvelope4 = new KnownObjects(0x9F78);
        UnprotectedDataEnvelope5 = new KnownObjects(0x9F79);
        UserInterfaceRequestData = new KnownObjects(0xDF8116);
        CardDataInputCapability = new KnownObjects(0xDF8117);

        _ValueObjectMap = new Dictionary<Tag, KnownObjects>
        {
            {accountType, AccountType},
            {acquirerIdentifier, AcquirerIdentifier},
            {additionalTerminalCapabilities, AdditionalTerminalCapabilities},
            {amountAuthorizedNumeric, AmountAuthorizedNumeric},
            {amountOther, AmountOther},
            {applicationCapabilitiesInformation, ApplicationCapabilitiesInformation},
            {applicationCryptogram, ApplicationCryptogram},
            {applicationCurrencyCode, ApplicationCurrencyCode},
            {applicationCurrencyExponent, ApplicationCurrencyExponent},
            {applicationEffectiveDate, ApplicationEffectiveDate},
            {applicationExpirationDate, ApplicationExpirationDate},
            {applicationFileLocator, ApplicationFileLocator},
            {applicationInterchangeProfile, ApplicationInterchangeProfile},
            {applicationLabel, ApplicationLabel},
            {applicationPAN, ApplicationPAN},
            {applicationPANSequenceNumber, ApplicationPANSequenceNumber},
            {applicationPreferredName, ApplicationPreferredName},
            {applicationPriorityIndicator, ApplicationPriorityIndicator},
            {applicationTransactionCounter, ApplicationTransactionCounter},
            {applicationUsageControl, ApplicationUsageControl},
            {applicationVersionNumberCard, ApplicationVersionNumberCard},
            {applicationVersionNumberReader, ApplicationVersionNumberReader},
            {balanceReadAfterGenAC, BalanceReadAfterGenAC},
            {cardRiskManagementDataObjectList1, CardRiskManagementDataObjectsList1},
            {cardRiskManagementDataObjectList1RelatedData, CardRiskManagementDataObjectsList1RelatedData},
            {cryptogramInformationData, CryptogramInformationData},
            {cvc3Track1, Cvc3Track1},
            {cvc3Track2, Cvc3Track2},
            {cvmCapabilityCvmRequired, CvmCapabilityCvmRequired},
            {cvmCapabilityNoCvmRequired, CvmCapabilityNoCvmRequired},
            {cvmList, CvmList},
            {cvmResults, CvmResults},
            {dataNeeded, DataNeeded},
            {dataRecord, DataRecord},
            {dataRecoveryDataObjectList, DataRecoveryDataObjectsList},
            {dataRecoveryDataObjectListRelatedData, DataRecoveryDataObjectsListRelatedData},
            {dataStorageApplicationCryptogramType, DataStorageApplicationCryptogramType},
            {dataStorageAvailability, DataStorageAvailability},
            {dataStorageDataObjectList, DataStorageDataObjectsList},
            {dataStorageDigestH, DataStorageDigestH},
            {dataStorageId, DataStorageId},
            {dataStorageInputCard, DataStorageInputCard},
            {dataStorageInputTerm, DataStorageInputTerm},
            {dataStorageOperatorDataSetCard, DataStorageOperatorDataSetCard},
            {dataStorageOperatorDataSetInfo, DataStorageOperatorDataSetInfo},
            {dataStorageOperatorDataSetInfoForReader, DataStorageOperatorDataSetInfoForReader},
            {dataStorageOperatorDataSetTerm, DataStorageOperatorDataSetTerm},
            {dataStorageRequestedOperatorId, DataStorageRequestedOperatorId},
            {dataStorageSlotManagementControl, DataStorageSlotManagementControl},
            {dataStorageSummary1, DataStorageSummary1},
            {dataStorageSummary2, DataStorageSummary2},
            {dataStorageSummary3, DataStorageSummary3},
            {dataStorageSummaryStatus, DataStorageSummaryStatus},
            {dataStorageUnpredictableNumber, DataStorageUnpredictableNumber},
            {dataStorageVnTerm, DataStorageVnTerm},
            {dataToSend, DataToSend},
            {ddCardTrack1, DdCardTrack1},
            {ddCardTrack2, DdCardTrack2},
            {dedicatedFileName, DedicatedFileName},
            {defaultUdol, DefaultUdol},
            {deviceEstimatedTransmissionTimeForRelayResistanceRAPDU, DeviceEstimatedTransmissionTimeForRelayResistanceRAPDU},
            {deviceRelayResistanceEntropy, DeviceRelayResistanceEntropy},
            {discretionaryData, DiscretionaryData},
            {errorIndication, ErrorIndication},
            {fileControlInformationIssuerDiscretionaryData, FileControlInformationIssuerDiscretionaryData},
            {fileControlInformationProprietaryTemplate, FileControlInformationProprietaryTemplate},
            {fileControlInformationTemplate, FileControlInformationTemplate},
            {holdTimeValue, HoldTimeValue},
            {iccDynamicNumber, IccDynamicNumber},
            {iccPublicKeyCertificate, IccPublicKeyCertificate},
            {iccPublicKeyExponent, IccPublicKeyExponent},
            {iccPublicKeyRemainder, IccPublicKeyRemainder},
            {integratedDataStorageStatus, IntegratedDataStorageStatus},
            {interfaceDeviceSerialNumber, InterfaceDeviceSerialNumber},
            {issuerActionCodeDefault, IssuerActionCodeDefault},
            {issuerActionCodeDenial, IssuerActionCodeDenial},
            {issuerActionCodeOnline, IssuerActionCodeOnline},
            {issuerApplicationData, IssuerApplicationData},
            {issuerCodeTableIndex, IssuerCodeTableIndex},
            {issuerCountryCode, IssuerCountryCode},
            {issuerPublicKeyCertificate, IssuerPublicKeyCertificate},
            {issuerPublicKeyExponent, IssuerPublicKeyExponent},
            {issuerPublicKeyRemainder, IssuerPublicKeyRemainder},
            {kernelConfiguration, KernelConfiguration},
            {kernelID, KernelId},
            {languagePreference, LanguagePreference},
            {logEntry, LogEntry},
            {magstripeApplicationVersionNumberReader, MagstripeApplicationVersionNumberReader},
            {magstripeCvmCapabilityCVMRequired, MagstripeCvmCapabilityCvmRequired},
            {magstripeCvmCapabilityNoCVMRequired, MagstripeCvmCapabilityNoCvmRequired},
            {maximumRelayResistanceGracePeriod, MaximumRelayResistanceGracePeriod},
            {maxLifetimeofTornTransactionLogRecord, MaxLifetimeOfTornTransactionLogRecord},
            {maxNumberofTornTransactionLogRecords, MaxNumberOfTornTransactionLogRecords},
            {maxTimeForProcessingRelayResistanceAPDU, MaxTimeForProcessingRelayResistanceApdu},
            {measuredRelayResistanceProcessingTime, MeasuredRelayResistanceProcessingTime},
            {merchantCategoryCode, MerchantCategoryCode},
            {merchantCustomData, MerchantCustomData},
            {merchantIdentifier, MerchantIdentifier},
            {merchantNameandLocation, MerchantNameandLocation},
            {messageHoldTime, MessageHoldTime},
            {minimumRelayResistanceGracePeriod, MinimumRelayResistanceGracePeriod},
            {minTimeForProcessingRelayResistanceAPDU, MinTimeForProcessingRelayResistanceApdu},
            {mobileSupportIndicator, MobileSupportIndicator},
            {nApplicationTransactionCounterTrack1, NApplicationTransactionCounterTrack1},
            {nApplicationTransactionCounterTrack2, NApplicationTransactionCounterTrack2},
            {offlineAccumulatorBalance, OfflineAccumulatorBalance},
            {outcomeParameterSet, OutcomeParameterSet},
            {paymentAccountReference, PaymentAccountReference},
            {phoneMessageTable, PhoneMessageTable},
            {posCardholderInteractionInformation, PosCardholderInteractionInformation},
            {positionOfCardVerificationCode3Track1, PositionOfCardVerificationCode3Track1},
            {positionOfCardVerificationCode3Track2, PositionOfCardVerificationCode3Track2},
            {punatcTrack1, PunatcTrack1},
            {punatcTrack2, PunatcTrack2},
            {postGenApplicationCryptogramPutDataStatus, PostGenApplicationCryptogramPutDataStatus},
            {preGenApplicationCryptogramPutDataStatus, PreGenApplicationCryptogramPutDataStatus},
            {proceedToFirstWriteFlag, ProceedToFirstWriteFlag},
            {processingDataObjectList, ProcessingDataObjectsList},
            {processingDataObjectListRelatedData, ProcessingDataObjectsListRelatedData},
            {protectedDataEnvelope1, ProtectedDataEnvelope1},
            {protectedDataEnvelope2, ProtectedDataEnvelope2},
            {protectedDataEnvelope3, ProtectedDataEnvelope3},
            {protectedDataEnvelope4, ProtectedDataEnvelope4},
            {protectedDataEnvelope5, ProtectedDataEnvelope5},
            {readerContactlessFloorLimit, ReaderContactlessFloorLimit},
            {readerContactlessTransactionLimitNoOnDeviceCvm, ReaderContactlessTransactionLimitNoOnDeviceCvm},
            {readerContactlessTransactionLimitOnDeviceCvm, ReaderContactlessTransactionLimitOnDeviceCvm},
            {readerCvmRequiredLimit, ReaderCvmRequiredLimit},
            {readRecordResponseMessageTemplate, ReadRecordResponseMessageTemplate},
            {referenceControlParameter, ReferenceControlParameter},
            {relayResistanceAccuracyThreshold, RelayResistanceAccuracyThreshold},
            {relayResistanceProtocolCounter, RelayResistanceProtocolCounter},
            {relayResistanceTransmissionTimeMismatchThreshold, RelayResistanceTransmissionTimeMismatchThreshold},
            {responseMessageTemplateFormat1, ResponseMessageTemplateFormat1},
            {responseMessageTemplateFormat2, ResponseMessageTemplateFormat2},
            {securityCapability, SecurityCapability},
            {serviceCode, ServiceCode},
            {signedDynamicApplicationData, SignedDynamicApplicationData},
            {staticDataAuthenticationTagList, StaticDataAuthenticationTagList},
            {tagsToRead, TagsToRead},
            {tagsToWriteAfterGenApplicationCryptogram, TagsToWriteAfterGenApplicationCryptogram},
            {tagsToWriteBeforeGenApplicationCryptogram, TagsToWriteBeforeGenApplicationCryptogram},
            {terminalActionCodeDefault, TerminalActionCodeDefault},
            {terminalActionCodeDenial, TerminalActionCodeDenial},
            {terminalActionCodeOnline, TerminalActionCodeOnline},
            {terminalCapabilities, TerminalCapabilities},
            {terminalCountryCode, TerminalCountryCode},
            {terminalExpectedTransmissionTimeForRelayResistanceCAPDU, TerminalExpectedTransmissionTimeForRelayResistanceCApdu},
            {terminalExpectedTransmissionTimeForRelayResistanceRAPDU, TerminalExpectedTransmissionTimeForRelayResistanceRApdu},
            {terminalIdentification, TerminalIdentification},
            {terminalRelayResistanceEntropy, TerminalRelayResistanceEntropy},
            {terminalRiskManagementData, TerminalRiskManagementData},
            {terminalVerificationResults, TerminalVerificationResults},
            {thirdPartyData, ThirdPartyData},
            {timeOutValue, TimeOutValue},
            {tornRecord, TornRecord},
            {track1Data, Track1Data},
            {track1DiscretionaryData, Track1DiscretionaryData},
            {track2Data, Track2Data},
            {track2DiscretionaryData, Track2DiscretionaryData},
            {track2EquivalentData, Track2EquivalentData},
            {transactionCategoryCode, TransactionCategoryCode},
            {transactionCurrencyCode, TransactionCurrencyCode},
            {transactionCurrencyExponent, TransactionCurrencyExponent},
            {transactionDate, TransactionDate},
            {transactionTime, TransactionTime},
            {transactionType, TransactionType},
            {unpredictableNumber, UnpredictableNumber},
            {unpredictableNumber, UnpredictableNumber},
            {unpredictableNumberDataObjectList, UnpredictableNumberDataObjectsList},
            {unprotectedDataEnvelope1, UnprotectedDataEnvelope1},
            {unprotectedDataEnvelope2, UnprotectedDataEnvelope2},
            {unprotectedDataEnvelope3, UnprotectedDataEnvelope3},
            {unprotectedDataEnvelope4, UnprotectedDataEnvelope4},
            {unprotectedDataEnvelope5, UnprotectedDataEnvelope5},
            {userInterfaceRequestData, UserInterfaceRequestData},
            {cardDataInputCapability, CardDataInputCapability}
        }.ToImmutableSortedDictionary();
    }

    private KnownObjects(Tag value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(KnownObjects other) => _Value._Value.CompareTo(other._Value);
    public static bool Exists(Tag value) => _ValueObjectMap.ContainsKey(value);

    public static IEnumerator<Tag> GetEnumerator()
    {
        return _ValueObjectMap.Values.Select(a => (Tag) a).GetEnumerator();
    }

    public static bool TryGet(Tag value, out KnownObjects result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(KnownObjects? other) => !(other is null) && (_Value == other._Value);

    public bool Equals(KnownObjects x, KnownObjects y)
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

    public int GetHashCode(KnownObjects obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(KnownObjects registeredApplicationProviderIndicators) =>
        registeredApplicationProviderIndicators._Value;

    public static explicit operator KnownObjects(Tag registeredApplicationProviderIndicator)
    {
        if (!TryGet(registeredApplicationProviderIndicator, out KnownObjects result))
        {
            throw new ArgumentOutOfRangeException(nameof(registeredApplicationProviderIndicator),
                $"The {nameof(KnownObjects)} could not be found from the number supplied to the argument: {registeredApplicationProviderIndicator}");
        }

        return result;
    }

    #endregion
}