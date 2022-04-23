using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: The Data Record is a list of TLV encoded data objects returned with the Outcome Parameter Set on the
///     completion of transaction processing.
/// </summary>
public record DataRecord : DataExchangeResponse, IEqualityComparer<DataRecord>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8105;

    #endregion

    #region Constructor

    public DataRecord(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static DataRecord CreateMagstripeDataRecord(IReadTlvDatabase database) => new(RetrieveMagstripeDataRecordObjects(database).ToArray());

    /// <summary>
    ///     RetrieveMagstripeDataRecordObjects
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public static IEnumerable<PrimitiveValue> RetrieveMagstripeDataRecordObjects(IReadTlvDatabase database)
    {
        if (database.TryGet(ApplicationLabel.Tag, out ApplicationLabel? applicationLabel))
            yield return applicationLabel!;
        if (database.TryGet(ApplicationPreferredName.Tag, out ApplicationPreferredName? applicationPreferredName))
            yield return applicationPreferredName!;
        if (database.TryGet(DedicatedFileName.Tag, out DedicatedFileName? dedicatedFileName))
            yield return dedicatedFileName!;
        if (database.TryGet(IssuerCodeTableIndex.Tag, out IssuerCodeTableIndex? issuerCodeTableIndex))
            yield return issuerCodeTableIndex!;
        if (database.TryGet(MagstripeApplicationVersionNumberReader.Tag, out MagstripeApplicationVersionNumberReader? magstripeApplicationVersionNumberReader))
            yield return magstripeApplicationVersionNumberReader!;
        if (database.TryGet(PaymentAccountReference.Tag, out PaymentAccountReference? paymentAccountReference))
            yield return paymentAccountReference!;
        if (database.TryGet(Track1Data.Tag, out Track1Data? track1Data))
            yield return track1Data!;
        if (database.TryGet(Track2Data.Tag, out Track2Data? track2Data))
            yield return track2Data!;
    }

    /// <exception cref="Exceptions.TerminalDataException"></exception>
    public static DataRecord CreateEmvDataRecord(IReadTlvDatabase database) => new(RetrieveEmvDataRecordObjects(database).ToArray());

    /// <exception cref="Exceptions.TerminalDataException"></exception>
    public static IEnumerable<PrimitiveValue> RetrieveEmvDataRecordObjects(IReadTlvDatabase database)
    {
        if (database.TryGet(AmountAuthorizedNumeric.Tag, out AmountAuthorizedNumeric? amountAuthorizedNumeric))
            yield return amountAuthorizedNumeric!;
        if (database.TryGet(AmountOtherNumeric.Tag, out AmountOtherNumeric? amountOtherNumeric))
            yield return amountOtherNumeric!;
        if (database.TryGet(ApplicationCryptogram.Tag, out ApplicationCryptogram? applicationCryptogram))
            yield return applicationCryptogram!;
        if (database.TryGet(ApplicationExpirationDate.Tag, out ApplicationExpirationDate? applicationExpirationDate))
            yield return applicationExpirationDate!;
        if (database.TryGet(ApplicationInterchangeProfile.Tag, out ApplicationInterchangeProfile? applicationInterchangeProfile))
            yield return applicationInterchangeProfile!;
        if (database.TryGet(ApplicationLabel.Tag, out ApplicationLabel? applicationLabel))
            yield return applicationLabel!;
        if (database.TryGet(ApplicationPan.Tag, out ApplicationPan? applicationPan))
            yield return applicationPan!;
        if (database.TryGet(ApplicationPanSequenceNumber.Tag, out ApplicationPanSequenceNumber? applicationPanSequenceNumber))
            yield return applicationPanSequenceNumber!;
        if (database.TryGet(ApplicationPreferredName.Tag, out ApplicationPreferredName? applicationPreferredName))
            yield return applicationPreferredName!;
        if (database.TryGet(ApplicationTransactionCounter.Tag, out ApplicationTransactionCounter? applicationTransactionCounter))
            yield return applicationTransactionCounter!;
        if (database.TryGet(ApplicationUsageControl.Tag, out ApplicationUsageControl? applicationUsageControl))
            yield return applicationUsageControl!;
        if (database.TryGet(ApplicationVersionNumberReader.Tag, out ApplicationVersionNumberReader? applicationVersionNumberReader))
            yield return applicationVersionNumberReader!;
        if (database.TryGet(CryptogramInformationData.Tag, out CryptogramInformationData? cryptogramInformationData))
            yield return cryptogramInformationData!;
        if (database.TryGet(CvmResults.Tag, out CvmResults? cvmResults))
            yield return cvmResults!;
        if (database.TryGet(DedicatedFileName.Tag, out DedicatedFileName? dedicatedFileName))
            yield return dedicatedFileName!;
        if (database.TryGet(InterfaceDeviceSerialNumber.Tag, out InterfaceDeviceSerialNumber? interfaceDeviceSerialNumber))
            yield return interfaceDeviceSerialNumber!;
        if (database.TryGet(IssuerApplicationData.Tag, out IssuerApplicationData? issuerApplicationData))
            yield return issuerApplicationData!;
        if (database.TryGet(IssuerCodeTableIndex.Tag, out IssuerCodeTableIndex? issuerCodeTableIndex))
            yield return issuerCodeTableIndex!;
        if (database.TryGet(PaymentAccountReference.Tag, out PaymentAccountReference? paymentAccountReference))
            yield return paymentAccountReference!;
        if (database.TryGet(TerminalCapabilities.Tag, out TerminalCapabilities? terminalCapabilities))
            yield return terminalCapabilities!;
        if (database.TryGet(TerminalCountryCode.Tag, out TerminalCountryCode? terminalCountryCode))
            yield return terminalCountryCode!;
        if (database.TryGet(TerminalType.Tag, out TerminalType? terminalType))
            yield return terminalType!;
        if (database.TryGet(TerminalVerificationResults.Tag, out TerminalVerificationResults? terminalVerificationResults))
            yield return terminalVerificationResults!;
        if (database.TryGet(Track2EquivalentData.Tag, out Track2EquivalentData? track2EquivalentData))
            yield return track2EquivalentData!;
        if (database.TryGet(TransactionCategoryCode.Tag, out TransactionCategoryCode? transactionCategoryCode))
            yield return transactionCategoryCode!;
        if (database.TryGet(TransactionCurrencyCode.Tag, out TransactionCurrencyCode? transactionCurrencyCode))
            yield return transactionCurrencyCode!;
        if (database.TryGet(TransactionDate.Tag, out TransactionDate? transactionDate))
            yield return transactionDate!;
        if (database.TryGet(TransactionType.Tag, out TransactionType? transactionType))
            yield return transactionType!;
        if (database.TryGet(UnpredictableNumber.Tag, out UnpredictableNumber? unpredictableNumber))
            yield return unpredictableNumber!;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    public static DataRecord Decode(ReadOnlySpan<byte> value) => new(Codec.DecodePrimitiveValuesAtRuntime(value).ToArray());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataRecord Decode(ReadOnlyMemory<byte> value) => new(Codec.DecodePrimitiveValuesAtRuntime(value.Span).ToArray());

    #endregion

    #region Equality

    public bool Equals(DataRecord? x, DataRecord? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataRecord obj) => obj.GetHashCode();

    #endregion
}