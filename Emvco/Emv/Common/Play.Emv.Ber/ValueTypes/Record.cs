using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.ValueTypes;

public record RecordKey : IComparable<RecordKey>
{
    #region Instance Values

    /// <summary>
    ///     The account number associated to this transaction snapshot
    /// </summary>
    protected readonly ApplicationPan _ApplicationPan;

    /// <summary>
    ///     A sequential number of transaction log items associated to this <see cref="PrimaryAccountNumber" />
    /// </summary>
    protected readonly ApplicationPanSequenceNumber _SequenceNumber;

    /// <summary>
    ///     Transaction date in the format YYMMDD
    /// </summary>
    protected readonly DateTimeUtc _CommitTimeStamp;

    #endregion

    #region Constructor

    public RecordKey(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber)
    {
        _ApplicationPan = pan;
        _SequenceNumber = sequenceNumber;
        _CommitTimeStamp = DateTimeUtc.Now();
    }

    #endregion

    #region Equality

    public int CompareTo(RecordKey? other) => _CommitTimeStamp.CompareTo(other._CommitTimeStamp);

    #endregion
}

public class Record : IEqualityComparer<Record>, IEquatable<Record>, IComparable<Record>
{
    #region Instance Values

    private readonly RecordKey _Key;
    private readonly PrimitiveValue[] _Value;

    #endregion

    #region Constructor

    protected Record(RecordKey key, PrimitiveValue[] value)
    {
        _Key = key;
        _Value = value;
    }

    #endregion

    #region Instance Members

    public RecordKey GetRecordKey() => _Key;

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
            out DeviceEstimatedTransmissionTimeForRelayResistanceRapdu? deviceEstimatedTransmissionTimeForRelayResistanceRapdu))
            buffer.Add(deviceEstimatedTransmissionTimeForRelayResistanceRapdu!);
        if (database.TryGet(MeasuredRelayResistanceProcessingTime.Tag,
            out MeasuredRelayResistanceProcessingTime? measuredRelayResistanceProcessingTime))
            buffer.Add(measuredRelayResistanceProcessingTime!);
        if (database.TryGet(RelayResistanceProtocolCounter.Tag, out RelayResistanceProtocolCounter? relayResistanceProtocolCounter))
            buffer.Add(relayResistanceProtocolCounter!);

        ApplicationPan applicationPan = database.Get<ApplicationPan>(ApplicationPan.Tag);
        ApplicationPanSequenceNumber applicationPanSequenceNumber =
            database.Get<ApplicationPanSequenceNumber>(ApplicationPanSequenceNumber.Tag);

        return new Record(new RecordKey(applicationPan, applicationPanSequenceNumber), buffer.ToArray());
    }

    #endregion

    #region Equality

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

    public int GetHashCode(Record obj)
    {
        const int hash = 755437;

        return hash * _Key.GetHashCode();
    }

    public int CompareTo(Record? other) => _Key.CompareTo(other?._Key);

    #endregion

    #region Operator Overrides

    public static explicit operator TornRecord(Record value) => new(value._Value);

    #endregion
}