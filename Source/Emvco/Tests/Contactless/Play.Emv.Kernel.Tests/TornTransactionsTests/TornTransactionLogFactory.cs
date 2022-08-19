using System;

using AutoFixture;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Tests.TornTransactionsTests;

internal class TornTransactionLogFactory
{
    public static TornRecord CreateTornRecordWithPrimitives(IFixture fixture, PrimitiveValue[] primitiveValues)
    {
        ApplicationPan pan = fixture.Create<ApplicationPan>();
        ApplicationPanSequenceNumber panSequenceNumber = fixture.Create<ApplicationPanSequenceNumber>();
        PrimitiveValue[] buffer = new PrimitiveValue[primitiveValues.Length + 2];
        buffer[0] = pan;
        buffer[1] = panSequenceNumber;
        primitiveValues.CopyTo(buffer, 2);

        TornRecord tornRecord = new(buffer);

        return tornRecord;
    }

    public static TornRecord CreateDefaultTornRecord(IFixture fixture)
    {
        ApplicationPan pan = fixture.Create<ApplicationPan>();
        ApplicationPanSequenceNumber panSequenceNumber = fixture.Create<ApplicationPanSequenceNumber>();

        TornRecord record = new(pan, panSequenceNumber);

        return record;
    }

    public static TornRecord CreateDefaultTornRecordFromEncodedValues(byte[] encodedPan, byte[] encodedPanSequenceNumberss)
    {
        ApplicationPan pan = ApplicationPan.Decode(encodedPan.AsSpan());
        ApplicationPanSequenceNumber panSequenceNumber = ApplicationPanSequenceNumber.Decode(encodedPanSequenceNumberss.AsSpan());

        TornRecord record = new(pan, panSequenceNumber);

        return record;
    }
}