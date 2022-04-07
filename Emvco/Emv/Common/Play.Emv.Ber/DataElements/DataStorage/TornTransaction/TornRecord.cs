using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     A copy of a record from the Torn Transaction Log that is expired.Torn Record is sent to the
///     Terminal as part of the Discretionary Data.
/// </summary>
public record TornRecord : DataExchangeResponse, IEqualityComparer<TornRecord>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8101;

    #endregion

    #region Constructor

    public TornRecord(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="Exceptions.TerminalDataException"></exception>
    public static TornRecord Create(ITlvReaderAndWriter database)
    {
        List<PrimitiveValue> buffer = new();

        if (database.TryGet(AmountAuthorizedNumeric.Tag, out AmountAuthorizedNumeric? amountAuthorizedNumeric))
            buffer.Add(amountAuthorizedNumeric!);
        if (database.TryGet(AmountOtherNumeric.Tag, out AmountOtherNumeric? amountOtherNumeric))
            buffer.Add(amountOtherNumeric!);
        if (database.TryGet(ApplicationPan.Tag, out ApplicationPan? applicationPan))
            buffer.Add(applicationPan!);
        if (database.TryGet(ApplicationPanSequenceNumber.Tag, out ApplicationPanSequenceNumber? applicationPanSequenceNumber))
            buffer.Add(applicationPanSequenceNumber!);
        if (database.TryGet(BalanceReadBeforeGenAc.Tag, out BalanceReadBeforeGenAc? balanceReadBeforeGenAc))
            buffer.Add(balanceReadBeforeGenAc!);
        if (database.TryGet(CardRiskManagementDataObjectList1RelatedData.Tag,
                            out CardRiskManagementDataObjectList1RelatedData? cardRiskManagementDataObjectList1RelatedData))
            buffer.Add(cardRiskManagementDataObjectList1RelatedData!);
        if (database.TryGet(CvmResults.Tag, out CvmResults? cvmResults))
            buffer.Add(cvmResults!);
        if (database.TryGet(DataRecoveryDataObjectListRelatedData.Tag,
                            out DataRecoveryDataObjectListRelatedData? dataRecoveryDataObjectListRelatedData))
            buffer.Add(dataRecoveryDataObjectListRelatedData!);
        if (database.TryGet(DataStorageSummary1.Tag, out DataStorageSummary1? dataStorageSummary1))
            buffer.Add(dataStorageSummary1!);
        if (database.TryGet(DataStorageSummaryStatus.Tag, out DataStorageSummaryStatus? dataStorageSummaryStatus))
            buffer.Add(dataStorageSummaryStatus!);
        if (database.TryGet(InterfaceDeviceSerialNumber.Tag, out InterfaceDeviceSerialNumber? interfaceDeviceSerialNumber))
            buffer.Add(interfaceDeviceSerialNumber!);
        if (database.TryGet(ProcessingOptionsDataObjectListRelatedData.Tag,
                            out ProcessingOptionsDataObjectListRelatedData? processingOptionsDataObjectListRelatedData))
            buffer.Add(processingOptionsDataObjectListRelatedData!);
        if (database.TryGet(ReferenceControlParameter.Tag, out ReferenceControlParameter? referenceControlParameter))
            buffer.Add(referenceControlParameter!);
        if (database.TryGet(TerminalCapabilities.Tag, out TerminalCapabilities? terminalCapabilities))
            buffer.Add(terminalCapabilities!);
        if (database.TryGet(TerminalCountryCode.Tag, out TerminalCountryCode? terminalCountryCode))
            buffer.Add(terminalCountryCode!);
        if (database.TryGet(TerminalType.Tag, out TerminalType? terminalType))
            buffer.Add(terminalType!);
        if (database.TryGet(TerminalVerificationResults.Tag, out TerminalVerificationResults? terminalVerificationResults))
            buffer.Add(terminalVerificationResults!);
        if (database.TryGet(TransactionCategoryCode.Tag, out TransactionCategoryCode? transactionCategoryCode))
            buffer.Add(transactionCategoryCode!);
        if (database.TryGet(TransactionCurrencyCode.Tag, out TransactionCurrencyCode? transactionCurrencyCode))
            buffer.Add(transactionCurrencyCode!);
        if (database.TryGet(TransactionDate.Tag, out TransactionDate? transactionDate))
            buffer.Add(transactionDate!);
        if (database.TryGet(TransactionTime.Tag, out TransactionTime? transactionTime))
            buffer.Add(transactionTime!);
        if (database.TryGet(UnpredictableNumber.Tag, out UnpredictableNumber? unpredictableNumber))
            buffer.Add(unpredictableNumber!);
        if (database.TryGet(TerminalRelayResistanceEntropy.Tag, out TerminalRelayResistanceEntropy? terminalRelayResistanceEntropy))
            buffer.Add(terminalRelayResistanceEntropy!);
        if (database.TryGet(DeviceRelayResistanceEntropy.Tag, out DeviceRelayResistanceEntropy? deviceRelayResistanceEntropy))
            buffer.Add(deviceRelayResistanceEntropy!);
        if (database.TryGet(MinTimeForProcessingRelayResistanceApdu.Tag,
                            out MinTimeForProcessingRelayResistanceApdu? minTimeForProcessingRelayResistanceApdu))
            buffer.Add(minTimeForProcessingRelayResistanceApdu!);
        if (database.TryGet(MaxTimeForProcessingRelayResistanceApdu.Tag,
                            out MaxTimeForProcessingRelayResistanceApdu? maxTimeForProcessingRelayResistanceApdu))
            buffer.Add(maxTimeForProcessingRelayResistanceApdu!);
        if (database.TryGet(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag,
                            out DeviceEstimatedTransmissionTimeForRelayResistanceRapdu?
                                deviceEstimatedTransmissionTimeForRelayResistanceRapdu))
            buffer.Add(deviceEstimatedTransmissionTimeForRelayResistanceRapdu!);
        if (database.TryGet(MeasuredRelayResistanceProcessingTime.Tag,
                            out MeasuredRelayResistanceProcessingTime? measuredRelayResistanceProcessingTime))
            buffer.Add(measuredRelayResistanceProcessingTime!);
        if (database.TryGet(RelayResistanceProtocolCounter.Tag, out RelayResistanceProtocolCounter? relayResistanceProtocolCounter))
            buffer.Add(relayResistanceProtocolCounter!);

        return new TornRecord(buffer.ToArray());
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public bool IsMatch(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber) => throw new NotImplementedException();

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public override DiscretionaryData Decode(TagLengthValue value)
    {
        throw new NotImplementedException();

        // TODO: Need to create this record as a list of Data Objects in EMV Book C-2 Table 4.2

        return Decode(value.EncodeValue().AsSpan());
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public static DiscretionaryData Decode(ReadOnlyMemory<byte> value)
    {
        throw new NotImplementedException();

        // TODO: Need to create this record as a list of Data Objects in EMV Book C-2 Table 4.2

        return new DiscretionaryData(_Codec.DecodePrimitiveValuesAtRuntime(value.Span).ToArray());
    }

    /// <exception cref="BerParsingException"></exception>
    public static DiscretionaryData Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodePrimitiveValuesAtRuntime(value).ToArray());

    #endregion

    #region Equality

    public bool Equals(TornRecord? x, TornRecord? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TornRecord obj) => obj.GetHashCode();

    #endregion
}