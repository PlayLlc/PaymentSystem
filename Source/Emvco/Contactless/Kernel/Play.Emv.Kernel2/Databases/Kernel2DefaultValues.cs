using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Globalization.Country;
using Play.Globalization.Time;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2DefaultValues : DefaultValues
{
    #region Instance Members

    /// <summary>
    ///     Initializes default values that are specified in EMVco Book 3
    /// </summary>
    public override IEnumerable<PrimitiveValue> GetDefaults(KnownObjects knownObjects)
    {
        foreach (PrimitiveValue? prim in GetKernel2Defaults())
        {
            if (knownObjects.Exists(prim.GetTag()))
                yield return prim;
        }
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private static PrimitiveValue[] GetKernel2Defaults()
    {
        return new PrimitiveValue[]
        {
            new ApplicationVersionNumberReader(0x02), new AdditionalTerminalCapabilities(0x00), new CardDataInputCapability(0x00),
            new CvmCapabilityCvmRequired(0x00), new CvmCapabilityNoCvmRequired(0x00), new UnpredictableNumberDataObjectList(new TagLength(0x9F6A, 0x04)),
            new HoldTimeValue(new Deciseconds(0x0D)), new KernelConfiguration(0x00), ShortKernelIdTypes.Kernel2,
            new MagstripeApplicationVersionNumberReader(0x01), new MagstripeCvmCapabilityCvmRequired(0xF0), new MagstripeCvmCapabilityNoCvmRequired(0xF0),
            MaxLifetimeOfTornTransactionLogRecords.Decode(new byte[] {0x1, 0x2C}.AsSpan()), new MaxNumberOfTornTransactionLogRecords(0x00),
            new MessageHoldTime(0x13), new MaximumRelayResistanceGracePeriod(0x32), new MinimumRelayResistanceGracePeriod(0x14),
            new ReaderContactlessFloorLimit(0x00), new ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice(0x00),
            new ReaderContactlessTransactionLimitWhenCvmIsOnDevice(0x00), new ReaderCvmRequiredLimit(0x00),
            RelayResistanceAccuracyThreshold.Decode(new byte[] {0x01, 0x2C}.AsSpan()), new RelayResistanceTransmissionTimeMismatchThreshold(0x32),
            TerminalActionCodeDefault.Decode(new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}.AsSpan()),
            TerminalActionCodeDenial.Decode(new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}.AsSpan()),
            TerminalActionCodeOnline.Decode(new byte[] {0x84, 0x00, 0x00, 0x00, 0x0C}.AsSpan()), new TerminalCountryCode(new NumericCountryCode(0)),
            TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Decode(new byte[] {0x00, 0x12}.AsSpan()), new TerminalType(0x00),
            new TimeoutValue(new Milliseconds(0x01F4)), new TransactionType(0x00), PhoneMessageTable.CreateKernel2Default()
        };
    }

    #endregion
}