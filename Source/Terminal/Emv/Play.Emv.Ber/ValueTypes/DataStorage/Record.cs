﻿using Play.Ber.DataObjects;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.ValueTypes.DataStorage;

/// <summary>
///     A snapshot of a transaction that has previously been processed by this payment system
/// </summary>
public class Record : IEqualityComparer<Record>, IEquatable<Record>
{
    #region Instance Values

    /// <summary>
    ///     The universal time that this record was created
    /// </summary>
    protected readonly DateTimeUtc _CommitTimeStamp;

    /// <summary>
    ///     A key that uniquely identifies a Record within a defined time period. This key could potentially cause collisions
    ///     if the maximum time threshold is not adhered to
    /// </summary>
    protected readonly RecordKey _Key;

    /// <summary>
    ///     A list of objects relevant to the transaction session that this <see cref="Record" /> belonged to
    /// </summary>
    private readonly PrimitiveValue[] _Value;

    #endregion

    #region Constructor

    protected Record(RecordKey key, PrimitiveValue[] value)
    {
        _Key = key;
        _Value = value;
        _CommitTimeStamp = DateTimeUtc.Now;
    }

    #endregion

    #region Instance Members

    public RecordKey GetKey() => _Key;
    public PrimitiveValue[] GetValues() => _Value;

    /// <exception cref="TerminalDataException"></exception>
    public static Record Create(ITlvReaderAndWriter database)
    {
        List<PrimitiveValue> buffer = new();

        if (!database.IsPresentAndNotEmpty(ApplicationPan.Tag))
        {
            throw new TerminalDataException(
                $"The {nameof(TornRecord)} could not be created because the {nameof(ITlvReaderAndWriter)} did not contain the required {nameof(ApplicationPan)} object");
        }

        if (!database.IsPresentAndNotEmpty(ApplicationPanSequenceNumber.Tag))
        {
            throw new TerminalDataException(
                $"The {nameof(TornRecord)} could not be created because the {nameof(ITlvReaderAndWriter)} did not contain the required {nameof(ApplicationPanSequenceNumber)} object");
        }

        if (database.TryGet(AmountAuthorizedNumeric.Tag, out AmountAuthorizedNumeric? amountAuthorizedNumeric))
            buffer.Add(amountAuthorizedNumeric!);
        if (database.TryGet(AmountOtherNumeric.Tag, out AmountOtherNumeric? amountOtherNumeric))
            buffer.Add(amountOtherNumeric!);
        if (database.TryGet(BalanceReadBeforeGenAc.Tag, out BalanceReadBeforeGenAc? balanceReadBeforeGenAc))
            buffer.Add(balanceReadBeforeGenAc!);
        if (database.TryGet(CardRiskManagementDataObjectList1RelatedData.Tag,
            out CardRiskManagementDataObjectList1RelatedData? cardRiskManagementDataObjectList1RelatedData))
            buffer.Add(cardRiskManagementDataObjectList1RelatedData!);
        if (database.TryGet(CvmResults.Tag, out CvmResults? cvmResults))
            buffer.Add(cvmResults!);
        if (database.TryGet(DataRecoveryDataObjectListRelatedData.Tag, out DataRecoveryDataObjectListRelatedData? dataRecoveryDataObjectListRelatedData))
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
        if (database.TryGet(MinTimeForProcessingRelayResistanceApdu.Tag, out MinTimeForProcessingRelayResistanceApdu? minTimeForProcessingRelayResistanceApdu))
            buffer.Add(minTimeForProcessingRelayResistanceApdu!);
        if (database.TryGet(MaxTimeForProcessingRelayResistanceApdu.Tag, out MaxTimeForProcessingRelayResistanceApdu? maxTimeForProcessingRelayResistanceApdu))
            buffer.Add(maxTimeForProcessingRelayResistanceApdu!);
        if (database.TryGet(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag,
            out DeviceEstimatedTransmissionTimeForRelayResistanceRapdu? deviceEstimatedTransmissionTimeForRelayResistanceRapdu))
            buffer.Add(deviceEstimatedTransmissionTimeForRelayResistanceRapdu!);
        if (database.TryGet(MeasuredRelayResistanceProcessingTime.Tag, out MeasuredRelayResistanceProcessingTime? measuredRelayResistanceProcessingTime))
            buffer.Add(measuredRelayResistanceProcessingTime!);
        if (database.TryGet(RelayResistanceProtocolCounter.Tag, out RelayResistanceProtocolCounter? relayResistanceProtocolCounter))
            buffer.Add(relayResistanceProtocolCounter!);

        ApplicationPan applicationPan = database.Get<ApplicationPan>(ApplicationPan.Tag);
        ApplicationPanSequenceNumber applicationPanSequenceNumber = database.Get<ApplicationPanSequenceNumber>(ApplicationPanSequenceNumber.Tag);

        return new Record(new RecordKey(applicationPan, applicationPanSequenceNumber), buffer.ToArray());
    }

    /// <exception cref="TerminalDataException"></exception>
    public static Record Create(PrimitiveValue[] values)
    {
        List<PrimitiveValue?> buffer = new();

        if (values.All(a => a.GetTag() != ApplicationPan.Tag))
        {
            throw new TerminalDataException(
                $"The {nameof(TornRecord)} could not be created because the {nameof(ITlvReaderAndWriter)} did not contain the required {nameof(ApplicationPan)} object");
        }

        if (values.All(a => a.GetTag() != ApplicationPanSequenceNumber.Tag))
        {
            throw new TerminalDataException(
                $"The {nameof(TornRecord)} could not be created because the {nameof(ITlvReaderAndWriter)} did not contain the required {nameof(ApplicationPanSequenceNumber)} object");
        }

        buffer.Add(values.FirstOrDefault(a => a.GetTag() == AmountAuthorizedNumeric.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == AmountOtherNumeric.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == BalanceReadBeforeGenAc.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == CardRiskManagementDataObjectList1RelatedData.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == CvmResults.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == DataRecoveryDataObjectListRelatedData.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == DataStorageSummary1.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == DataStorageSummaryStatus.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == InterfaceDeviceSerialNumber.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == ProcessingOptionsDataObjectListRelatedData.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == ReferenceControlParameter.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TerminalCapabilities.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TerminalCountryCode.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TerminalType.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TerminalVerificationResults.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TransactionCategoryCode.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TransactionCurrencyCode.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TransactionDate.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TransactionTime.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == UnpredictableNumber.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == TerminalRelayResistanceEntropy.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == DeviceRelayResistanceEntropy.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == MinTimeForProcessingRelayResistanceApdu.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == MaxTimeForProcessingRelayResistanceApdu.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == MeasuredRelayResistanceProcessingTime.Tag));
        buffer.Add(values.FirstOrDefault(a => a.GetTag() == RelayResistanceProtocolCounter.Tag));

        ApplicationPan applicationPan = (ApplicationPan) values.First(a => a.GetTag() == ApplicationPan.Tag);
        ApplicationPanSequenceNumber applicationPanSequenceNumber =
            (ApplicationPanSequenceNumber) values.First(a => a.GetTag() == ApplicationPanSequenceNumber.Tag);

        buffer.RemoveAll(a => a is null);

        return new Record(new RecordKey(applicationPan, applicationPanSequenceNumber), buffer.ToArray() as PrimitiveValue[]);
    }

    #endregion

    #region Serialization

    public static Record Decode(EmvCodec codec, ReadOnlyMemory<byte> value)
    {
        List<PrimitiveValue> buffer = new();
        EncodedTlvSiblings siblings = codec.DecodeSiblings(value);

        if (!siblings.TryGetValueOctetsOfSibling(ApplicationPan.Tag, out ReadOnlySpan<byte> applicationPanRaw))
            throw new TerminalDataException($"The {nameof(Record)} could not {nameof(Decode)} because the {nameof(ApplicationPan)} was not present");

        if (!siblings.TryGetValueOctetsOfSibling(ApplicationPanSequenceNumber.Tag, out ReadOnlySpan<byte> applicationPanSequenceNumberRaw))
        {
            throw new TerminalDataException(
                $"The {nameof(Record)} could not {nameof(Decode)} because the {nameof(ApplicationPanSequenceNumber)} was not present");
        }

        ApplicationPan applicationPan = ApplicationPan.Decode(applicationPanRaw);
        ApplicationPanSequenceNumber applicationPanSequenceNumber = ApplicationPanSequenceNumber.Decode(applicationPanSequenceNumberRaw);

        buffer.Add(applicationPan);
        buffer.Add(applicationPanSequenceNumber);

        if (siblings.TryGetValueOctetsOfSibling(AmountAuthorizedNumeric.Tag, out ReadOnlySpan<byte> amountAuthorizedNumeric))
            buffer.Add(AmountAuthorizedNumeric.Decode(amountAuthorizedNumeric));
        if (siblings.TryGetValueOctetsOfSibling(AmountOtherNumeric.Tag, out ReadOnlySpan<byte> amountOtherNumeric))
            buffer.Add(AmountOtherNumeric.Decode(amountOtherNumeric));
        if (siblings.TryGetValueOctetsOfSibling(BalanceReadBeforeGenAc.Tag, out ReadOnlySpan<byte> balanceReadBeforeGenAc))
            buffer.Add(BalanceReadBeforeGenAc.Decode(balanceReadBeforeGenAc));
        if (siblings.TryGetValueOctetsOfSibling(CardRiskManagementDataObjectList1RelatedData.Tag,
            out ReadOnlySpan<byte> cardRiskManagementDataObjectList1RelatedData))
            buffer.Add(CardRiskManagementDataObjectList1RelatedData.Decode(cardRiskManagementDataObjectList1RelatedData));
        if (siblings.TryGetValueOctetsOfSibling(CvmResults.Tag, out ReadOnlySpan<byte> cvmResults))
            buffer.Add(CvmResults.Decode(cvmResults));
        if (siblings.TryGetValueOctetsOfSibling(DataRecoveryDataObjectListRelatedData.Tag, out ReadOnlySpan<byte> dataRecoveryDataObjectListRelatedData))
            buffer.Add(DataRecoveryDataObjectListRelatedData.Decode(dataRecoveryDataObjectListRelatedData));
        if (siblings.TryGetValueOctetsOfSibling(DataStorageSummary1.Tag, out ReadOnlySpan<byte> dataStorageSummary1))
            buffer.Add(DataStorageSummary1.Decode(dataStorageSummary1));
        if (siblings.TryGetValueOctetsOfSibling(DataStorageSummaryStatus.Tag, out ReadOnlySpan<byte> dataStorageSummaryStatus))
            buffer.Add(DataStorageSummaryStatus.Decode(dataStorageSummaryStatus));
        if (siblings.TryGetValueOctetsOfSibling(InterfaceDeviceSerialNumber.Tag, out ReadOnlySpan<byte> interfaceDeviceSerialNumber))
            buffer.Add(InterfaceDeviceSerialNumber.Decode(interfaceDeviceSerialNumber));
        if (siblings.TryGetValueOctetsOfSibling(ProcessingOptionsDataObjectListRelatedData.Tag,
            out ReadOnlySpan<byte> processingOptionsDataObjectListRelatedData))
            buffer.Add(ProcessingOptionsDataObjectListRelatedData.Decode(processingOptionsDataObjectListRelatedData));
        if (siblings.TryGetValueOctetsOfSibling(ReferenceControlParameter.Tag, out ReadOnlySpan<byte> referenceControlParameter))
            buffer.Add(ReferenceControlParameter.Decode(referenceControlParameter));
        if (siblings.TryGetValueOctetsOfSibling(TerminalCapabilities.Tag, out ReadOnlySpan<byte> terminalCapabilities))
            buffer.Add(TerminalCapabilities.Decode(terminalCapabilities));
        if (siblings.TryGetValueOctetsOfSibling(TerminalCountryCode.Tag, out ReadOnlySpan<byte> terminalCountryCode))
            buffer.Add(TerminalCountryCode.Decode(terminalCountryCode));
        if (siblings.TryGetValueOctetsOfSibling(TerminalType.Tag, out ReadOnlySpan<byte> terminalType))
            buffer.Add(TerminalType.Decode(terminalType));
        if (siblings.TryGetValueOctetsOfSibling(TerminalVerificationResults.Tag, out ReadOnlySpan<byte> terminalVerificationResults))
            buffer.Add(TerminalVerificationResults.Decode(terminalVerificationResults));
        if (siblings.TryGetValueOctetsOfSibling(TransactionCategoryCode.Tag, out ReadOnlySpan<byte> transactionCategoryCode))
            buffer.Add(TransactionCategoryCode.Decode(transactionCategoryCode));
        if (siblings.TryGetValueOctetsOfSibling(TransactionCurrencyCode.Tag, out ReadOnlySpan<byte> transactionCurrencyCode))
            buffer.Add(TransactionCurrencyCode.Decode(transactionCurrencyCode));
        if (siblings.TryGetValueOctetsOfSibling(TransactionDate.Tag, out ReadOnlySpan<byte> transactionDate))
            buffer.Add(TransactionDate.Decode(transactionDate));
        if (siblings.TryGetValueOctetsOfSibling(TransactionTime.Tag, out ReadOnlySpan<byte> transactionTime))
            buffer.Add(TransactionTime.Decode(transactionTime));
        if (siblings.TryGetValueOctetsOfSibling(UnpredictableNumber.Tag, out ReadOnlySpan<byte> unpredictableNumber))
            buffer.Add(UnpredictableNumber.Decode(unpredictableNumber));
        if (siblings.TryGetValueOctetsOfSibling(TerminalRelayResistanceEntropy.Tag, out ReadOnlySpan<byte> terminalRelayResistanceEntropy))
            buffer.Add(TerminalRelayResistanceEntropy.Decode(terminalRelayResistanceEntropy));
        if (siblings.TryGetValueOctetsOfSibling(DeviceRelayResistanceEntropy.Tag, out ReadOnlySpan<byte> deviceRelayResistanceEntropy))
            buffer.Add(DeviceRelayResistanceEntropy.Decode(deviceRelayResistanceEntropy));
        if (siblings.TryGetValueOctetsOfSibling(MinTimeForProcessingRelayResistanceApdu.Tag, out ReadOnlySpan<byte> minTimeForProcessingRelayResistanceApdu))
            buffer.Add(MinTimeForProcessingRelayResistanceApdu.Decode(minTimeForProcessingRelayResistanceApdu));
        if (siblings.TryGetValueOctetsOfSibling(MaxTimeForProcessingRelayResistanceApdu.Tag, out ReadOnlySpan<byte> maxTimeForProcessingRelayResistanceApdu))
            buffer.Add(MaxTimeForProcessingRelayResistanceApdu.Decode(maxTimeForProcessingRelayResistanceApdu));
        if (siblings.TryGetValueOctetsOfSibling(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag,
            out ReadOnlySpan<byte> deviceEstimatedTransmissionTimeForRelayResistanceRapdu))
            buffer.Add(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(deviceEstimatedTransmissionTimeForRelayResistanceRapdu));
        if (siblings.TryGetValueOctetsOfSibling(MeasuredRelayResistanceProcessingTime.Tag, out ReadOnlySpan<byte> measuredRelayResistanceProcessingTime))
            buffer.Add(MeasuredRelayResistanceProcessingTime.Decode(measuredRelayResistanceProcessingTime));
        if (siblings.TryGetValueOctetsOfSibling(RelayResistanceProtocolCounter.Tag, out ReadOnlySpan<byte> relayResistanceProtocolCounter))
            buffer.Add(RelayResistanceProtocolCounter.Decode(relayResistanceProtocolCounter));

        return new Record(new RecordKey(applicationPan, applicationPanSequenceNumber), buffer.ToArray());
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is Record record && Equals(record);

    public bool Equals(Record? x, Record? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x._Key == y._Key;
    }

    public bool Equals(Record? other)
    {
        if (other is null)
            return false;

        return other._Key == _Key;
    }

    public override int GetHashCode()
    {
        const int hash = 755437;

        return hash * _Key.GetHashCode();
    }

    public int GetHashCode(Record obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(Record left, Record right) => left._Key == right._Key;
    public static bool operator !=(Record left, Record right) => left._Key != right._Key;

    #endregion
}