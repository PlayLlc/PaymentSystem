using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Globalization.Country;
using Play.Globalization.Time.Seconds;

namespace DeleteMe._Temp;

public class Kernel2PersistentValues : PersistentValues
{
    #region Instance Values

    private readonly PrimitiveValue[] _PersistentValues;

    #endregion

    #region Constructor

    public Kernel2PersistentValues(PrimitiveValue[] values)
    {
        Dictionary<Tag, PrimitiveValue> persistentValues = GetDefaultValues();

        foreach (PrimitiveValue value in values)
        {
            // If there is a value that is not a persistent value we will ignore it
            if (!persistentValues.ContainsKey(value.GetTag()))
                continue;

            persistentValues[value.GetTag()] = value;
        }

        _PersistentValues = persistentValues.Values.ToArray();
    }

    #endregion

    #region Instance Members

    private static Dictionary<Tag, PrimitiveValue> GetDefaultValues() =>
        new()
        {
            {ApplicationVersionNumberReader.Tag, new ApplicationVersionNumberReader(0x02)},
            {AdditionalTerminalCapabilities.Tag, new AdditionalTerminalCapabilities(0x00)},
            {CardDataInputCapability.Tag, new CardDataInputCapability(0x00)},
            {CvmCapabilityCvmRequired.Tag, new CvmCapabilityCvmRequired(0x00)},
            {CvmCapabilityNoCvmRequired.Tag, new CvmCapabilityNoCvmRequired(0x00)},
            {UnpredictableNumberDataObjectList.Tag, new UnpredictableNumberDataObjectList(new byte[] {0x9F, 0x6A, 0x04})},
            {HoldTimeValue.Tag, new HoldTimeValue(new Deciseconds(0x0D))},
            {KernelConfiguration.Tag, new KernelConfiguration(0x00)},
            {KernelId.Tag, (KernelId) ShortKernelIdTypes.Kernel2},
            {MagstripeApplicationVersionNumberReader.Tag, new MagstripeApplicationVersionNumberReader(0x01)},
            {MagstripeCvmCapabilityCvmRequired.Tag, new MagstripeCvmCapabilityCvmRequired(0xF0)},
            {MagstripeCvmCapabilityNoCvmRequired.Tag, new MagstripeCvmCapabilityNoCvmRequired(0xF0)},
            {
                MaxLifetimeOfTornTransactionLogRecords.Tag,
                MaxLifetimeOfTornTransactionLogRecords.Decode(new byte[] {0x1, 0x2C}.AsSpan())
            },
            {MaxNumberOfTornTransactionLogRecords.Tag, new MaxNumberOfTornTransactionLogRecords(0x00)},
            {MessageHoldTime.Tag, new MessageHoldTime(0x13)},
            {MaximumRelayResistanceGracePeriod.Tag, new MaximumRelayResistanceGracePeriod(0x32)},
            {MinimumRelayResistanceGracePeriod.Tag, new MinimumRelayResistanceGracePeriod(0x14)},
            {ReaderContactlessFloorLimit.Tag, new ReaderContactlessFloorLimit(0x00)},
            {
                ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag,
                new ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice(0x00)
            },
            {ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag, new ReaderContactlessTransactionLimitWhenCvmIsOnDevice(0x00)},
            {ReaderCvmRequiredLimit.Tag, new ReaderCvmRequiredLimit(0x00)},
            {RelayResistanceAccuracyThreshold.Tag, RelayResistanceAccuracyThreshold.Decode(new byte[] {0x01, 0x2C}.AsSpan())},
            {RelayResistanceTransmissionTimeMismatchThreshold.Tag, new RelayResistanceTransmissionTimeMismatchThreshold(0x32)},
            {TerminalActionCodeDefault.Tag, TerminalActionCodeDefault.Decode(new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}.AsSpan())},
            {TerminalActionCodeDenial.Tag, TerminalActionCodeDenial.Decode(new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}.AsSpan())},
            {TerminalActionCodeOnline.Tag, TerminalActionCodeOnline.Decode(new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}.AsSpan())},
            {TerminalCountryCode.Tag, new TerminalCountryCode(new NumericCountryCode(0))},
            {
                TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag,
                TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode(new byte[] {0x00, 0x12}.AsSpan())
            },
            {TerminalType.Tag, new TerminalType(0x00)},
            {TimeoutValue.Tag, new TimeoutValue(new Milliseconds(0x01F4))},
            {TransactionType.Tag, new TransactionType(0x00)}
        };

    public override IReadOnlyCollection<PrimitiveValue> GetPersistentValues() => _PersistentValues;

    #endregion
}