using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2PersistentValues : PersistentValues
{
    #region Instance Values

    private readonly DatabaseValues _PersistentValues;

    #endregion

    #region Constructor

    public Kernel2PersistentValues(TagLengthValue[] values)
    {
        Dictionary<Tag, DatabaseValue> persistentValues = GetDefaultValues();

        foreach (TagLengthValue value in values)
        {
            // If there is a value that is not a persistent value we will ignore it
            if (!persistentValues.ContainsKey(value.GetTag()))
                continue;

            persistentValues[value.GetTag()] = (DatabaseValue) value;
        }

        _PersistentValues = new DatabaseValues(persistentValues.Values.ToArray());
    }

    /// <summary>
    /// </summary>
    /// <param name="values">
    ///     The configurable objects for the TLV Database that will persist from transaction to transaction
    /// </param>
    public Kernel2PersistentValues(DatabaseValues values)
    {
        Dictionary<Tag, DatabaseValue> persistentValues = GetDefaultValues();

        foreach (DatabaseValue? value in values)
        {
            // If there is a value that is not a persistent value we will ignore it
            if (!persistentValues.ContainsKey(value.GetTag()))
                continue;

            persistentValues[value.GetTag()] = value;
        }

        _PersistentValues = new DatabaseValues(persistentValues.Values.ToArray());
    }

    #endregion

    #region Instance Members

    private static Dictionary<Tag, DatabaseValue> GetDefaultValues() =>
        new()
        {
            {
                KnownObjects.ApplicationVersionNumberReader,
                new DatabaseValue(new TagLengthValue(KnownObjects.ApplicationVersionNumberReader, new byte[] {0x02}))
            },
            {
                KnownObjects.AdditionalTerminalCapabilities,
                new DatabaseValue(new TagLengthValue(KnownObjects.AdditionalTerminalCapabilities, new byte[] {0x00}))
            },
            {
                KnownObjects.CardDataInputCapability,
                new DatabaseValue(new TagLengthValue(KnownObjects.CardDataInputCapability, new byte[] {0x00}))
            },
            {
                KnownObjects.CvmCapabilityCvmRequired,
                new DatabaseValue(new TagLengthValue(KnownObjects.CvmCapabilityCvmRequired, new byte[] {0x00}))
            },
            {
                KnownObjects.CvmCapabilityNoCvmRequired,
                new DatabaseValue(new TagLengthValue(KnownObjects.CvmCapabilityNoCvmRequired, new byte[] {0x00}))
            },
            {
                KnownObjects.UnpredictableNumberDataObjectsList,
                new DatabaseValue(new TagLengthValue(KnownObjects.UnpredictableNumberDataObjectsList, new byte[] {0x9F, 0x6A, 0x04}))
            },
            {KnownObjects.HoldTimeValue, new DatabaseValue(new TagLengthValue(KnownObjects.HoldTimeValue, new byte[] {0x0D}))},
            {KnownObjects.KernelConfiguration, new DatabaseValue(new TagLengthValue(KnownObjects.KernelConfiguration, new byte[] {0x00}))},
            {KnownObjects.KernelId, new DatabaseValue(new TagLengthValue(KnownObjects.KernelId, new byte[] {0x02}))},
            {
                KnownObjects.MagstripeApplicationVersionNumberReader,
                new DatabaseValue(new TagLengthValue(KnownObjects.MagstripeApplicationVersionNumberReader, new byte[] {0x01}))
            },
            {
                KnownObjects.MagstripeCvmCapabilityCvmRequired,
                new DatabaseValue(new TagLengthValue(KnownObjects.MagstripeCvmCapabilityCvmRequired, new byte[] {0xF0}))
            },
            {
                KnownObjects.MagstripeCvmCapabilityNoCvmRequired,
                new DatabaseValue(new TagLengthValue(KnownObjects.MagstripeCvmCapabilityNoCvmRequired, new byte[] {0xF0}))
            },
            {
                KnownObjects.MaxLifetimeOfTornTransactionLogRecord,
                new DatabaseValue(new TagLengthValue(KnownObjects.MaxLifetimeOfTornTransactionLogRecord, new byte[] {0x1, 0x2C}))
            },
            {
                KnownObjects.MaxNumberOfTornTransactionLogRecords,
                new DatabaseValue(new TagLengthValue(KnownObjects.MaxNumberOfTornTransactionLogRecords, new byte[] {0x00}))
            },
            {KnownObjects.MessageHoldTime, new DatabaseValue(new TagLengthValue(KnownObjects.MessageHoldTime, new byte[] {0x13}))},
            {
                KnownObjects.MaximumRelayResistanceGracePeriod,
                new DatabaseValue(new TagLengthValue(KnownObjects.MaximumRelayResistanceGracePeriod, new byte[] {0x32}))
            },
            {
                KnownObjects.MinimumRelayResistanceGracePeriod,
                new DatabaseValue(new TagLengthValue(KnownObjects.MinimumRelayResistanceGracePeriod, new byte[] {0x14}))
            },
            {
                KnownObjects.ReaderContactlessFloorLimit,
                new DatabaseValue(new TagLengthValue(KnownObjects.ReaderContactlessFloorLimit, new byte[] {0x00}))
            },
            {
                KnownObjects.ReaderContactlessTransactionLimitNoOnDeviceCvm,
                new DatabaseValue(new TagLengthValue(KnownObjects.ReaderContactlessTransactionLimitNoOnDeviceCvm, new byte[] {0x00}))
            },
            {
                KnownObjects.ReaderContactlessTransactionLimitOnDeviceCvm,
                new DatabaseValue(new TagLengthValue(KnownObjects.ReaderContactlessTransactionLimitOnDeviceCvm, new byte[] {0x00}))
            },
            {
                KnownObjects.ReaderCvmRequiredLimit,
                new DatabaseValue(new TagLengthValue(KnownObjects.ReaderCvmRequiredLimit, new byte[] {0x00}))
            },
            {
                KnownObjects.RelayResistanceAccuracyThreshold,
                new DatabaseValue(new TagLengthValue(KnownObjects.RelayResistanceAccuracyThreshold, new byte[] {0x01, 0x2C}))
            },
            {
                KnownObjects.RelayResistanceTransmissionTimeMismatchThreshold,
                new DatabaseValue(new TagLengthValue(KnownObjects.RelayResistanceTransmissionTimeMismatchThreshold, new byte[] {0x32}))
            },
            {
                KnownObjects.TerminalActionCodeDefault,
                new DatabaseValue(new TagLengthValue(KnownObjects.TerminalActionCodeDefault, new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}))
            },
            {
                KnownObjects.TerminalActionCodeDenial,
                new DatabaseValue(new TagLengthValue(KnownObjects.TerminalActionCodeDenial, new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}))
            },
            {
                KnownObjects.TerminalActionCodeDenial,
                new DatabaseValue(new TagLengthValue(KnownObjects.TerminalActionCodeDenial, new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}))
            },
            {KnownObjects.TerminalCountryCode, new DatabaseValue(new TagLengthValue(KnownObjects.TerminalCountryCode, new byte[] {0x00}))},
            {
                KnownObjects.TerminalExpectedTransmissionTimeForRelayResistanceCApdu,
                new DatabaseValue(new TagLengthValue(KnownObjects.TerminalExpectedTransmissionTimeForRelayResistanceCApdu,
                                                     new byte[] {0x00, 0x12}))
            },
            {KnownObjects.TerminalType, new DatabaseValue(new TagLengthValue(KnownObjects.TerminalType, new byte[] {0x00}))},
            {KnownObjects.TimeOutValue, new DatabaseValue(new TagLengthValue(KnownObjects.TimeOutValue, new byte[] {0x01, 0xF4}))},
            {KnownObjects.TransactionType, new DatabaseValue(new TagLengthValue(KnownObjects.TransactionType, new byte[] {0x00}))}
        };

    public override DatabaseValues GetPersistentValues() => _PersistentValues;

    #endregion
}