using Play.Codecs;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Configuration;
using Play.Emv.Terminal.Session;

namespace MockPos.Dtos;

public class TerminalConfigurationDto
{
    #region Instance Values

    public string? TerminalIdentification { get; set; }
    public string? MerchantIdentifier { get; set; }
    public string? MerchantCategoryCode { get; set; }
    public string? MerchantNameAndLocation { get; set; }
    public string? AcquirerIdentifier { get; set; }
    public string? InterfaceDeviceSerialNumber { get; set; }
    public string? TerminalType { get; set; }
    public string? TerminalCapabilities { get; set; }
    public string? AdditionalTerminalCapabilities { get; set; }
    public string? TerminalCountryCode { get; set; }
    public string? LanguagePreference { get; set; }
    public string? TransactionCurrencyCode { get; set; }
    public string? TransactionCurrencyExponent { get; set; }
    public string? TransactionReferenceCurrencyCode { get; set; }
    public string? TransactionReferenceCurrencyExponent { get; set; }
    public string? TerminalFloorLimit { get; set; }
    public string? TerminalRiskManagementData { get; set; }
    public string? DataStorageRequestedOperatorId { get; set; }
    public string? BiasedRandomSelectionProbability { get; set; }
    public string? BiasedRandomSelectionTargetProbability { get; set; }
    public string? ThresholdValueForBiasedRandomSelection { get; set; }
    public string? MaxNumberOfTornTransactionLogRecords { get; set; }
    public string? MaxLifetimeOfTornTransactionLogRecords { get; set; }
    public SequenceConfigurationDto? SequenceConfiguration { get; set; }

    #endregion

    #region Serialization

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.CodecParsingException"></exception>
    public TerminalConfiguration Decode()
    {
        TerminalIdentification terminalIdentification =
            Play.Emv.Ber.DataElements.TerminalIdentification.Decode(PlayCodec.HexadecimalCodec.Encode(TerminalIdentification).AsSpan());
        MerchantIdentifier merchantIdentifier =
            Play.Emv.Ber.DataElements.MerchantIdentifier.Decode(PlayCodec.HexadecimalCodec.Encode(MerchantIdentifier).AsSpan());
        MerchantCategoryCode merchantCategoryCode =
            Play.Emv.Ber.DataElements.MerchantCategoryCode.Decode(PlayCodec.HexadecimalCodec.Encode(MerchantCategoryCode).AsSpan());
        MerchantNameAndLocation merchantNameAndLocation =
            Play.Emv.Ber.DataElements.MerchantNameAndLocation.Decode(PlayCodec.HexadecimalCodec.Encode(MerchantNameAndLocation).AsSpan());
        AcquirerIdentifier acquirerIdentifier =
            Play.Emv.Ber.DataElements.AcquirerIdentifier.Decode(PlayCodec.HexadecimalCodec.Encode(AcquirerIdentifier).AsSpan());
        InterfaceDeviceSerialNumber interfaceDeviceSerialNumber =
            Play.Emv.Ber.DataElements.InterfaceDeviceSerialNumber.Decode(PlayCodec.HexadecimalCodec.Encode(InterfaceDeviceSerialNumber).AsSpan());
        TerminalType terminalType = Play.Emv.Ber.DataElements.TerminalType.Decode(PlayCodec.HexadecimalCodec.Encode(TerminalType).AsSpan());
        TerminalCapabilities terminalCapabilities =
            Play.Emv.Ber.DataElements.TerminalCapabilities.Decode(PlayCodec.HexadecimalCodec.Encode(TerminalCapabilities).AsSpan());
        AdditionalTerminalCapabilities additionalTerminalCapabilities =
            Play.Emv.Ber.DataElements.AdditionalTerminalCapabilities.Decode(PlayCodec.HexadecimalCodec.Encode(AdditionalTerminalCapabilities).AsSpan());
        TerminalCountryCode terminalCountryCode =
            Play.Emv.Ber.DataElements.TerminalCountryCode.Decode(PlayCodec.HexadecimalCodec.Encode(TerminalCountryCode).AsSpan());
        LanguagePreference languagePreference =
            Play.Emv.Ber.DataElements.LanguagePreference.Decode(PlayCodec.HexadecimalCodec.Encode(LanguagePreference).AsSpan());
        TransactionCurrencyCode transactionCurrencyCode =
            Play.Emv.Ber.DataElements.TransactionCurrencyCode.Decode(PlayCodec.HexadecimalCodec.Encode(TransactionCurrencyCode).AsSpan());
        TransactionCurrencyExponent transactionCurrencyExponent =
            Play.Emv.Ber.DataElements.TransactionCurrencyExponent.Decode(PlayCodec.HexadecimalCodec.Encode(TransactionCurrencyExponent).AsSpan());
        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode =
            Play.Emv.Ber.DataElements.TransactionReferenceCurrencyCode.Decode(PlayCodec.HexadecimalCodec.Encode(TransactionReferenceCurrencyCode).AsSpan());
        TransactionReferenceCurrencyExponent transactionReferenceCurrencyExponent =
            Play.Emv.Ber.DataElements.TransactionReferenceCurrencyExponent.Decode(PlayCodec.HexadecimalCodec.Encode(TransactionReferenceCurrencyExponent)
                .AsSpan());
        TerminalRiskManagementData terminalRiskManagementData =
            Play.Emv.Ber.DataElements.TerminalRiskManagementData.Decode(PlayCodec.HexadecimalCodec.Encode(TerminalRiskManagementData).AsSpan());
        TerminalFloorLimit terminalFloorLimit =
            Play.Emv.Ber.DataElements.TerminalFloorLimit.Decode(PlayCodec.HexadecimalCodec.Encode(TerminalFloorLimit).AsSpan());
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId =
            Play.Emv.Ber.DataElements.DataStorageRequestedOperatorId.Decode(PlayCodec.HexadecimalCodec.Encode(DataStorageRequestedOperatorId).AsSpan());

        MaxNumberOfTornTransactionLogRecords maxNumberOfTornTransactionLogRecords =
            Play.Emv.Ber.DataElements.MaxNumberOfTornTransactionLogRecords.Decode(PlayCodec.HexadecimalCodec.Encode(MaxNumberOfTornTransactionLogRecords)
                .AsSpan());
        MaxLifetimeOfTornTransactionLogRecords maxLifetimeOfTornTransactionLogRecords =
            Play.Emv.Ber.DataElements.MaxLifetimeOfTornTransactionLogRecords.Decode(PlayCodec.HexadecimalCodec.Encode(MaxLifetimeOfTornTransactionLogRecords)
                .AsSpan());

        Probability biasedRandomSelectionProbability = new(PlayCodec.HexadecimalCodec.DecodeToByte(BiasedRandomSelectionProbability));
        Probability biasedRandomSelectionTargetProbability = new(PlayCodec.HexadecimalCodec.DecodeToByte(BiasedRandomSelectionTargetProbability));
        ulong thresholdValueForBiasedRandomSelection = PlayCodec.HexadecimalCodec.DecodeToUIn64(ThresholdValueForBiasedRandomSelection);
        PoiInformation poiInformation = new(0);

        SequenceConfiguration sequenceConfiguration = new(SequenceConfiguration?.Threshold ?? 999999, SequenceConfiguration?.InitializationValue ?? 1);

        return new TerminalConfiguration(terminalIdentification, merchantIdentifier, interfaceDeviceSerialNumber, transactionCurrencyCode, terminalCapabilities,
            terminalFloorLimit, terminalType, terminalCountryCode, merchantCategoryCode, languagePreference, merchantNameAndLocation,
            terminalRiskManagementData, biasedRandomSelectionProbability, biasedRandomSelectionTargetProbability, thresholdValueForBiasedRandomSelection,
            poiInformation, additionalTerminalCapabilities, transactionReferenceCurrencyCode, transactionReferenceCurrencyExponent, acquirerIdentifier,
            dataStorageRequestedOperatorId, transactionCurrencyExponent, maxLifetimeOfTornTransactionLogRecords, maxNumberOfTornTransactionLogRecords,
            sequenceConfiguration);
    }

    #endregion
}